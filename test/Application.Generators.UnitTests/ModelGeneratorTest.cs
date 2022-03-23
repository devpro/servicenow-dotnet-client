using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.UnitTests
{
    using VerifyCS = CSharpSourceGeneratorVerifier<ModelGenerator>;

    [Trait("Category", "UnitTests")]
    public class ModelGeneratorTest
    {
        [Fact]
        public async Task ModelGeneratorGenerateModel()
        {
            var original = @"
";
            var expected = @"
namespace RabbidsIncubator.ServiceNowClient.DummyProject.Domain.Models
{
    public partial class LocationModel
    {

        public string? Name { get; set; }

        public int? SomeId { get; set; }

        public bool? IsImportant { get; set; }

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
      - name: SomeId
        mapFrom: someId
        fieldType: Number
      - name: IsImportant
        mapFrom: isImportant
        fieldType: Boolean
")
                    },
                    GeneratedSources =
                    {
                        (typeof(ModelGenerator), "GeneratedLocationModel.cs", SourceText.From(expected, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                    ExpectedDiagnostics =
                    {
                        DiagnosticResult
                            .CompilerError("CS8632")
                            .WithSpan($"RabbidsIncubator.ServiceNowClient.Application.Generators{Path.DirectorySeparatorChar}RabbidsIncubator.ServiceNowClient.Application.Generators.ModelGenerator{Path.DirectorySeparatorChar}GeneratedLocationModel.cs", 7, 22, 7, 23)
                    }
                }
            };
            await test.RunAsync();
        }
    }
}
