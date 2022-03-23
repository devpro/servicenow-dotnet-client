using System.Linq;
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
            model.Entities?.ForEach(x => GenerateRepositoryInterface(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowRestClientRepository(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateSqlServerClientRepository(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateServiceNowRestClientDto(context, x, model.Namespaces));
            model.Entities?.ForEach(x => GenerateSqlServerClientDto(context, x, model.Namespaces));
        }

        private static void GenerateRepositoryInterface(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder();

            if (entity.IsCallingSqlServerDatabase())
            {
                sourceBuilder.Append($@"
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.Repositories;
using {namespaces.Root}.Domain.Models;

namespace {namespaces.Root}.Domain.Repositories
{{
    public interface I{entityPascalName}Repository : ISqlServerClientQueryRepository<{entityPascalName}Model>
    {{
    }}
}}
");
            }
            else
            {
                sourceBuilder.Append($@"
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
            }

            // inject the created source into the users compilation
            context.AddSource($"IGenerated{entityPascalName}Repository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static void GenerateServiceNowRestClientRepository(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            if (!entity.IsCallingServiceNowRestApi())
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
using RabbidsIncubator.ServiceNowClient.Domain.Diagnostics;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.Repositories;
using {namespaces.Root}.Domain.Models;
using {namespaces.Root}.Domain.Repositories;
using {namespaces.Root}.Infrastructure.ServiceNowRestClient.Dto;

namespace {namespaces.Root}.Infrastructure.ServiceNowRestClient.Repositories
{{
    public partial class {entityPascalName}Repository : ServiceNowRestClientRepositoryBase, I{entityPascalName}Repository
    {{
        public {entityPascalName}Repository(
            ILogger<{entityPascalName}Repository> logger,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            ServiceNowRestClientConfiguration restApiConfiguration,
            IMetricsContext metricsContext)
            : base(logger, httpClientFactory, mapper, restApiConfiguration, metricsContext)
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

        private static void GenerateSqlServerClientRepository(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            if (!entity.IsCallingSqlServerDatabase())
            {
                return;
            }

            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Data.SqlClient;
using RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient;
using RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.Repositories;
using {namespaces.Root}.Domain.Models;
using {namespaces.Root}.Domain.Repositories;

namespace {namespaces.Root}.Infrastructure.SqlServerClient.Repositories
{{
    public partial class {entityPascalName}Repository : SqlServerClientRepositoryBase<{entityPascalName}Model>, I{entityPascalName}Repository
    {{
        public {entityPascalName}Repository(ILogger<{entityPascalName}Repository> logger, SqlServerClientConfiguration sqlServerClientConfiguration)
            : base(logger, sqlServerClientConfiguration)
        {{
        }}

        protected override string QueryTableName => ""{entity.Queries.FindAll.SqlServerDatabaseTable}"";

        protected override string GetSelectQueryField()
        {{
            return ""\""{string.Join("\\\",\\\"", entity.Fields.Select(x => x.MapFrom))}\"""";
        }}

        protected override {entityPascalName}Model CreateModel(SqlDataReader reader)
        {{
            return new {entityPascalName}Model()
            {{
");

            foreach (var field in entity.Fields)
            {
                switch (field.FieldType)
                {
                    case Models.FieldType.String:
                        sourceBuilder.Append($@"
                {field.Name.FirstCharToUpper()} = reader[""{field.MapFrom}""].ToString(),
");
                        break;
                    case Models.FieldType.Number:
                        sourceBuilder.Append($@"
                {field.Name.FirstCharToUpper()} = reader[""{field.MapFrom}""] != null ? int.Parse(reader[""{field.MapFrom}""].ToString() ?? ""0"") : null,
");
                        break;
                    case Models.FieldType.Boolean:
                        sourceBuilder.Append($@"
                {field.Name.FirstCharToUpper()} = reader[""{field.MapFrom}""] != null ? bool.Parse(reader[""{field.MapFrom}""].ToString() ?? ""false"") : null,
");
                        break;
                }
            }

            sourceBuilder.Append(@"
            };
        }
    }
}
");

            // injects the created source into the users compilation
            context.AddSource($"GeneratedSqlServerClient{entityPascalName}Repository.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
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
