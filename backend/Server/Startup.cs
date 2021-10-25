using BL;
using BL.Builder;
using BL.Loggers;
using BL.Services;
using Common.Extentions;

using Common.Interfaces;
using Common.Interfaces.Hubs;
using Common.Interfaces.Loggers;
using Dal;
using Dal.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Hubs;

namespace Server
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
            services.AddDbContext<AirportContext>();

            services.AddScoped<IAirportRepository, AirportRepository>();
            services.AddScoped<IDataService, DataService>();

            services.AddSingleton<IFlightHub, FlightsHub>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IFlightStationMovementLogger, FlightStationMovementLogger>();
            services.AddSingleton<IAirportLogic, AirportLogic>();
            services.AddSingleton<IBuilder, StationBuilder>();

            services.AddSignalR();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors();          
            app.UseHttpsRedirection();           
            app.UseRouting();
            app.UseAuthorization();           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<FlightsHub>("/FlightsHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=airport}");
            });
        }
    }
}
