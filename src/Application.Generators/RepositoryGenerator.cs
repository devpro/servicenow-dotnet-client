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
            model.Entities?.ForEach(x => GenerateRepositoryInterface(context, x));
            model.Entities?.ForEach(x => GenerateServiceNowRepository(context, x));
            model.Entities?.ForEach(x => GenerateServiceNowDto(context, x));
        }

        private static void GenerateRepositoryInterface(GeneratorExecutionContext context, Models.EntityModel entity)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.ServiceNowClient.Domain.Repositories
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

        private static void GenerateServiceNowRepository(GeneratorExecutionContext context, Models.EntityModel entity)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories
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

        public async Task<List<{entityPascalName}Model>> FindAllAsync(QueryModel<{entityPascalName}Model> query)
        {{
            return await FindAllAsync<{entityPascalName}Model, {entityPascalName}Dto>(""{entity.Queries[0].Table}"", query);
        }}
    }}
}}
");

            // inject the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}ServiceNowRepository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static void GenerateServiceNowDto(GeneratorExecutionContext context, Models.EntityModel entity)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto
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
