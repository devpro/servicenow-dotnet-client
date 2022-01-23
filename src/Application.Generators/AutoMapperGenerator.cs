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
            var sourceBuilder = new StringBuilder(@"
using AutoMapper;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.MappingProfiles
{
    public class GeneratedServiceNowRestClientMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return ""RabbidsIncubatorServiceNowRestClientGeneratedMappingProfile""; }
        }

        public GeneratedServiceNowRestClientMappingProfile()
        {
");
            foreach (var entity in model.Entities)
            {
                sourceBuilder.Append($@"
            CreateMap<Dto.{entity.Name.FirstCharToUpper()}Dto, Domain.Models.{entity.Name.FirstCharToUpper()}Model>();
");
            }

            sourceBuilder.Append(@"
        }
    }
}
");

            // inject the created source into the users compilation
            var fileContent = SourceText.From(sourceBuilder.ToString(), Encoding.UTF8);
            context.AddSource($"GeneratedServiceNowRestClientMappingProfile.cs", fileContent);
        }
    }
}
