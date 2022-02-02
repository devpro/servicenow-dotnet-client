using AutoMapper;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Infrastructure
{
    public class InfrastructureMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "RabbidsIncubatorServiceNowRestClientMappingProfile"; }
        }

        public InfrastructureMappingProfile()
        {
            CreateMap<ConfigurationItemRelationshipDto, Domain.ConfigurationItemRelationshipModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.SysId))
                .ForMember(x => x.ParentId, opt => opt.MapFrom(x => x.Parent != null ? x.Parent.Value : null))
                .ForMember(x => x.TypeId, opt => opt.MapFrom(x => x.ConfigurationItemRelationshipType != null ? x.ConfigurationItemRelationshipType.Value : null));

            CreateMap<SwitchDto, Domain.SwitchModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.SysId));

            CreateMap<Domain.SwitchModel, SwitchDto>()
                .ForMember(x => x.SysId, opt => opt.MapFrom(x => x.Id));
        }
    }
}
