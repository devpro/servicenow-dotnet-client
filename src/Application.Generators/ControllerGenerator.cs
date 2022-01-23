using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class ControllerGenerator : GeneratorBase
    {
        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            model.Entities?.ForEach(x => GenerateController(context, x));
        }

        private static void GenerateController(GeneratorExecutionContext context, Models.EntityModel entity)
        {
            var entityCamelName = entity.Name.FirstCharToLower();
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
using Microsoft.AspNetCore.Mvc;
using RabbidsIncubator.ServiceNowClient.Domain.Models;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;

namespace RabbidsIncubator.ServiceNowClient.WebApi.Controllers
{{
    [ApiController]
    [Route(""{entity.ResourceName}"")]
    public class {entityPascalName}Controller : ControllerBase
    {{
        private readonly ILogger _logger;

        private readonly I{entityPascalName}Repository _{entityCamelName}Repository;

        public {entityPascalName}Controller(ILogger<{entityPascalName}Controller> logger, I{entityPascalName}Repository {entityCamelName}Repository)
        {{
            _logger = logger;
            _{entityCamelName}Repository = {entityCamelName}Repository;
        }}

        [HttpGet(Name = ""Get{entity.ResourceName}"")]
        public async Task<List<{entityPascalName}Model>> Get([FromQuery] {entityPascalName}Model model, int? startIndex, int? limit)
        {{
            var items = await _{entityCamelName}Repository.FindAllAsync(new QueryModel<{entityPascalName}Model>(model, startIndex, limit));
            _logger.LogDebug($""Number of items found: {{items.Count}}"");
            return items;
        }}
    }}
}}
");

            // inject the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}Controller.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
