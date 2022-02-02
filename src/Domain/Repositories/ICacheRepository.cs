namespace RabbidsIncubator.ServiceNowClient.Domain.Repositories
{
    public interface ICacheRepository
    {
        void Clean(string key);

        void CleanAll();

        bool Contains<T>(string key);

        T? Get<T>(string key) where T : class;

        void Set<T>(string key, T value);
    }
}
