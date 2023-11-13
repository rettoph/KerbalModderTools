using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KerbalModderTools.Library.Extensions;
using Microsoft.Extensions.Configuration;

namespace KerbalModderTools.Deploy.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureKerbalModderToolsDeployLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            return services.ConfigureKerbalModderToolsLibrary(configuration).AddSingleton<Deployer>();
        }
    }
}
