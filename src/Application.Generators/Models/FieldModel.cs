namespace RabbidsIncubator.ServiceNowClient.Application.Generators.Models
{
    /// <summary>
    /// Field type enumeration.
    /// </summary>
    public enum FieldType
    {
        String,
        Number,
        Boolean
    }

    /// <summary>
    /// Field model.
    /// </summary>
    public class FieldModel
    {
        /// <summary>
        /// Field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Field type.
        /// </summary>
        public FieldType FieldType { get; set; } = FieldType.String;

        /// <summary>
        /// Map from.
        /// </summary>
        public string MapFrom { get; set; }
    }
}
