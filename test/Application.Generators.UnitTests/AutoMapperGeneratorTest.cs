using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.UnitTests
{
    using VerifyCS = CSharpSourceGeneratorVerifier<AutoMapperGenerator>;

    [Trait("Category", "UnitTests")]
    public class AutoMapperGeneratorTest
    {
        [Fact]
        public async Task AutoMapperGeneratorGenerateCode()
        {
            var original = @"
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Infrastructure.ServiceNowRestClient.Dto
{
    public partial class LocationDto
    {
    }
}
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Models
{
    public partial class LocationModel
    {
    }
}
";

            var expectedServiceNowRest = @"
using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.DummyProject.Infrastructure.ServiceNowRestClient.MappingProfiles
{
    public partial class GeneratedServiceNowRestClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return ""RabbidsIncubatorServiceNowClientDummyProjectGeneratedServiceNowRestClientMappingProfile""; }
        }

        public GeneratedServiceNowRestClientMappingProfile()
        {

            CreateMap<Dto.LocationDto, Domain.Models.LocationModel>();
            CreateMap<Domain.Models.LocationModel, Dto.LocationDto>();

        }
    }
}
";

            var expectedSqlServer = @"
using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.DummyProject.Infrastructure.SqlServerClient.MappingProfiles
{
    public partial class GeneratedSqlServerClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return ""RabbidsIncubatorServiceNowClientDummyProjectGeneratedSqlServerClientMappingProfile""; }
        }

        public GeneratedSqlServerClientMappingProfile()
        {

        }
    }
}
";

            var test = new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { original },
                    AdditionalFiles =
                    {
                        ("entities.yml", @"
namespaces:
  root: RabbidsIncubator.ServiceNowClient.DummyProject
  webApi: RabbidsIncubator.ServiceNowClient.DummyProject
targetApplication: WebApp
entities:
  - name: Location
    resourceName: locations
    queries:
      findAll:
        serviceNowRestApiTable: cmn_location
    fields:
      - name: Name
        mapFrom: name
      - name: City
        mapFrom: city
      - name: CountryName
        mapFrom: country
      - name: Latitude
        mapFrom: latitude
      - name: Longitude
        mapFrom: longitude
")
                    },
                    AdditionalReferences =
                    {
                        typeof(AutoMapper.Profile).Assembly
                    },
                    GeneratedSources =
                    {
                        (typeof(AutoMapperGenerator), "GeneratedServiceNowRestClientMappingProfile.cs", SourceText.From(expectedServiceNowRest, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                        (typeof(AutoMapperGenerator), "GeneratedSqlServerClientMappingProfile.cs", SourceText.From(expectedSqlServer, Encoding.UTF8, SourceHashAlgorithm.Sha1))
                    }
                }
            };
            await test.RunAsync();
        }
    }
}
