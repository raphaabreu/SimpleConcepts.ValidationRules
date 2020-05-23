using System;
using System.Linq;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleConcepts.ValidationRules.AspNetTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddValidationRules<WeatherForecast>(typeof(Startup).Assembly, ServiceLifetime.Transient);
            services.AddValidationRules<WeatherForecast, DateTime>(typeof(Startup).Assembly, ServiceLifetime.Scoped);

            services.AddValidationMetrics();
            services.AddValidationLogging();

            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.ReadFrom(Configuration)
                .OutputMetrics.AsPrometheusPlainText()
                .Build();

            services.AddMetrics(metrics);
            services.AddMetricsEndpoints(endpointsOptions => endpointsOptions.MetricsEndpointOutputFormatter = endpointsOptions.MetricsOutputFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First());
            services.AddMetricsReportingHostedService();
            services.AddMetricsTrackingMiddleware(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMetricsEndpoint();
            app.UseMetricsAllMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
