using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContactListApp.DataAccess {
    public static class FactoryExtensions {
        public static IServiceCollection AddDbContextFactory<TContext>(
            this IServiceCollection collection,
            Action<DbContextOptionsBuilder> optionsAction = null,
            ServiceLifetime contextAndOptionsLifetime = ServiceLifetime.Singleton)
            where TContext : DbContext {
            collection.Add(new ServiceDescriptor(
                typeof(DbContextFactory<TContext>),
                sp => new DbContextFactory<TContext>(sp),
                contextAndOptionsLifetime));

            collection.Add(new ServiceDescriptor(
                typeof(DbContextOptions<TContext>),
                sp => GetOptions<TContext>(optionsAction, sp),
                contextAndOptionsLifetime));

            return collection;
        }

        private static DbContextOptions<TContext> GetOptions<TContext>(
            Action<DbContextOptionsBuilder> action,
            IServiceProvider sp = null) where TContext : DbContext {
            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
            if (sp != null) optionsBuilder.UseApplicationServiceProvider(sp);
            action?.Invoke(optionsBuilder);
            return optionsBuilder.Options;
        }
    }
}