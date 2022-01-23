using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RabbidsIncubator.ServiceNowClient.Application.Generators.Extensions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    [Generator]
    public class ModelGenerator : GeneratorBase
    {
        protected override void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model)
        {
            model.Entities?.ForEach(x => GenerateModel(context, x));
        }

        private static void GenerateModel(GeneratorExecutionContext context, Models.EntityModel entity)
        {
            var entityPascalName = entity.Name.FirstCharToUpper();

            var sourceBuilder = new StringBuilder($@"
namespace RabbidsIncubator.ServiceNowClient.Domain.Models
{{
    public class {entityPascalName}Model
    {{
");
            foreach (var field in entity.Fields)
            {
                switch (field.FieldType)
                {
                    case Models.FieldType.String:
                        sourceBuilder.Append($@"
        public string? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Number:
                        sourceBuilder.Append($@"
        public int? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                    case Models.FieldType.Boolean:
                        sourceBuilder.Append($@"
        public bool? {field.Name.FirstCharToUpper()} {{ get; set; }}
");
                        break;
                }
            }

            sourceBuilder.Append(@"
    }
}");

            // inject the created source into the users compilation
            context.AddSource($"Generated{entityPascalName}Model.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
