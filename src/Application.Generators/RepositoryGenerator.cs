using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class RepositoryGenerator : GeneratorBase
    {
        protected override bool IsCompatible(Models.TargetApplicationType targetApplication)
        {
            return true;
        }

        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            // TODO: add SqlServer repository

            model.Entities?.ForEach(x => GenerateRepositoryInterface(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowRestClientRepository(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowRestClientDto(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateSqlServerClientDto(context, x, model.Namespaces));
        }

        private static void GenerateRepositoryInterface(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
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

        private static void GenerateServiceNowRestClientRepository(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            if (string.IsNullOrEmpty(entity.Queries.FindAll.ServiceNowRestApiTable))
            {
                return;
            }

            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient;
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
            return await FindAllAsync<{entityPascalName}Model, {entityPascalName}Dto>(""{entity.Queries.FindAll.ServiceNowRestApiTable}"", query, ""{entity.Queries.FindAll.Filter}"");
        }}
");
            }

            sourceBuilder.Append(@"
    }
}");

            // injects the created source into the users compilation
            context.AddSource($"GeneratedServiceNowRestClient{entityPascalName}Repository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static void GenerateServiceNowRestClientDto(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            if (string.IsNullOrEmpty(entity.Queries.FindAll.ServiceNowRestApiTable))
            {
                return;
            }

            GenerateDto(context, entity, namespaces, "ServiceNowRestClient", $"RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Dto.IEntityDto");
        }

        private static void GenerateSqlServerClientDto(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            if (string.IsNullOrEmpty(entity.Queries.FindAll.SqlServerDatabaseTable))
            {
                return;
            }

            GenerateDto(context, entity, namespaces, "SqlServerClient");
        }

        private static void GenerateDto(
            GeneratorExecutionContext context,
            Models.EntityModel entity,
            Models.NamespacesModel namespaces,
            string projectName,
            string interfaceName = "")
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var inheritance = string.IsNullOrEmpty(interfaceName) ? "" : $": {interfaceName}";

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using Newtonsoft.Json;

namespace {namespaces.Root}.Infrastructure.{projectName}.Dto
{{
    public partial class {entityPascalName}Dto {inheritance}
    {{
");

            foreach (var field in entity.Fields)
            {
                switch (field.FieldType)
                {
                    case Models.FieldType.String:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.MapFrom}"")]
        public string? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Number:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.MapFrom}"")]
        public int? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Boolean:
                        sourceBuilder.Append($@"
        [JsonProperty(""{field.MapFrom}"")]
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
                if (field.FieldType == Models.FieldType.String)
                {
                    sourceBuilder.Append($@"
            if ({field.Name.FirstCharToUpper()} != null)
            {{
                dictionary[""{field.MapFrom}""] = {field.Name.FirstCharToUpper()}.ToString();
            }}
");
                }
                else
                {
                    sourceBuilder.Append($@"
            if ({field.Name.FirstCharToUpper()} != null)
            {{
                dictionary[""{field.MapFrom}""] = {field.Name.FirstCharToUpper()}.Value.ToString();
            }}
");
                }
            }

            sourceBuilder.Append(@"
            return dictionary;
        }
    }
}");

            // inject the created source into the users compilation
            context.AddSource($"Generated{projectName}{entityPascalName}Dto.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
