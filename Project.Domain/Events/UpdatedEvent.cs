namespace Project.Domain.Events
{
    public class UpdatedEvent<TEntity> : BaseEvent<TEntity>
        where TEntity : Entity
    {
        public UpdatedEvent(TEntity entity) : base(entity)
        {
        }
    }
}
