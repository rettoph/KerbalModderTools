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

        public KSP_Launcher(IOptions<Constants> constants, EnvironmentLoader environment)
        {
            _ksp_x64_dbg = Path.Combine(environment.KSP_DIR, constants.Value.KSP_x64_DBG);
        }

        public void Launch()
        {
            Process.Start(_ksp_x64_dbg);

            Console.WriteLine("KSP has been started. To debug open Debug > Attach Unity Debugger");
        }
    }
}
