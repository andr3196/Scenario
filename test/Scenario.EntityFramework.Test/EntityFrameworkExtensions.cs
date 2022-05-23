using Microsoft.EntityFrameworkCore;
using Scenario.TestDataBuilder;

namespace Scenario.EntityFramework.Test;

public static class EntityFrameworkExtensions
{
    public static async Task<Unbuilder<TEntity>> ExistsInDatabaseAsync<TEntity>(this DataBuilder<TEntity> builder, DbContext context) where TEntity : class, new()
    {
        var entities = builder.BuildAll();
        context.Set<TEntity>().AddRange(entities);
        await context.SaveChangesAsync();
        return new Unbuilder<TEntity>(entities, () =>
        {
            context.Set<TEntity>().RemoveRange(entities);
            return context.SaveChangesAsync();
        });
    }
}