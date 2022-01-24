using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators
{
    /// <summary>
    /// Base class for all code generators.
    /// </summary>
    public abstract class GeneratorBase : ISourceGenerator
    {
        // ISourceGenerator methods

        public void Execute(GeneratorExecutionContext context)
        {
            var files = GetMappingFiles(context);
            files?.ToList().ForEach(x =>
            {
                var yml = File.ReadAllText(x);

                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var model = deserializer.Deserialize<Models.GenerationConfigurationModel>(yml);

                GenerateCode(context, model);
            });
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        // Protected methods

        protected abstract void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model);

        // Private methods

        private static IEnumerable<string> GetMappingFiles(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                if (!Path.GetExtension(file.Path).Equals(".yml", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                yield return file.Path;
            }
        }
    }
}
