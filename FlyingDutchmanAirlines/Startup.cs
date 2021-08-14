using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlyingDutchmanAirlines
{
    class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient(typeof(BookingService), typeof(BookingService));
            services.AddTransient(typeof(FlightService), typeof(FlightService));
            services.AddTransient(typeof(AirportRepository), typeof(AirportRepository));
            services.AddTransient(typeof(BookingRepository), typeof(BookingRepository));
            services.AddTransient(typeof(CustomerRepository), typeof(CustomerRepository));
            services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
            
            services.AddTransient(typeof(FlyingDutchmanAirlinesContext), 
                typeof(FlyingDutchmanAirlinesContext));
        }
    }
}