using System.Net;

namespace Powerplant.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseConfiguration(new ConfigurationBuilder().Build())
                        .UseKestrel(options =>
                        {
                            options.Listen(IPAddress.Loopback, 8888);
                        })
                    .UseDefaultServiceProvider(options => options.ValidateScopes = false);
                });
        }
    }
}