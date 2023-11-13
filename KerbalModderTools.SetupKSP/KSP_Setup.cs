using KerbalModderTools.Library;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.SetupKSP
{
    public class KSP_Setup
    {
        private const string WindowsPlayerEXE = "WindowsPlayer.exe";
        private const string UnityPlayerDLL = "UnityPlayer.dll";
        private const string WinPixEventRuntimeDLL = "WinPixEventRuntime.dll";
        private const string KSP_x64_Data = "KSP_x64_Data";
        private const string KSP_x64_Dbg_Data = "KSP_x64_Dbg_Data";
        private const string BootConfig = "boot.config";

        private readonly EnvironmentLoader _environment;
        private readonly Constants _constants;

        public KSP_Setup(IOptions<Constants> constants, EnvironmentLoader environment)
        {
            _constants = constants.Value;
            _environment = environment;
        }

        public void Setup()
        {
            KSP_Setup.CheckDeleteExistingInstance(_environment.KSP_DIR);
            KSP_Setup.CopySteamInstanceToDevInstance(_constants.STEAM_KSP_DIR, _environment.KSP_DIR);
            KSP_Setup.CopyUnityDebugFilesToDevInstance(_constants.UNITY_DIR, _environment.KSP_DIR, _constants.KSP_x64_DBG);
            KSP_Setup.CreateJunction(_environment.KSP_DIR);

            Console.WriteLine($"Setup complete. Run '{Path.Combine(_environment.KSP_DIR, _constants.KSP_x64_DBG)}' to debug KSP.");
        }

        private static void CheckDeleteExistingInstance(string ksp_dir)
        {
            if (Directory.Exists(ksp_dir))
            {
                Console.WriteLine("Deleting Existing KSP Dev Install...");
                try
                {
                    Directory.Delete(ksp_dir, true);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Warning: " + e.Message);
                }
            }
        }

        private static void CopySteamInstanceToDevInstance(string steam_dir, string dev_dir)
        {
            Directory.CreateDirectory(dev_dir);

            if(Directory.Exists(steam_dir) == false)
            {
                throw new DirectoryNotFoundException(steam_dir);
            }

            DirectoryHelper.CopyRecursive(steam_dir, dev_dir);
        }

        private static void CopyUnityDebugFilesToDevInstance(string unity_dir, string dev_dir, string ksp_x64_dbg)
        {
            File.AppendAllText(Path.Combine(dev_dir, KSP_x64_Data, BootConfig), "player-connection-debug=1");

            File.Copy(Path.Combine(unity_dir, WindowsPlayerEXE), Path.Combine(dev_dir, ksp_x64_dbg));
            File.Copy(Path.Combine(unity_dir, UnityPlayerDLL), Path.Combine(dev_dir, UnityPlayerDLL), true);
            File.Copy(Path.Combine(unity_dir, WinPixEventRuntimeDLL), Path.Combine(dev_dir, WinPixEventRuntimeDLL));
        }

        private static void CreateJunction(string ksp_dir)
        {
            Process.Start("cmd.exe", $"/C mklink /J \"{Path.Combine(ksp_dir, KSP_x64_Dbg_Data)}\" \"{Path.Combine(ksp_dir, KSP_x64_Data)}\"");
        }
    }
}
