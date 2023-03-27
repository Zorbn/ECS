namespace ECS;

public readonly struct Entity : IEquatable<Entity>
{
    private readonly int _id;
    private static int _nextId;
    
    public Entity()
    {
        _id = _nextId++;
    }

    public bool Equals(Entity other)
    {
        return _id == other._id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _id;
    }
}