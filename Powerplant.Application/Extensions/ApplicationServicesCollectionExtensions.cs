using Microsoft.Extensions.DependencyInjection;
using Powerplant.Application.Services;

namespace Powerplant.Application.Extensions
{
    public static class ApplicationServicesCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IProductionPlanService, ProductionPlanService>();
        }
    }
}
