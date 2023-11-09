using Microsoft.ApplicationInsights.Extensibility;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Infrastructure;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Service;
using Ofgem.API.GBI.AddressVerification.Infrastructure;
using Ofgem.API.GBI.AddressVerification.Service;

namespace Ofgem.API.GBI.AddressVerification.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthorization();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IOsPlacesApiClient, OsPlacesApiClient>();
            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitialiser>();
            services.AddApplicationInsightsTelemetry(configuration.GetSection("APPINSIGHTS_CONNECTIONSTRING"));


            return services;
        }
    }
}
