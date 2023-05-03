using Powerplant.Domain.Models;

namespace Powerplant.Application.Services
{
    public interface IProductionPlanService
    {
        public List<ProductionPlanResponse> CalculateProductionPlan(uint load, Fuels fuels, List<Powerplant.Domain.Models.Powerplant> powerplants);
    }
}
