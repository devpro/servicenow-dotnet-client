using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    /// <summary>
    /// Configuration model for the generation of classes.
    /// </summary>
    public class GenerationConfigurationModel
    {
        /// <summary>
        /// Entities.
        /// </summary>
        public List<EntityModel> Entities { get; set; }
    }
}
