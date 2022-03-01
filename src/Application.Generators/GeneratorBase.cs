using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        // Protected methods

        protected abstract bool IsCompatible(Models.TargetApplicationType targetApplication);

        protected abstract void GenerateCode(GeneratorExecutionContext context, Models.GenerationConfigurationModel model);

        protected virtual void EnableDebug()
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif
        }

        // Public methods

        public void Execute(GeneratorExecutionContext context)
        {
            var files = GetMappingFiles(context.AdditionalFiles);
            files?.ToList().ForEach(file =>
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var model = deserializer.Deserialize<Models.GenerationConfigurationModel>(file.GetText().ToString());

                if (IsCompatible(model.TargetApplication))
                {
                    GenerateCode(context, model);
                }
            });
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // initialization is not needed here
        }

        // Private methods

        private static IEnumerable<AdditionalText> GetMappingFiles(ImmutableArray<AdditionalText> additionalFiles)
        {
            if (additionalFiles == null || !additionalFiles.Any())
            {
                yield break;
            }

            foreach (var file in additionalFiles)
            {
                if (!Path.GetExtension(file.Path).Equals(".yml", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                yield return file;
            }
        }
    }
}
