using System.Collections.Generic;
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
        protected override bool IsCompatible(Models.TargetApplicationType targetApplication)
        {
            return true;
        }

        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            if (model.Entities == null)
            {
                return;
            }

            GenerateMappingProfile(
                context,
                model.Namespaces.Root,
                "ServiceNowRestClient",
                model.Entities.Where(x => !string.IsNullOrEmpty(x.Queries.FindAll.ServiceNowRestApiTable)).Select(x => x.Name).ToList());

            GenerateMappingProfile(
                context,
                model.Namespaces.Root,
                "SqlServerClient",
                model.Entities.Where(x => !string.IsNullOrEmpty(x.Queries.FindAll.SqlServerDatabaseTable)).Select(x => x.Name).ToList());
        }

        private void GenerateMappingProfile(
            GeneratorExecutionContext context,
            string rootNamespace,
            string projectName,
            List<string> entityNames)
        {
            var sourceBuilder = new StringBuilder($@"
using AutoMapper;

namespace {rootNamespace}.Infrastructure.{projectName}.MappingProfiles
{{
    public partial class Generated{projectName}MappingProfile : Profile
    {{
        public override string ProfileName
        {{
            get {{ return ""{rootNamespace.Replace(".", "")}Generated{projectName}MappingProfile""; }}
        }}

        public Generated{projectName}MappingProfile()
        {{
");
            foreach (var entityName in entityNames)
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

            // injects the created source into the users compilation
            context.AddSource($"Generated{projectName}MappingProfile.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
