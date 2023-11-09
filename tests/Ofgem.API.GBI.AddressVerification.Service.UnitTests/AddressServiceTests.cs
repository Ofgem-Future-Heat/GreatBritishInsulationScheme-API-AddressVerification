using AutoMapper;
using Moq;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Service;
using Ofgem.API.GBI.AddressVerification.Application.DTOs;
using Ofgem.API.GBI.AddressVerification.Domain;
using Ofgem.API.GBI.AddressVerification.Application.Models;
using Ofgem.API.GBI.AddressVerification.Application.Contracts.Infrastructure;
using Ofgem.API.GBI.AddressVerification.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace Ofgem.API.GBI.AddressVerification.Service.UnitTests
{
    public class AddressServiceTests
    {
        private readonly IAddressService _addressService;
        private readonly Mock<IOsPlacesApiClient> _osApiClient;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<AddressService>> _logger;
        private readonly IEnumerable<Address> _mapResponse = new List<Address>() { };
        private const string _queryValue = "12345";
        private readonly OsApiQuery _mapQueryResponse = new() { Query = _queryValue };

        public AddressServiceTests()
        {
            _osApiClient = new();
            _mapper = new();
            _logger = new();
            _addressService = new AddressService(_osApiClient.Object, _mapper.Object, _logger.Object);

            _osApiClient.Reset();
            _mapper.Reset();

            _osApiClient.Setup(os => os.FindByUprn(It.IsAny<OsApiQuery>()))
                .ReturnsAsync(new OsApiResponse()
                {
                    Results = new List<AddressResultContainer>()
                    {
                        new AddressResultContainer() { }
                    }
                });

            _osApiClient.Setup(os => os.FindByPostcode(It.IsAny<OsApiQuery>()))
                .ReturnsAsync(new OsApiResponse()
                {
                    Results = new List<AddressResultContainer>()
                    {
                        new AddressResultContainer() { }
                    }
                });

            _osApiClient.Setup(os => os.FindBySearchQuery(It.IsAny<OsApiQuery>()))
                .ReturnsAsync(new OsApiResponse()
                {
                    Results = new List<AddressResultContainer>()
                    {
                        new AddressResultContainer() { }
                    }
                });

            _mapper.Setup(mapper => mapper.Map<IEnumerable<Address>>(It.IsAny<IEnumerable<AddressResult>>()))
                .Returns(_mapResponse);

            _mapper.Setup(mapper => mapper.Map<OsApiQuery>(It.IsAny<AddressQuery>()))
                .Returns(_mapQueryResponse);
        }

        [Fact]
        public async Task FindByUprn_Valid_MakesApiCall()
        {
            var response = await _addressService.FindByUprn(new AddressQuery() { Query = _queryValue });

            _osApiClient.Verify(os => os.FindByUprn(It.Is<OsApiQuery>(q => q == _mapQueryResponse)), Times.Once());
            Assert.Equal(_mapResponse, response);
        }

        [Fact]
        public void FindByUprn_Invalid_ThrowsValidationException()
        {
            var query = new AddressQuery()
            {
                Query = null
            };

            Assert.ThrowsAsync<AddressValidationException>(async () => {
                await _addressService.FindByUprn(query);
            });
        }

        [Fact]
        public async Task FindByPostcode_Valid_MakesApiCall()
        {
            var response = await _addressService.FindByPostcode(new AddressQuery() { Query = _queryValue });

            _osApiClient.Verify(os => os.FindByPostcode(It.Is<OsApiQuery>(q => q == _mapQueryResponse)), Times.Once());
            Assert.Equal(_mapResponse, response);
        }

        [Fact]
        public void FindByPostcode_Invalid_ThrowsValidationException()
        {
            var query = new AddressQuery()
            {
                Query = null
            };

            Assert.ThrowsAsync<AddressValidationException>(async () => {
                await _addressService.FindByPostcode(query);
            });
        }

        [Fact]
        public async Task Find_Valid_MakesApiCall()
        {
            var response = await _addressService.Find(new FindAddressQuery() { Query = _queryValue });

            _osApiClient.Verify(os => os.FindBySearchQuery(It.Is<OsApiQuery>(q => q == _mapQueryResponse)), Times.Once());
            Assert.Equal(_mapResponse, response!);
        }

        [Fact]
        public void Find_Invalid_ThrowsValidationException()
        {
            var query = new FindAddressQuery()
            {
                Query = null
            };

            Assert.ThrowsAsync<AddressValidationException>(async () => {
                await _addressService.Find(query);
            });
        }

        [Fact]
        public async Task Validate_MakesApiCall()
        {
            SimpleAddress address = new() { BuildingNumber = "32", Street = "Albion Street", Postcode = "G1 1LH" };
            List<AddressValidationResult> addressValidationResults = new() { new AddressValidationResult() { Address = address } };
            var response = await _addressService.ValidateAddresses(new List<SimpleAddress>() { address });
            var result = response.ToList();
            _osApiClient.Verify(os => os.FindBySearchQuery(It.Is<OsApiQuery>(q => q == _mapQueryResponse)), Times.Once());

            Assert.Equal(addressValidationResults[0].Address, result[0].Address);
        }
    }
}