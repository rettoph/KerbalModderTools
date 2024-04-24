using Microsoft.Extensions.Options;

namespace KerbalModderTools.Library
{
    public class EnvironmentLoader
    {
        private readonly EnvironmentVariables _variables;

        public string KSP_DIR => this.Get(_variables.KSP_DIR);
        public string UNITY_ENGINE_DIR => this.Get(_variables.UNITY_ENGINE_DIR);

        public EnvironmentLoader(IOptions<Constants> constants)
        {
            _variables = constants.Value.Environment;
        }

        public string Get(string variable)
        {
            string? value = Environment.GetEnvironmentVariable(variable);

            if (value == string.Empty || value is null)
            {
                throw new KeyNotFoundException($"Environment Variable '{variable}' not found.");
            }

            return value;
        }
    }
}
