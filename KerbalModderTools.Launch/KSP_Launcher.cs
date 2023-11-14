﻿using KerbalModderTools.Deploy.Library;
using KerbalModderTools.Library;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Launch
{
    public class KSP_Launcher
    {
        private readonly string _ksp_x64_dbg;
        private readonly Deployer _deployer;

        public KSP_Launcher(IOptions<Constants> constants, EnvironmentLoader environment, Deployer deployer)
        {
            _ksp_x64_dbg = Path.Combine(environment.KSP_DIR, constants.Value.KSP_x64_DBG);
            _deployer = deployer;
        }

        public void Launch()
        {
            this.KillRunningInstances();

            if(_deployer.TryDeploy())
            {
                //Process.Start(_ksp_x64_dbg, "-popupwindow");
                Process.Start(_ksp_x64_dbg);

                Console.WriteLine("KSP has been started. To debug open Debug > Attach Unity Debugger");
            }
        }

        private Process KillRunningInstances()
        {
            foreach(Process process in Process.GetProcesses())
            {
                try
                {
                    if (process.MainModule.FileName.StartsWith(_ksp_x64_dbg))
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch(Exception e)
                {
                    //
                }
            }

            return null;
        }
    }
}
