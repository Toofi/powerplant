using Newtonsoft.Json;

namespace Powerplant.Domain.Models
{
    public class ProductionPlanResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("p")]
        public uint P { get; set; }

        public ProductionPlanResponse(string name, uint p)
        {
            Name = name;
            P = p;
        }
    }
}
