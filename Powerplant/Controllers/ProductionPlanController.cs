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
        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(IProductionPlanService productionPlanService, ILogger<ProductionPlanController> logger)
        {
            this._productionPlanService = productionPlanService;
            this._logger = logger;
        }

        [HttpPost]
        public IActionResult CalculateProductionPlan()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string requestBody = reader.ReadToEnd();

                ProductionPlanPayload productionPlanPayload = new ProductionPlanPayload();
                try
                {
                    productionPlanPayload = JsonConvert.DeserializeObject<ProductionPlanPayload>(requestBody);
                }
                catch (Exception)
                {
                    return this.GetBadRequest("The payload received in the request body isn't corresponding to the correct structure.");
                }
                if (string.IsNullOrEmpty(requestBody) || productionPlanPayload == null)
                {
                    return this.GetBadRequest("There is no body.");
                }
                List<ProductionPlanResponse> productionPlanResponses = this._productionPlanService.CalculateProductionPlan(productionPlanPayload.Load,
                                                                                                                           productionPlanPayload.Fuels,
                                                                                                                           productionPlanPayload.Powerplants);
                if (productionPlanResponses.Count == 0) return this.GetBadRequest("No production plan available.");
                return Ok(productionPlanResponses);
            }
        }

        private BadRequestObjectResult GetBadRequest(string message)
        {
            this._logger.LogError("{message}", message);
            return BadRequest(message);
        }
    }
}
