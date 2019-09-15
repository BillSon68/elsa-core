using System;
using Elsa.AutoMapper.Extensions;
using Elsa.AutoMapper.Extensions.NodaTime;
using Elsa.Extensions;
using Elsa.Persistence.YesSql.Indexes;
using Elsa.Persistence.YesSql.Mapping;
using Elsa.Persistence.YesSql.Services;
using Elsa.Persistence.YesSql.StartupTasks;
using Elsa.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YesSql;
using YesSql.Indexes;

namespace Elsa.Persistence.YesSql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddYesSql(this IServiceCollection services, Action<IConfiguration> configure)
        {
            return services
                .AddSingleton(sp => StoreFactory.CreateStore(sp, configure))
                .AddSingleton<IIndexProvider, WorkflowDefinitionIndexProvider>()
                .AddSingleton<IIndexProvider, WorkflowInstanceIndexProvider>()
                .AddScoped(CreateSession)
                .AddAutoMapperProfile<InstantProfile>(ServiceLifetime.Singleton)
                .AddAutoMapperProfile<DocumentProfile>(ServiceLifetime.Singleton)
                .AddStartupTask<StoreInitializationTask>();
        }

        public static IServiceCollection AddYesSqlWorkflowInstanceStore(this IServiceCollection services)
        {
            return services
                .Replace<IWorkflowInstanceStore, YesSqlWorkflowInstanceStore>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddYesSqlWorkflowDefinitionStore(this IServiceCollection services)
        {
            return services
                .Replace<IWorkflowDefinitionStore, YesSqlWorkflowDefinitionStore>(ServiceLifetime.Scoped);
        }

        private static ISession CreateSession(IServiceProvider services)
        {
            var store = services.GetRequiredService<IStore>();
            return store.CreateSession();
        }
    }
}