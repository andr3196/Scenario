namespace Project.Domain.Events
{
    public class CreatedEvent<TEntity> : BaseEvent<TEntity>
        where TEntity : Entity
    {
        public CreatedEvent(TEntity entity) : base(entity)
        {
        }
    }
}
