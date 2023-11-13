using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureKerbalModderToolsLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<EnvironmentLoader>()
                .Configure<Constants>(x => configuration.Bind(nameof(Constants), x));
        }
    }
}
