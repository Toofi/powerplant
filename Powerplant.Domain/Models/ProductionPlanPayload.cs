namespace Powerplant.Domain.Models
{
    public class ProductionPlanPayload
    {
        public uint Load { get; set; }
        public Fuels Fuels { get; set; }
        public List<Powerplant> Powerplants { get; set; }
    }
}
