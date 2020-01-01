using Newtonsoft.Json;

namespace Azure.Functions.DI.Tests.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
