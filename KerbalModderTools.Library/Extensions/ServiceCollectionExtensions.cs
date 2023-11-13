using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
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
                .Configure<LoggerConfiguration>(loggerConf =>
                {
                    loggerConf.MinimumLevel.Is(Serilog.Events.LogEventLevel.Verbose);
                    loggerConf.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                })
                .AddScoped<ILogger>(p =>
                {
                    var loggerConf = p.GetRequiredService<IOptions<LoggerConfiguration>>().Value;
                    var logger = loggerConf.CreateLogger();

                    return logger;
                })
                .AddSingleton<EnvironmentLoader>()
                .Configure<Constants>(x => configuration.Bind(nameof(Constants), x));
        }
    }
}
