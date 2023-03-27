using System.Diagnostics;

namespace ECS;

internal static class Program
{
    private struct Health
    {
        public readonly int Id;

        public Health(int id)
        {
            Id = id;
        }
    }

    private struct Mana
    {
        public readonly int Id;

        public Mana(int id)
        {
            Id = id;
        }
    }

    private struct Armour
    {
        public readonly int Id;

        public Armour(int id)
        {
            Id = id;
        }
    }
    
    public static void Main()
    {
        var world = new World();
        var entityOne = new Entity();
        world.Add(entityOne, new Health(1));
        var entityTwo = new Entity();
        world.Add(entityTwo, new Health(2));
        world.Add(entityTwo, new Mana(2));
        var entityThree = new Entity();
        world.Add(entityThree, new Health(3));
        world.Add(entityThree, new Mana(3));
        world.Add(entityThree, new Armour(3));
        
        var healthQuery = new Query().Include<Health>();
        var healthAndManaQuery = new Query().Include<Health>().Include<Mana>();
        
        Console.WriteLine("Health query:");
        foreach (var entity in world.Query(healthQuery))
        {
            Console.WriteLine(world.Get<Health>(entity).Id);
        }
        
        Console.WriteLine("Health and Mana query:");
        foreach (var entity in world.Query(healthAndManaQuery))
        {
            Console.WriteLine(world.Get<Mana>(entity).Id);
        }
        
        const int extraEntityCount = 1000;
        
        for (var i = 0; i < extraEntityCount; i++)
        {
            var extraEntity = new Entity();
            world.Add(extraEntity, new Health(3));
        }
        
        for (var i = 0; i < extraEntityCount; i++)
        {
            var extraEntity = new Entity();
            world.Add(extraEntity, new Health(3));
            world.Add(extraEntity, new Mana(3));
        }

        for (var i = 0; i < extraEntityCount; i++)
        {
            var extraEntity = new Entity();
            world.Add(extraEntity, new Health(3));
            world.Add(extraEntity, new Mana(3));
            world.Add(extraEntity, new Armour(3));
        }
        
        var healthAndManaAndArmourQuery = new Query()
            .Include<Health>()
            .Include<Mana>()
            .Include<Armour>()
            .Sort(world);

        Console.WriteLine("Health and Mana and Armour query:");
        var stopwatch = new Stopwatch();
        for (var i = 0; i < 5; i++)
        {
            stopwatch.Restart();
            var foundCount = world.Query(healthAndManaAndArmourQuery).Count();
            stopwatch.Stop();
            Console.WriteLine(
                $"Ran the final query, found {foundCount} entities in: {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
    }
}