﻿using System.Collections.Generic;

namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    /// <summary>
    /// Entity model.
    /// </summary>
    public class EntityModel
    {
        /// <summary>
        /// Name of the entity.
        /// Can follow PascalCase or camelCase convention.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// REST API resource name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Queries.
        /// </summary>
        public QueriesModel Queries { get; set; }

        /// <summary>
        /// Entity field definitions.
        /// </summary>
        public List<FieldModel> Fields { get; set; }
    }
}
