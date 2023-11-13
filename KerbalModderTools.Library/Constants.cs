using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Library
{
    public class Constants
    {
        public string KSP_x64_DBG { get; set; } = "ksp_x64_dbg.exe";
        public string STEAM_KSP_DIR { get; set; } = null;
        public string UNITY_DIR { get; set; } = null;

        public EnvironmentVariables Environment { get; set; } = new EnvironmentVariables();
    }
}
