using System;
using Elsa.AutoMapper.Extensions;
using Elsa.AutoMapper.Extensions.NodaTime;
using Elsa.Extensions;
using Elsa.Persistence.EntityFrameworkCore.Mapping;
using Elsa.Persistence.EntityFrameworkCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Elsa.Persistence.EntityFrameworkCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkCore(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configureOptions, bool usePooling = true)
        {
            if (usePooling)
                services.AddDbContextPool<ElsaContext>(configureOptions);
            else
                services.AddDbContext<ElsaContext>(configureOptions);

            return services
                .AddAutoMapperProfile<InstantProfile>(ServiceLifetime.Singleton)
                .AddAutoMapperProfile<DocumentProfile>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddEntityFrameworkCoreWorkflowInstanceStore(this IServiceCollection services)
        {
            return services
                .Replace<IWorkflowInstanceStore, EntityFrameworkCoreWorkflowInstanceStore>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddEntityFrameworkCoreWorkflowDefinitionStore(this IServiceCollection services)
        {
            return services
                .Replace<IWorkflowDefinitionStore, EntityFrameworkCoreWorkflowDefinitionStore>(ServiceLifetime.Scoped);
        }
    }
}