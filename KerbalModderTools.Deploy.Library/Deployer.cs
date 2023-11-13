using KerbalModderTools.Library;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Deploy.Library
{
    public class Deployer
    {
        private const string DeployTXT = "Deploy.txt";
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

            string deployDirectory = Path.GetDirectoryName(configFile);
            string[] deployTargets = File.ReadAllLines(configFile);

            foreach(string deployTarget in deployTargets)
            {
                string deployTargetPath = Path.GetFullPath(Path.Combine(deployDirectory, deployTarget));
                string deployTargetOutput = Path.GetFullPath(Path.Combine(_environment.KSP_DIR, GameData, deployTarget));

                _logger.Verbose("Building {DeployTarget} at {DeployTargetOutput}", deployTarget, deployTargetOutput);

                Process.Start("cmd.exe", $"/C dotnet build \"{deployTargetPath}\" -o \"{deployTargetOutput}\" -r any").WaitForExit();
            }

            return true;
        }

        private bool TryFindDeployConfig(out string configFile)
        {
            string workingDir = Directory.GetCurrentDirectory();

            while (File.Exists(configFile = Path.Combine(workingDir, DeployTXT)) == false)
            {
                string newDir = Path.GetFullPath(Path.Combine(workingDir, ".."));
                if(newDir == workingDir)
                {
                    return false;
                }

                workingDir = newDir;
            }

            return true;
        }
    }
}
