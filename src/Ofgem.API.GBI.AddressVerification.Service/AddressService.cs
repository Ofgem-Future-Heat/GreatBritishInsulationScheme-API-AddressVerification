using AutoMapper;
using Microsoft.Extensions.Logging;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Infrastructure;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Service;
using Ofgem.API.GBI.AddressVerification.Application.DTOs;
using Ofgem.API.GBI.AddressVerification.Application.Exceptions;
using Ofgem.API.GBI.AddressVerification.Application.Models;
using Ofgem.API.GBI.AddressVerification.Application.Validators;
using Ofgem.API.GBI.AddressVerification.Domain;

namespace Ofgem.API.GBI.AddressVerification.Service
{
    public class AddressService : IAddressService
    {
        private readonly IOsPlacesApiClient _osApi;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IOsPlacesApiClient osApi, IMapper mapper, ILogger<AddressService> logger)
        {
            _osApi = osApi;
            _mapper = mapper;
            _logger = logger;
        }

        private static async Task ValidateAddressQuery(AddressQuery addressQuery, AddressQueryValidator validator)
        {
            addressQuery.SetDefaults();
            var validationResult = await validator.ValidateAsync(addressQuery);
            if (!validationResult.IsValid)
            {
                throw new AddressValidationException(validationResult.Errors.Select(e => e.ErrorMessage).Aggregate((current, next) => $"{current} {next}"));
            }
        }

        private IEnumerable<Address> HandleOsApiResponse(OsApiResponse osApiResponse)
        {
            if (osApiResponse?.Results is null || osApiResponse?.Results?.Count() == 0)
            {
                _logger.LogInformation("Address not found");
                throw new AddressNotFoundException();
            }
            var addresses = osApiResponse?.Results?.Select(x => x.Value);
            return _mapper.Map<IEnumerable<Address>>(addresses);
        }

        public async Task<IEnumerable<Address>> Find(FindAddressQuery addressQuery)
        {
            try
            {
                await ValidateAddressQuery(addressQuery, new AddressQueryValidator());
                OsApiResponse osApiResponse = await _osApi.FindBySearchQuery(_mapper.Map<OsApiQuery>(addressQuery));
                return HandleOsApiResponse(osApiResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Find failed. {message}", e.Message);
                return HandleOsApiResponse(new OsApiResponse());
            }
        }

        public async Task<IEnumerable<Address>> FindByPostcode(AddressQuery addressQuery)
        {
            try
            {
                await ValidateAddressQuery(addressQuery, new AddressQueryValidator());
                OsApiResponse osApiResponse = await _osApi.FindByPostcode(_mapper.Map<OsApiQuery>(addressQuery));
                return HandleOsApiResponse(osApiResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "FindByPostcode failed. {message}", e.Message);
                return HandleOsApiResponse(new OsApiResponse());
            }
        }

        public async Task<IEnumerable<Address>> FindByUprn(AddressQuery addressQuery)
        {
            try
            {
                await ValidateAddressQuery(addressQuery, new UprnAddressQueryValidator());
                OsApiResponse osApiResponse = await _osApi.FindByUprn(_mapper.Map<OsApiQuery>(addressQuery));
                return HandleOsApiResponse(osApiResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "FindByUprn failed. {message}", e.Message);
                return HandleOsApiResponse(new OsApiResponse());
            }
        }

        public async Task<IEnumerable<AddressValidationResult>> ValidateAddresses(IEnumerable<SimpleAddress> addresses)
        {
            try
            {
                List<AddressValidationResult> addressValidationResults = new();

                var validator = new SimpleAddressValidator();
                foreach (var address in addresses)
                {
                    var validationResult = await validator.ValidateAsync(address);
                    if (validationResult?.IsValid == false)
                    {
                        addressValidationResults.Add(new AddressValidationResult()
                        {
                            Address = address,
                            IsValid = false,
                            ErrorMessage = validationResult.Errors.Select(e => e.ErrorMessage)
                                .Aggregate((current, next) => $"{current} {next}")
                        });
                        break;
                    }

                    IEnumerable<Address>? results = null;
                    try
                    {
                        //Check by UPRN if provided
                        AddressValidationResult? uprnValidationResult = null;
                        if (!String.IsNullOrEmpty(address.Uprn))
                        {
                            var uprnResults = await FindByUprn(new AddressQuery()
                            {
                                Query = address.Uprn
                            });

                            uprnValidationResult = CheckUprnAddressResult(address, uprnResults);
                        }

                        //If no match found by UPRN, search by address
                        if (uprnValidationResult is null)
                        {
                            results = await Find(new FindAddressQuery()
                            {
                                Query = address.ToString(),
                                MatchPrecision = 2,
                                MinMatch = 0.7f,
                                MaxResults = 10,
                                Source = "LPI,DPA"
                            });

                            addressValidationResults.Add(await CheckFindAddressResultsAsync(address, results));
                        }
                        else
                        {
                            addressValidationResults.Add(uprnValidationResult);
                        }
                    }
                    catch (AddressNotFoundException ex)
                    {
                        var countryCode = await SetCountryCodeAsync(address);

                        addressValidationResults.Add(new AddressValidationResult()
                        {
                            Address = address,
                            IsValid = false,
                            ErrorMessage = ex.Message,
                            CountryCode = countryCode
                        });
                    }
                }

                return addressValidationResults;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ValidateAddresses failed. {message}", e.Message);
                return new List<AddressValidationResult>();
            }
        }

        private async Task<string?> SetCountryCodeAsync(SimpleAddress address)
        {
            //Find by Post Code
            var findPostCodeResults = await FindByPostcode(new AddressQuery() { Query = address.Postcode });
            string? countryCode = null;
            if (findPostCodeResults != null && findPostCodeResults.Any())
            {
                countryCode = findPostCodeResults.First().CountryCode;
            }

            return countryCode;
        }

        private static AddressValidationResult? CheckUprnAddressResult(SimpleAddress address, IEnumerable<Address> results)
        {
            AddressValidationResult? validationResult = null;
            foreach (var result in results)
            {
                if (address.Uprn == result.Uprn && address.Equals(result))
                {
                    validationResult = new AddressValidationResult() { Address = address, IsValid = true, Uprn = result.Uprn, CountryCode = result.CountryCode };
                    break;
                }
            }
            return validationResult;
        }

        private async Task<AddressValidationResult> CheckFindAddressResultsAsync(SimpleAddress address, IEnumerable<Address> results)
        {
            bool matchFound = false;
            AddressValidationResult validationResult = new();
            foreach (var result in results)
            {
                if (result.Match == 1 || address.ValidateFullAddress(result))
                {
                    validationResult = new AddressValidationResult() { Address = address, IsValid = true, Uprn = result.Uprn, CountryCode = result.CountryCode };
                    matchFound = true;
                    break;
                }
                else if (result.Match >= 0.9 && address.Equals(result))
                {
                    validationResult = new AddressValidationResult() { Address = address, IsValid = true, Uprn = result.Uprn, CountryCode = result.CountryCode };
                    matchFound = true;
                    break;
                }
            }
            if (!matchFound)
            {
                var countryCode = await SetCountryCodeAsync(address);
                validationResult = new AddressValidationResult() { Address = address, IsValid = false, CountryCode = countryCode };
            }

            return validationResult;
        }
    }
}