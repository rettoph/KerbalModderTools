using KerbalModderTools.Library;
using KerbalModderTools.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Launch
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
                .AddSingleton<KSP_Launcher>()
                .BuildServiceProvider();

            provider.GetRequiredService<KSP_Launcher>().Launch();
        }
    }
}
