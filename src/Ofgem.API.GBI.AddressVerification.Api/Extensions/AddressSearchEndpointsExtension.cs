using Microsoft.AspNetCore.Mvc;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Service;
using Ofgem.API.GBI.AddressVerification.Application.Models;
using Ofgem.API.GBI.AddressVerification.Service;
using System.Runtime.CompilerServices;

namespace Ofgem.API.GBI.AddressVerification.Api.Extensions
{
    public static class AddressSearchEndpointsExtension
    {
        public static void MapAddressSearchEndpoints(this WebApplication app)
        {
            app.MapPost("/AddressSearch/Uprn", async (AddressQuery addressQuery, IAddressService _addressService) => 
            {
                return Results.Ok(await _addressService.FindByUprn(addressQuery)); 
            });

            app.MapPost("/AddressSearch/Find", async ([FromBody] FindAddressQuery addressQuery, IAddressService _addressService) =>
            {
                return Results.Ok(await _addressService.Find(addressQuery));
            });

            app.MapPost("/AddressSearch/Postcode", async ([FromBody] AddressQuery addressQuery, IAddressService _addressService) =>
            {
                return Results.Ok(await _addressService.FindByPostcode(addressQuery));
            });

            app.MapPost("/AddressSearch/Validate", async ([FromBody] IEnumerable<SimpleAddress> addresses, IAddressService _addressService) =>
            {
                return Results.Ok(await _addressService.ValidateAddresses(addresses));
            });
        }
    }
}
