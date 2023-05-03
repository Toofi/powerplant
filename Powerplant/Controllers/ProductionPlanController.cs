using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Powerplant.Application.Services;
using Powerplant.Domain.Models;
using System.Text;

namespace Powerplant.API.Controllers
{
    [ApiController]
    [Route("/productionplan")]
    [Produces("application/json")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly IProductionPlanService _productionPlanService;

        public ProductionPlanController(IProductionPlanService productionPlanService)
        {
            this._productionPlanService = productionPlanService;
        }

        [HttpPost]
        public IActionResult CalculateProductionPlan()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string requestBody = reader.ReadToEnd();
                if (string.IsNullOrEmpty(requestBody))
                {
                    return BadRequest("Payload isn't corresponding.");
                }
                ProductionPlanPayload productionPlanPayload = JsonConvert.DeserializeObject<ProductionPlanPayload>(requestBody);
                List<ProductionPlanResponse> productionPlanResponses = this._productionPlanService.CalculateProductionPlan(productionPlanPayload.Load,
                                                                                                                           productionPlanPayload.Fuels,
                                                                                                                           productionPlanPayload.Powerplants);
                return Ok(productionPlanResponses);
            }
        }
    }
}
