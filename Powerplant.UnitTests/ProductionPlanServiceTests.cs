using Powerplant.Application.Services;

namespace Powerplant.UnitTests
{
    [TestClass]
    public class ProductionPlanServiceTests
    {
        private readonly IProductionPlanService _productionPlanService;

        public ProductionPlanServiceTests(IProductionPlanService productionPlanService)
        {
            this._productionPlanService = productionPlanService;
        }

        [TestMethod]
        public void PowerplantCostShouldBeProperlyCalculated()
        {
            double powerplantCost = this._productionPlanService.CalculatePowerplantCost(0.9, 70.0);
            Assert.Equals(powerplantCost, 77.78);
        }

        [TestMethod]
        public void NoFueldCostMeansNoCostForPowerplant()
        {
            double powerplantCost = this._productionPlanService.CalculatePowerplantCost(0.9, 0.0);
            Assert.Equals(powerplantCost, 0.0);
        }
    }
}