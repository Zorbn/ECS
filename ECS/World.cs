namespace ECS;

public class World
{
    private readonly Dictionary<Type, IComponentSet> _componentSets = new();
    private readonly List<Entity> _entityCache = new();

    public void Add<T>(Entity entity, T component)
    {
        while (true)
        {
            if (_componentSets.TryGetValue(typeof(T), out var iComponentSet))
            {
                var componentSet = (ComponentSet<T>)iComponentSet;
                componentSet.Add(entity, component);
                return;
            }

            CreateComponentSet<T>();
        }
    }

    public bool Remove<T>(Entity entity)
    {
        while (true)
        {
            if (_componentSets.TryGetValue(typeof(T), out var iComponentSet))
            {
                var componentSet = (ComponentSet<T>)iComponentSet;
                return componentSet.Remove(entity);
            }

            CreateComponentSet<T>();
        }
    }

    public bool Has<T>(Entity entity)
    {
        while (true)
        {
            if (_componentSets.TryGetValue(typeof(T), out var iComponentSet))
            {
                var componentSet = (ComponentSet<T>)iComponentSet;
                return componentSet.Has(entity);
            }

            CreateComponentSet<T>();
        }
    }

    public T Get<T>(Entity entity)
    {
        if (_componentSets.TryGetValue(typeof(T), out var iComponentSet))
        {
            var componentSet = (ComponentSet<T>)iComponentSet;
            return componentSet.Get(entity);
        }

        throw new ArgumentException($"Failed to find component ${typeof(T)} on entity!");
    }

    public int Count(Type type)
    {
        return _componentSets.TryGetValue(type, out var iComponentSet) ? iComponentSet.Count() : 0;
    }

    public IEnumerable<Entity> Query(Query query)
    {
        _entityCache.Clear();

        if (query.IncludedTypes.Count == 0)
        {
            yield break;
        }

        if (_componentSets.TryGetValue(query.IncludedTypes[0], out var firstIComponentSet))
        {
            foreach (var entity in firstIComponentSet.Entities())
            {
                _entityCache.Add(entity);
            }
        }
        else
        {
            yield break;
        }

        for (var i = 1; i < query.IncludedTypes.Count; i++)
        {
            if (_componentSets.TryGetValue(query.IncludedTypes[i], out var secondaryIComponentSet))
            {
                for (var j = _entityCache.Count - 1; j >= 0; j--)
                {
                    if (!secondaryIComponentSet.Has(_entityCache[j]))
                    {
                        _entityCache.RemoveAt(j);
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        foreach (var entity in _entityCache)
        {
            yield return entity;
        }
    }

    private void CreateComponentSet<T>()
    {
        _componentSets.Add(typeof(T), new ComponentSet<T>());
    }
}