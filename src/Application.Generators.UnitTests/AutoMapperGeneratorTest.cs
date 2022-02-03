using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.UnitTests
{
    using VerifyCS = CSharpSourceGeneratorVerifier<AutoMapperGenerator>;

    public class AutoMapperGeneratorTest
    {
        [Fact()]
        public async Task AutoMapperGeneratorGenerateCode()
        {
            var original = @"
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Infrastructure.ServiceNowRestClient.Dto
{
    public class LocationDto
    {
    }
}
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Models
{
    public class LocationModel
    {
    }
}
";
            var expected = @"
using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.DummyProject.Infrastructure.ServiceNowRestClient.MappingProfiles
{
    public class GeneratedServiceNowRestClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return ""RabbidsIncubatorServiceNowClientDummyProjectServiceNowRestClientGeneratedMappingProfile""; }
        }

        public GeneratedServiceNowRestClientMappingProfile()
        {

            CreateMap<Dto.LocationDto, Domain.Models.LocationModel>();
            CreateMap<Domain.Models.LocationModel, Dto.LocationDto>();

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
        table: cmn_location
    fields:
      - name: Name
        serviceNowFieldName: name
      - name: City
        serviceNowFieldName: city
      - name: CountryName
        serviceNowFieldName: country
      - name: Latitude
        serviceNowFieldName: latitude
      - name: Longitude
        serviceNowFieldName: longitude
")
                    },
                    GeneratedSources =
                    {
                        (typeof(AutoMapperGenerator), "GeneratedServiceNowRestClientMappingProfile.cs", SourceText.From(expected, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    }
                }
            };
            test.TestState.AdditionalReferences.Add(typeof(AutoMapper.Profile).Assembly);
            await test.RunAsync();
        }
    }
}
