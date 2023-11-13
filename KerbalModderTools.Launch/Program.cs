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
using KerbalModderTools.Deploy.Library.Extensions;

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
                .ConfigureKerbalModderToolsDeployLibrary(configuration)
                .AddSingleton<KSP_Launcher>()
                .BuildServiceProvider();

            provider.GetRequiredService<KSP_Launcher>().Launch();
        }
    }
}
