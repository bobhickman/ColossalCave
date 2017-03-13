using ColossalCave.Engine;
using ColossalCave.Engine.ActionHandlers;
using ColossalCave.Engine.AssetProviders;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Webhook.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Webhook
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add basic authentication service configuration
            services.Configure<BasicAuthorizationModel>(Configuration.GetSection("BasicAuthorization"));

            services.AddMvc();

            // Add application services
            services.AddScoped<IAdventureContextHelper, AdventureContextHelper>();
            services.AddScoped<IMapHelper, MapHelper>();
            services.AddScoped<IResponseBuilder, ResponseBuilder>();

            // Add intent handlers
            services.AddScoped<IActionHandler, Dispatcher>();
            services.AddScoped<IControlHandler, ControlHandler>();
            services.AddScoped<IExamineHandler, ExamineHandler>();
            services.AddScoped<IInventoryHandler, InventoryHandler>();
            services.AddScoped<ILookAroundHandler, LookAroundHandler>();
            services.AddScoped<IMagicHandler, MagicHandler>();
            services.AddScoped<IMoveDirectionHandler, MoveDirectionHandler>();
            services.AddScoped<IMoveFeatureHandler, MoveFeatureHandler>();
            services.AddScoped<IMoveLocationHandler, MoveLocationHandler>();

            // Add the adventurer context
            services.AddScoped<AdventureContext>();

            // Add providers
            services.AddSingleton<IItemProvider, ItemProvider>();
            services.AddSingleton<ILocationProvider, LocationProvider>();
            services.AddSingleton<IMessageProvider, MessageProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Add basic authorization service
            app.UseMiddleware<BasicAuthorization>();

            app.UseMvc();
        }
    }
}
