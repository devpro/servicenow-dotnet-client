using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.MappingProfiles
{
    public class SqlServerClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "RabbidsIncubatorSqlServerClientMappingProfile"; }
        }

        public SqlServerClientMappingProfile()
        {
            // nothing yet
        }
    }
}
