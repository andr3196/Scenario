using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Project.Api.Persistence;
using Project.Domain;
using Scenario.Domain.Shared.Events;

namespace Project.Api.Services
{
    public class ProjectDataProvider
    {
        private readonly ProjectDatabaseContext projectDatabaseContext;

        public ProjectDataProvider(ProjectDatabaseContext projectDatabaseContext)
        {
            this.projectDatabaseContext = projectDatabaseContext;
        }

        public object GetData<TEvent, TEntity>(TEvent @event, IEnumerable<string> includes = null)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class, IScenarioEntity
        {

            return ApplyIncludes(projectDatabaseContext.Set<TEntity>(), includes).Single(e => e.Id == @event.EntityId);
        }

        private IQueryable<TEntity> ApplyIncludes<TEntity>(IQueryable<TEntity> source, IEnumerable<string> includes)
            where TEntity : class, IScenarioEntity
        {
            if(includes == null)
            {
                return source;
            }

            foreach( var includePath in includes)
            {
                source = source.Include(includePath);
            }
            return source;
        }
    }
}
