using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SimpleConcepts.ValidationRules.AspNetTest
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
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppMetricsHostingConfiguration(options => options.MetricsEndpoint = "/internal/metrics");
                    webBuilder.ConfigureKestrel(options => options.AllowSynchronousIO = true);
                });
        }
    }
}
