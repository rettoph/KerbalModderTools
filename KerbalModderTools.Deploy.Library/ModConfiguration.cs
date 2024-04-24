using System.Text.Json.Serialization;

namespace KerbalModderTools.Deploy.Library
{
    public class ModConfiguration
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
