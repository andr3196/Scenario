using Scenario.Domain.Models;

namespace Scenario.TestDataBuilder;

public abstract class DataBuilder<TEntity> where TEntity : class, new()
{
    
    private readonly Dictionary<Guid, List<Action<TEntity>>> buildActions = new Dictionary<Guid, List<Action<TEntity>>>();
    protected Guid Current = Guid.NewGuid();

    protected List<Action<TEntity>> CurrentAssignments => buildActions.ContainsKey(Current)
        ? buildActions[Current].ToList()
        : new List<Action<TEntity>>();

    public void With(Action<TEntity> assignment)
    {
        if (!buildActions.ContainsKey(Current))
        {
            buildActions.Add(Current, new List<Action<TEntity>>());
        }
        buildActions[Current].Add(assignment);
    }

    public Guid Next()
    {
        Current = Guid.NewGuid();
        buildActions.Add(Current, new List<Action<TEntity>>());
        return Current;
    }

    protected void AddRange(IEnumerable<Action<TEntity>> actions)
    {
        buildActions[Current].AddRange(actions);
    }

    protected virtual TEntity Construct()
    {
        return new TEntity();
    }

    public TEntity Build()
    {
        var result = Construct();
        buildActions[Current].ForEach(assignment => assignment(result));
        return result;
    }

    public List<TEntity> BuildAll()
    {
        return buildActions.Values
            .Select(actions => actions.Aggregate(Construct(), (result, assignment) =>
            {
                assignment(result);
                return result;
            })).ToList();
    }

}