using KerbalModderTools.Library;
using KerbalModderTools.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.SetupKSP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .AddKerbalModderToolsLibraryConfigurations()
             .Build();

            IServiceProvider provider = new ServiceCollection()
                .ConfigureKerbalModderToolsLibrary(configuration)
                .AddSingleton<KSP_Setup>()
                .BuildServiceProvider();

            provider.GetRequiredService<KSP_Setup>().Setup();

            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }
    }
}
