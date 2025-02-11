using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.UnitTests
{
    using VerifyCS = CSharpSourceGeneratorVerifier<ControllerGenerator>;

    [Trait("Category", "UnitTests")]
    public class ControllerGeneratorTest
    {
        [Fact]
        public async Task ControllerGeneratorGenerateCode()
        {
            var original = @"
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Models
{
    public partial class LocationModel
    {
    }
}
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Repositories
{
    public interface ILocationRepository
    {
        Task<List<Models.LocationModel>> FindAllAsync(QueryModel<Models.LocationModel> query);
    }
}
namespace RabbidsIncubator.ServiceNowClient.Application.Mvc
{
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected ILogger Logger { get; private set; }

        protected ControllerBase(ILogger<ControllerBase> logger)
        {
            Logger = logger;
        }

        protected void ReportListCount(int count)
        {
            Logger.LogDebug(""Dummy log"");
        }
    }
}
";
            var expected = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Models;
using RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Repositories;

namespace RabbidsIncubator.ServiceNowClient.DummyProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route(""locations"")]
    public partial class LocationController : RabbidsIncubator.ServiceNowClient.Application.Mvc.ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILogger<LocationController> logger, ILocationRepository locationRepository)
            : base(logger)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet(Name = ""Getlocations"")]
        public async Task<List<LocationModel>> Get([FromQuery] LocationModel model, int? startIndex, int? limit)
        {
            var items = await _locationRepository.FindAllAsync(new QueryModel<LocationModel>(model, startIndex, limit));
            ReportListCount(items.Count);
            return items;
        }
    }
}
";

            // TODO: investigate and fix (may come from netstandard2.0 vs net9.0)
            var expectedDiagnostic = DiagnosticResult
                .CompilerError("CS1705")
                .WithArguments("RabbidsIncubator.ServiceNowClient.Domain", "RabbidsIncubator.ServiceNowClient.Domain, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null", "System.Runtime, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Runtime", "System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

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
                    ReferenceAssemblies = new ReferenceAssemblies("net6.0", new("Microsoft.NETCore.App.Ref", "6.0.0"), @"ref\net6.0")
                        .WithPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.AspNetCore.App.Ref", "6.0.0")))
                    ,
                    AdditionalReferences =
                    {
                        typeof(Domain.Repositories.ICacheRepository).Assembly
                    },
                    GeneratedSources =
                    {
                        (typeof(ControllerGenerator), "GeneratedLocationController.cs", SourceText.From(expected, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                    ExpectedDiagnostics =
                    {
                        expectedDiagnostic,
                        expectedDiagnostic,
                        expectedDiagnostic
                    }
                }
            };
            
            await test.RunAsync();
        }
    }
}
