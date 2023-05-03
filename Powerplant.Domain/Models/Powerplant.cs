using Powerplant.Domain.Enums;

namespace Powerplant.Domain.Models
{
    public class Powerplant
    {
        public string Name { get; set; }
        public PowerType Type { get; set; }
        public double Efficiency { get; set; }
        public uint Pmin { get; set; }
        public uint Pmax { get; set; }
    }
}
