namespace ECS;

public interface IComponentSet
{
    public bool Has(Entity entity);
    public IEnumerable<Entity> Entities();
    public int Count();
}