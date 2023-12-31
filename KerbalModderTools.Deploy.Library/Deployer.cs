﻿using KerbalModderTools.Library;
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

            bool result = true;
            string deployDirectory = Path.GetDirectoryName(configFile);
            string[] deployTargets = File.ReadAllLines(configFile);

            foreach(string deployTarget in deployTargets)
            {
                string deployTargetPath = Path.GetFullPath(Path.Combine(deployDirectory, deployTarget));
                string deployTargetOutput = Path.GetFullPath(Path.Combine(_environment.KSP_DIR, GameData, deployTarget));

                _logger.Verbose("Building {DeployTarget} at {DeployTargetOutput}", deployTarget, deployTargetOutput);

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
                    _logger.Verbose(e.Data);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    _logger.Error(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                var exitCode = process.ExitCode;

                if(exitCode != 0)
                {
                    result &= false;
                }
            }

            return result;
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
