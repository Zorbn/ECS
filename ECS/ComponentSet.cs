namespace ECS;

public class ComponentSet<T> : IComponentSet
{
    private readonly Dictionary<Entity, T> _components = new();

    public void Add(Entity entity, T component)
    {
        _components.Add(entity, component);
    }

    public bool Remove(Entity entity)
    {
        return _components.Remove(entity);
    }

    public bool Has(Entity entity)
    {
        return _components.ContainsKey(entity);
    }

    public T Get(Entity entity)
    {
        return _components[entity];
    }

    public int Count()
    {
        return _components.Count;
    }

    public IEnumerable<Entity> Entities()
    {
        foreach (var pair in _components)
        {
            yield return pair.Key;
        }
    }
}