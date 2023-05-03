using Powerplant.Domain.Enums;
using Powerplant.Domain.Models;
using System.Collections.Generic;

namespace Powerplant.Application.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public List<ProductionPlanResponse> CalculateProductionPlan(uint load, Fuels fuels, List<Domain.Models.Powerplant> powerplants)
        {
            List<(string powerplantName, double costPerMWh)> meritOrder = this.GetMeritOrder(powerplants, fuels);
            return this.GetProductionPlanResponse(load, meritOrder, powerplants, fuels.WindPercentage);
        }

        private List<(string powerplantName, double costPerMWh)> GetMeritOrder(List<Domain.Models.Powerplant> powerplants, Fuels fuels)
        {
            List<(string powerplantName, double costPerMWh)> meritOrder = new List<(string powerplantName, double costPerMWh)>();
            foreach (Domain.Models.Powerplant powerplant in powerplants)
            {
                double costPerMWh;
                switch (powerplant.Type)
                {
                    case PowerType.Windturbine:
                        costPerMWh = 0.00;
                        meritOrder.Add((powerplant.Name, costPerMWh));
                        break;
                    case PowerType.Gasfired:
                        costPerMWh = this.CalculatePowerplantCost(powerplant.Efficiency, fuels.GasEuroMwh) + this.GetCo2CostPerMWh(fuels.Co2EuroTon);
                        meritOrder.Add((powerplant.Name, costPerMWh));
                        break;
                    case PowerType.Turbojet:
                        costPerMWh = this.CalculatePowerplantCost(powerplant.Efficiency, fuels.KerosineEuroMWh);
                        meritOrder.Add((powerplant.Name, costPerMWh));
                        break;
                    default:
                        break;
                }
            }
            meritOrder.Sort((meritOrderA, meritOrderB) => meritOrderA.costPerMWh.CompareTo(meritOrderB.costPerMWh));
            return meritOrder;
        }

        private List<ProductionPlanResponse> GetProductionPlanResponse(uint load, 
            List<(string powerplantName, double costPerMWh)> meritOrder, 
            List<Domain.Models.Powerplant> powerplants, 
            uint windPercentage)
        {
            List<ProductionPlanResponse> productionPlanResponse = new List<ProductionPlanResponse>();
            foreach((string powerplantName, double costPerMWh) merit in meritOrder) 
            {
                Domain.Models.Powerplant powerplant = powerplants.Single(powerplant => powerplant.Name == merit.powerplantName);
                if (powerplant.Type == PowerType.Windturbine) powerplant.Pmax = this.CalculateWindturbinePowerAmount(powerplant.Pmax, windPercentage);
                if (load == 0)
                {
                    productionPlanResponse.Add(new ProductionPlanResponse(powerplant.Name, 0));
                }
                else if (this.IsThePowerplantCanProduceTheLeftOverLoad(load, powerplant))
                {
                    productionPlanResponse.Add(new ProductionPlanResponse(powerplant.Name, load));
                    load = 0;
                }
                else
                {
                    load -= powerplant.Pmax;
                    productionPlanResponse.Add(new ProductionPlanResponse(powerplant.Name, powerplant.Pmax));
                }
            }
            return productionPlanResponse;
        }

        private bool IsThePowerplantCanProduceTheLeftOverLoad(uint load, Domain.Models.Powerplant powerplant)
        {
            return (load < powerplant.Pmax && powerplant.Pmin <= load);
        }

        private double CalculatePowerplantCost(double efficiency, double fuelCostPerMWh)
        {
            return fuelCostPerMWh / efficiency;
        }

        private double GetCo2CostPerMWh(uint co2EuroTon)
        {
            double co2TonProducedPerMWh = 0.3;
            return co2EuroTon * co2TonProducedPerMWh;
        }

        /// <summary>
        /// Calculate how much a windturbine can produce electricity during an hour, based on the windPercentage and the windturbine pmax.
        /// </summary>
        /// <param name="pMax"></param>
        /// <param name="windPercentage"></param>
        /// <returns></returns>
        private uint CalculateWindturbinePowerAmount(uint pMax, uint windPercentage)
        {
            return (uint)(pMax * ((double)windPercentage / 100));
        }
    }
}
