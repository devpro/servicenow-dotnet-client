using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class RepositoryGenerator : GeneratorBase
    {
        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            model.Entities?.ForEach(x => GenerateRepositoryInterface(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowRepository(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowDto(context, x, model.Namespaces));
        }

        private static void GenerateRepositoryInterface(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Threading.Tasks;
using {namespaces.Root}.Domain.Models;

namespace {namespaces.Root}.Domain.Repositories
{{
    public interface I{entityPascalName}Repository
    {{
        Task<List<{entityPascalName}Model>> FindAllAsync(QueryModel<{entityPascalName}Model> query);
    }}
}}
");

            // inject the created source into the users compilation
            context.AddSource($"IGenerated{entityPascalName}Repository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static void GenerateServiceNowRepository(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories;
using {namespaces.Root}.Domain.Models;
using {namespaces.Root}.Domain.Repositories;
using {namespaces.Root}.Infrastructure.ServiceNowRestClient.Dto;

namespace {namespaces.Root}.Infrastructure.ServiceNowRestClient.Repositories
{{
    public class {entityPascalName}Repository : ServiceNowRestClientRepositoryBase, I{entityPascalName}Repository
    {{
        public {entityPascalName}Repository(
            ILogger<{entityPascalName}Repository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestClientConfiguration restApiConfiguration)
            : base(logger, httpClientFactory, mapper, restApiConfiguration)
        {{
        }}
");

            if (entity.Queries.FindAll != null)
            {
                sourceBuilder.Append($@"
        public async Task<List<{entityPascalName}Model>> FindAllAsync(QueryModel<{entityPascalName}Model> query)
        {{
            return await FindAllAsync<{entityPascalName}Model, {entityPascalName}Dto>(""{entity.Queries.FindAll.Table}"", query, ""{entity.Queries.FindAll.Filter}"");
        }}
");
            }

            sourceBuilder.Append(@"
    }
}");

            // inject the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}ServiceNowRepository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static void GenerateServiceNowDto(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using Newtonsoft.Json;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto;

namespace {namespaces.Root}.Infrastructure.ServiceNowRestClient.Dto
{{
    public partial class {entityPascalName}Dto : IEntityDto
    {{
");

            foreach (var field in entity.Fields)
            {
                switch (field.FieldType)
                {
                    case Models.FieldType.String:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.ServiceNowFieldName}"")]
        public string? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Number:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.ServiceNowFieldName}"")]
        public int? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Boolean:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.ServiceNowFieldName}"")]
        public bool? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                }
            }

            sourceBuilder.Append(@"
        public Dictionary<string, string>? ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
");

            foreach (var field in entity.Fields)
            {
                sourceBuilder.Append($@"
            if (!string.IsNullOrEmpty({field.Name.FirstCharToUpper()}))
            {{
                dictionary[""{field.ServiceNowFieldName}""] = {field.Name.FirstCharToUpper()};
            }}
");
            }

            sourceBuilder.Append(@"
            return dictionary;
        }
    }
}");

            // inject the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}ServiceNowDto.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
