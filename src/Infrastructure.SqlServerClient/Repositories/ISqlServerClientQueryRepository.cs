using System.Collections.Generic;
using System.Threading.Tasks;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.Repositories
{
    public interface ISqlServerClientQueryRepository<T>
        where T : class, new()
    {
        Task<List<T>> FindAllAsync(QueryModel<T> query);
    }
}
