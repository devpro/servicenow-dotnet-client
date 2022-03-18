using System.Collections.Generic;

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
        /// Is an authorization required to access this resource?
        /// </summary>
        public bool IsAuthorizationRequired { get; set; } = true;

        /// <summary>
        /// Queries.
        /// </summary>
        public QueriesModel Queries { get; set; }

        /// <summary>
        /// Entity field definitions.
        /// </summary>
        public List<FieldModel> Fields { get; set; }

        /// <summary>
        /// Is calling ServiceNow REST API?
        /// </summary>
        /// <returns></returns>
        public bool IsCallingServiceNowRestApi()
        {
            return !string.IsNullOrEmpty(Queries.FindAll.ServiceNowRestApiTable);
        }

        /// <summary>
        /// Is calling SQL Server database?
        /// </summary>
        /// <returns></returns>
        public bool IsCallingSqlServerDatabase()
        {
            return !string.IsNullOrEmpty(Queries.FindAll.SqlServerDatabaseTable);
        }
    }
}
