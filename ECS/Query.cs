namespace ECS;

public class Query
{
    public readonly List<Type> IncludedTypes = new();

    public Query Include<T>()
    {
        IncludedTypes.Add(typeof(T));
        return this;
    }

    public Query Sort(World world)
    {
        for (var i = 0; i < IncludedTypes.Count - 1; i++)
        {
            for (var j = 0; j < IncludedTypes.Count - i - 1; j++)
            {
                var firstCount = world.Count(IncludedTypes[j]);
                var secondCount = world.Count(IncludedTypes[j + 1]);

                if (firstCount <= secondCount) continue;
                
                var tempType = IncludedTypes[j];
                IncludedTypes[j] = IncludedTypes[j + 1];
                IncludedTypes[j + 1] = tempType;
            }
        }

        return this;
    }
}