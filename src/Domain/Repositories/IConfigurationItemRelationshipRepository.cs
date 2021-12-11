using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.ServiceNowClient.Domain.Repositories
{
    public interface IConfigurationItemRelationshipRepository
    {
        Task<List<ConfigurationItemRelationshipModel>> FindAllAsync();
    }
}
