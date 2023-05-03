using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Powerplant.Application.Extensions;
using Powerplant.Domain.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Powerplant.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductionPlan", Version = "v1" });
            });
            services.AddHttpContextAccessor();

            services.AddApplicationServices();
            services.AddControllers();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductionPlan V1");
                c.RoutePrefix = "swagger";
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
