using Newtonsoft.Json;

namespace Powerplant.Domain.Models
{
    public class Fuels
    {
        [JsonProperty("gas(euro/MWh)")]
        public double GasEuroMwh { get; set; }

        [JsonProperty("kerosine(euro/MWh)")]
        public double KerosineEuroMWh { get; set; }

        [JsonProperty("co2(euro/ton)")]
        public uint Co2EuroTon { get; set; }

        [JsonProperty("wind(%)")]
        public uint WindPercentage { get; set; }
    }
}
