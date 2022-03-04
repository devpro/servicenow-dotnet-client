using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Domain.Models;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.SqlServerClient.Repositories
{
    public abstract class SqlServerClientRepositoryBase<T>
        where T : class, new()
    {
        protected ILogger<SqlServerClientRepositoryBase<T>> Logger { get; private set; }

        protected SqlServerClientConfiguration Configuration { get; private set; }

        protected SqlServerClientRepositoryBase(ILogger<SqlServerClientRepositoryBase<T>> logger, SqlServerClientConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        protected abstract string GetSelectQueryField();

        protected abstract T CreateModel(SqlDataReader reader);

        protected abstract string QueryTableName { get; }

        public async Task<List<T>> FindAllAsync(QueryModel<T> query)
        {
            var sql = $"SELECT {GetSelectQueryField()} FROM {QueryTableName} ORDER BY CMDB_ID OFFSET {query.StartIndex} ROWS FETCH NEXT {query.Limit} ROWS ONLY";

            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration.DataSource,
                    UserID = Configuration.UserId,
                    Password = Configuration.Password,
                    InitialCatalog = Configuration.InitialCatalog,

                };

                using var connection = new SqlConnection(builder.ConnectionString);

                connection.Open();

                var models = new List<T>();

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    models.Add(CreateModel(reader));
                }

                return models;
            }
            catch (Exception exc)
            {
                Logger.LogWarning("An error is raised on database call [DataSource={DataSource}] [ExceptionMessage={ExceptionMessage}] [SqlQuery={SqlQuery}]",
                    Configuration.DataSource, exc.Message, sql);
                throw;
            }
        }
    }
}
