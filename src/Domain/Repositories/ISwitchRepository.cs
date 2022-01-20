using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.ServiceNowClient.Domain.Repositories
{
    public interface ISwitchRepository
    {
        Task<List<SwitchModel>> FindAllAsync(QueryModel<SwitchModel> query);
    }
}
