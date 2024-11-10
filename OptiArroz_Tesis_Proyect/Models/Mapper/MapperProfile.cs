using AutoMapper;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RiceLot, RiceLotTableDTO>()
                .ForMember(dest => dest.Classification, opt => opt.MapFrom(src => src.RiceClassification != null ? src.RiceClassification.Name : string.Empty))
                .ForMember(dest => dest.LastUbication, opt => opt.MapFrom(src => src.LastUbication != null ? src.LastUbication.UbicationName() : (src.Zone != null ? src.Zone.Name : "")));

            CreateMap<RiceLot, RiceLotDetailDTO>()
                .ForMember(dest => dest.Classification, opt => opt.MapFrom(src => src.RiceClassification != null ? src.RiceClassification.Name : string.Empty))
                .ForMember(dest => dest.LastUbication, opt => opt.MapFrom(src => src.LastUbication != null ? src.LastUbication.UbicationName() : (src.Zone != null ? src.Zone.Name : "")))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.Name + " " + src.CreatedBy.LastName : ""))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State == 0 ? "INACTIVO" : "ACTIVO"))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.UpdatedBy.Name + " " + src.UpdatedBy.LastName : ""));
          

            CreateMap<Zone, ZoneDTO>()
                .ForMember(dest => dest.IdZone, opt => opt.MapFrom(src => src.IdZone))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Rows, opt => opt.MapFrom(src => src.Rows))
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(src => src.Columns))
                .ForMember(dest => dest.Ubications, opt => opt.MapFrom(src => src.Ubications));

            CreateMap<Ubication, UbicationDTO>()
                .ForMember(dest => dest.IdUbication, opt => opt.MapFrom(src => src.IdUbication))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Row))
                .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Column))
                .ForMember(dest => dest.IdCurrentRiceLot, opt => opt.MapFrom(src => src.IdCurrentRiceLot));

        }
    }
}
