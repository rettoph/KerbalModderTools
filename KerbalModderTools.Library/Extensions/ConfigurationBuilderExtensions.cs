using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalModderTools.Library.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        private static string ConstantsJSON = "Constants.json";

        public static IConfigurationBuilder AddKerbalModderToolsLibraryConfigurations(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConstantsJSON);
        }
    }
}
