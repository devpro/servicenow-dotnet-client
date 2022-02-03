using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class AutoMapperGenerator : GeneratorBase
    {
        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            var sourceBuilder = new StringBuilder($@"
using AutoMapper;

namespace {model.Namespaces.Root}.Infrastructure.ServiceNowRestClient.MappingProfiles
{{
    public class GeneratedServiceNowRestClientMappingProfile : Profile
    {{
        public override string ProfileName
        {{
            get {{ return ""{model.Namespaces.Root.Replace(".", "")}ServiceNowRestClientGeneratedMappingProfile""; }}
        }}

        public GeneratedServiceNowRestClientMappingProfile()
        {{
");
            foreach (var entityName in model.Entities?.Select(x => x.Name))
            {
                sourceBuilder.Append($@"
            CreateMap<Dto.{entityName.FirstCharToUpper()}Dto, Domain.Models.{entityName.FirstCharToUpper()}Model>();
            CreateMap<Domain.Models.{entityName.FirstCharToUpper()}Model, Dto.{entityName.FirstCharToUpper()}Dto>();
");
            }

            sourceBuilder.Append(@"
        }
    }
}
");

            // inject the created source into the users compilation
            var fileContent = SourceText.From(sourceBuilder.ToString(), Encoding.UTF8);
            context.AddSource("GeneratedServiceNowRestClientMappingProfile.cs", fileContent);
        }

        protected override bool IsCompatible(Models.TargetApplicationType targetApplication)
        {
            return true;
        }
    }
}
