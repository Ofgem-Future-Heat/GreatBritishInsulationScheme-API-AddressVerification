using AutoMapper;
using Ofgem.API.GBI.AddressVerification.Application.DTOs;
using Ofgem.API.GBI.AddressVerification.Application.Models;
using Ofgem.API.GBI.AddressVerification.Domain;

namespace Ofgem.API.GBI.AddressVerification.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FindAddressQuery, OsApiQuery>();
            //CreateMap<UprnAddressQuery, OsApiQuery>();
            CreateMap<AddressQuery, OsApiQuery>().ReverseMap();

            CreateMap<Address, LpiAddressResult>().ReverseMap()
                .ForMember(x => x.ConcatenatedAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.BuildingName, opt => opt.MapFrom(src => src.PaoText))
                .ForMember(x => x.BuildingNumber, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PaoEndNumber) ? src.PaoStartNumber : $"{src.PaoStartNumber}-{src.PaoEndNumber}"))
                .ForMember(x => x.Street, opt => opt.MapFrom(src => src.StreetDescription))
                .ForMember(x => x.Town, opt => opt.MapFrom(src => src.TownName))
                .ForMember(x => x.Postcode, opt => opt.MapFrom(src => src.PostcodeLocator));

            CreateMap<Address, DpaAddressResult>().ReverseMap()
                .ForMember(x => x.ConcatenatedAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.Street, opt => opt.MapFrom(src => src.ThoroughfareName))
                .ForMember(x => x.Town, opt => opt.MapFrom(src => src.PostTown));
        }
    }
}
