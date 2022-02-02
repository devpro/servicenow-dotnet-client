using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.Samples.ServiceNowWebApiSample.Domain
{
    public interface ISwitchRepository
    {
        Task<List<SwitchModel>> FindAllAsync(QueryModel<SwitchModel> query);
    }
}
