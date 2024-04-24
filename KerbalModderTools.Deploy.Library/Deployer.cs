using KerbalModderTools.Library;
using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace KerbalModderTools.Deploy.Library
{
    public class Deployer
    {
        private const string DeployConfig = "deploy.config.json";
        private const string GameData = "GameData";

        private readonly EnvironmentLoader _environment;
        private readonly ILogger _logger;

        public Deployer(ILogger logger, EnvironmentLoader environment)
        {
            _environment = environment;
            _logger = logger;
        }

        public bool TryDeploy()
        {
            if (this.TryFindDeployConfig(out string configFile) == false)
            {
                return false;
            }

            string deployDirectory = Path.GetDirectoryName(configFile) ?? throw new NotImplementedException();

            string modJson = File.ReadAllText(configFile);
            ModConfiguration[] mods = JsonSerializer.Deserialize<ModConfiguration[]>(modJson) ?? Array.Empty<ModConfiguration>();

            bool result = true;

            foreach (ModConfiguration mod in mods)
            {
                if (mod.Enabled == false)
                {
                    continue;
                }

                string deployTargetPath = Path.GetFullPath(Path.Combine(deployDirectory, mod.Source));
                string deployTargetOutput = Path.GetFullPath(Path.Combine(_environment.KSP_DIR, GameData, mod.Name));

                _logger.Verbose("Building {DeployTarget} at {DeployTargetOutput}", mod.Source, deployTargetOutput);

                _logger.Verbose($"dotnet build \"{deployTargetPath}\" -o \"{deployTargetOutput}\" -r any -c Debug /p:Platform=AnyCPU");
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C dotnet build \"{deployTargetPath}\" -o \"{deployTargetOutput}\" -r any -c Debug /p:Platform=AnyCPU",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data is null)
                    {
                        return;
                    }

                    _logger.Verbose(e.Data);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data is null)
                    {
                        return;
                    }

                    _logger.Error(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                var exitCode = process.ExitCode;

                if (exitCode != 0)
                {
                    result &= false;
                }
            }

            return result;
        }

        private bool TryFindDeployConfig(out string configFile)
        {
            string workingDir = Directory.GetCurrentDirectory();

            while (File.Exists(configFile = Path.Combine(workingDir, DeployConfig)) == false)
            {
                string newDir = Path.GetFullPath(Path.Combine(workingDir, ".."));
                if (newDir == workingDir)
                {
                    return false;
                }

                workingDir = newDir;
            }

            return true;
        }
    }
}
