using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class ControllerGenerator : GeneratorBase
    {
        protected override bool IsCompatible(Models.TargetApplicationType targetApplication)
        {
            return targetApplication == Models.TargetApplicationType.WebApp;
        }

        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            model.Entities?.ForEach(x => GenerateController(context, x, model.Namespaces));
        }

        private static void GenerateController(GeneratorExecutionContext context, Models.EntityModel entity, Models.NamespacesModel namespaces)
        {
            var entityCamelName = entity.Name.FirstCharToLower();
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using {namespaces.Root}.Domain.Models;
using {namespaces.Root}.Domain.Repositories;

namespace {namespaces.WebApi}.Controllers
{{");

            if (entity.IsAuthorizationRequired && string.IsNullOrEmpty(entity.AuthorizationRoles))
            {
                sourceBuilder.Append(@"
    [Authorize]");
            }
            else if (entity.IsAuthorizationRequired && !string.IsNullOrEmpty(entity.AuthorizationRoles))
            {
                sourceBuilder.Append($@"
    [Authorize(Roles = ""{entity.AuthorizationRoles}"")]");
            }

            sourceBuilder.Append($@"
    [ApiController]
    [Route(""{entity.ResourceName}"")]
    public partial class {entityPascalName}Controller : RabbidsIncubator.ServiceNowClient.Application.Mvc.ControllerBase
    {{
        private readonly I{entityPascalName}Repository _{entityCamelName}Repository;

        public {entityPascalName}Controller(ILogger<{entityPascalName}Controller> logger, I{entityPascalName}Repository {entityCamelName}Repository)
            : base(logger)
        {{
            _{entityCamelName}Repository = {entityCamelName}Repository;
        }}
");

            if (!string.IsNullOrEmpty(entity.Queries.FindAll.SqlServerDatabaseTable))
            {
                sourceBuilder.Append($@"
        [HttpGet(Name = ""Get{entity.ResourceName}"")]
        public async Task<List<{entityPascalName}Model>> Get(int? startIndex, int? limit)
        {{
            var items = await _{entityCamelName}Repository.FindAllAsync(new QueryModel<{entityPascalName}Model>(null, startIndex, limit));
            ReportListCount(items.Count);
            return items;
        }}
    }}
}}
");
            }
            else
            {
                sourceBuilder.Append($@"
        [HttpGet(Name = ""Get{entity.ResourceName}"")]
        public async Task<List<{entityPascalName}Model>> Get([FromQuery] {entityPascalName}Model model, int? startIndex, int? limit)
        {{
            var items = await _{entityCamelName}Repository.FindAllAsync(new QueryModel<{entityPascalName}Model>(model, startIndex, limit));
            ReportListCount(items.Count);
            return items;
        }}
    }}
}}
");
            }

            // injects the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}Controller.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
