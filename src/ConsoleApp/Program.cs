﻿using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbidsIncubator.ServiceNowClient.Application.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.ConsoleApp.Infrastructure.ServiceNowRestClient.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.ConsoleApp.Infrastructure.ServiceNowRestClient.MappingProfiles;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestClient.DependencyInjection;

namespace RabbidsIncubator.ServiceNowClient.ConsoleApp
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(
                    (CommandLineOptions opts) => RunOptionsAndReturnExitCode(opts),
                    errs => Task.FromResult(HandleParseError())
                 );
        }

        private async static Task<int> RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            var appConfiguration = new AppConfiguration(LoadConfiguration());
            if (!appConfiguration.ServiceNowRestClientConfiguration.IsValid())
            {
                Console.WriteLine("Missing or invalid ServiceNow configuration. Please make sure all parameters are set.");
                return -1;
            }

            using var serviceProvider = CreateServiceProvider(opts, appConfiguration);

            try
            {
                var repository = serviceProvider.GetRequiredService<Domain.Repositories.ILocationRepository>();
                var items = await repository.FindAllAsync(new ServiceNowClient.Domain.Models.QueryModel<Domain.Models.LocationModel>(null, null, null));

                Console.WriteLine($"Number of items found: {items.Count}");
                Console.WriteLine($"First item received: Id={items[0].Name}");

                return 0;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"An error occured: {exc.Message}");
                return -2;
            }
        }

        private static int HandleParseError()
        {
            return -2;
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        private static ServiceProvider CreateServiceProvider(CommandLineOptions opts, AppConfiguration appConfiguration)
        {
            LogVerbose(opts, "Creating the service provider");
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Microsoft", opts.IsVerbose ? LogLevel.Information : LogLevel.Warning)
                        .AddFilter("System", opts.IsVerbose ? LogLevel.Information : LogLevel.Warning)
                        .AddFilter("RabbidsIncubator.ServiceNowClient", opts.IsVerbose ? LogLevel.Debug : LogLevel.Information)
                        .AddConsole();
                })
                .AddServiceNowRestClientGeneratedRepositories()
                .AddServiceNowRestClientRepositories(appConfiguration.ServiceNowRestClientConfiguration)
                .AddAutoMapper(new GeneratedServiceNowRestClientMappingProfile());

            return serviceCollection.BuildServiceProvider();
        }

        private static void LogVerbose(CommandLineOptions opts, string message)
        {
            if (opts.IsVerbose)
            {
                Console.WriteLine(message);
            }
        }
    }
}
