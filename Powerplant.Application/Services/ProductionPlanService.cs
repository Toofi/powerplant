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
            return this.GetProductionPlanResponse(load, meritOrder, powerplants);
        }

        private List<ProductionPlanResponse> GetProductionPlanResponse(uint load, List<(string powerplantName, double costPerMWh)> meritOrder, List<Domain.Models.Powerplant> powerplants)
        {
            List<ProductionPlanResponse> productionPlanResponse = new List<ProductionPlanResponse>();
            foreach((string powerplantName, double costPerMWh) merit in meritOrder) 
            {
                Domain.Models.Powerplant powerplant = powerplants.Single(powerplant => powerplant.Name == merit.powerplantName);
                if (load == 0)
                {
                    productionPlanResponse.Add(new ProductionPlanResponse(powerplant.Name, 0));
                }
                else if (load < powerplant.Pmax && powerplant.Pmin <= load)
                {
                    //400 . 500 . 300
                    //400 OK
                    //300 . 500 . 400
                    //300 PAS OK
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
                        costPerMWh = this.CalculatePowerplantCost(powerplant.Efficiency, fuels.GasEuroMwh);
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

        private double CalculatePowerplantCost(double efficiency, double costFuelPerMWh)
        {
            return costFuelPerMWh / efficiency;
        }

        /// <summary>
        /// Calculate how much a windturbine can produce electricity during an hour, based on the windPercentage and the windturbine pmax.
        /// </summary>
        /// <param name="pMax"></param>
        /// <param name="windPercentage"></param>
        /// <returns></returns>
        private uint CalculateWindturbinePowerAmount(uint pMax, uint windPercentage)
        {
            return pMax * (windPercentage / 100);
        }
    }
}

