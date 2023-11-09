using Microsoft.Extensions.Configuration;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Infrastructure;
using Ofgem.API.GBI.AddressVerification.Domain;
using System.Text.Json;

namespace Ofgem.API.GBI.AddressVerification.Infrastructure
{
    public class OsPlacesApiClient : IOsPlacesApiClient
    {
        private readonly string _apiKey;
        private const string _baseUrl = "https://api.os.uk/search/places/v1/";
        private static readonly HttpClient client = new();

        public OsPlacesApiClient(IConfiguration configuration)
        {
            _apiKey = configuration["OSPlaces:ApiKey"] ?? "";
        }

        private static async Task<OsApiResponse> CallApi(string uri)
        {
            OsApiResponse? response = null;
            var streamTask = client.GetStreamAsync(uri);
            try
            {
                response = await JsonSerializer.DeserializeAsync<OsApiResponse>(await streamTask);
            
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            
            return response;
        }

        public async Task<OsApiResponse> FindBySearchQuery(OsApiQuery osApiQuery)
        {
            string uri = $"{_baseUrl}find?query={osApiQuery.Query}&key={_apiKey}&dataset={osApiQuery.Source}&minmatch={osApiQuery.MinMatch}&matchprecision={osApiQuery.MatchPrecision}&maxresults={osApiQuery.MaxResults}";
            OsApiResponse response = await CallApi(uri);
            return response;
        }

        public async Task<OsApiResponse> FindByPostcode(OsApiQuery osApiQuery)
        {
            string uri = $"{_baseUrl}postcode?postcode={osApiQuery.Query}&key={_apiKey}&dataset={osApiQuery.Source}";
            OsApiResponse response = await CallApi(uri);
            return response;
        }

        public async Task<OsApiResponse> FindByUprn(OsApiQuery osApiQuery)
        {
            string uri = $"{_baseUrl}uprn?uprn={osApiQuery.Query}&key={_apiKey}&dataset={osApiQuery.Source}";
            OsApiResponse response = await CallApi(uri);
            return response;
        }
    }
}
