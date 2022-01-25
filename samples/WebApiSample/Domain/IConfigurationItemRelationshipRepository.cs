namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain
{
    public interface IConfigurationItemRelationshipRepository
    {
        Task<List<ConfigurationItemRelationshipModel>> FindAllAsync();
    }
}
