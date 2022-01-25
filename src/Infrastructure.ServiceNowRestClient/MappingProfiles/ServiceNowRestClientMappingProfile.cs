using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.MappingProfiles
{
    public class ServiceNowRestClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "RabbidsIncubatorServiceNowRestClientMappingProfile"; }
        }

        public ServiceNowRestClientMappingProfile()
        {
            // nothing yet
        }
    }
}
