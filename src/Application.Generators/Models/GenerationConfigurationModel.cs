using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    /// <summary>
    /// Target application type.
    /// </summary>
    public enum TargetApplicationType
    {
        Console,
        WebApp
    }

    /// <summary>
    /// Configuration model for the generation of classes.
    /// </summary>
    public class GenerationConfigurationModel
    {
        /// <summary>
        /// Target application.
        /// </summary>
        public TargetApplicationType TargetApplication { get; set; }

        /// <summary>
        /// Entities.
        /// </summary>
        public List<EntityModel> Entities { get; set; }

        /// <summary>
        /// Namespaces.
        /// </summary>
        public NamespacesModel Namespaces { get; set; }
    }
}
