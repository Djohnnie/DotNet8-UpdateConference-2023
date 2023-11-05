using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<BusinessDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLDEV;Database=efdb3;Integrated Security=True;Encrypt=True;TrustServerCertificate=True", config => config.UseHierarchyId()));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var dbContext = serviceProvider.GetRequiredService<BusinessDbContext>();
await dbContext.Database.EnsureCreatedAsync();
await dbContext.Halflings.ExecuteDeleteAsync();

await dbContext.Halflings.AddRangeAsync(
    new Halfling(HierarchyId.Parse("/"), "Balbo", 1167),
    new Halfling(HierarchyId.Parse("/1/"), "Mungo", 1207),
    new Halfling(HierarchyId.Parse("/2/"), "Pansy", 1212),
    new Halfling(HierarchyId.Parse("/3/"), "Ponto", 1216),
    new Halfling(HierarchyId.Parse("/4/"), "Largo", 1220),
    new Halfling(HierarchyId.Parse("/5/"), "Lily", 1222),
    new Halfling(HierarchyId.Parse("/1/1/"), "Bungo", 1246),
    new Halfling(HierarchyId.Parse("/1/2/"), "Belba", 1256),
    new Halfling(HierarchyId.Parse("/1/3/"), "Longo", 1260),
    new Halfling(HierarchyId.Parse("/1/4/"), "Linda", 1262),
    new Halfling(HierarchyId.Parse("/1/5/"), "Bingo", 1264),
    new Halfling(HierarchyId.Parse("/3/1/"), "Rosa", 1256),
    new Halfling(HierarchyId.Parse("/3/2/"), "Polo"),
    new Halfling(HierarchyId.Parse("/4/1/"), "Fosco", 1264),
    new Halfling(HierarchyId.Parse("/1/1/1/"), "Bilbo", 1290),
    new Halfling(HierarchyId.Parse("/1/3/1/"), "Otho", 1310),
    new Halfling(HierarchyId.Parse("/1/5/1/"), "Falco", 1303),
    new Halfling(HierarchyId.Parse("/3/2/1/"), "Posco", 1302),
    new Halfling(HierarchyId.Parse("/3/2/2/"), "Prisca", 1306),
    new Halfling(HierarchyId.Parse("/4/1/1/"), "Dora", 1302),
    new Halfling(HierarchyId.Parse("/4/1/2/"), "Drogo", 1308),
    new Halfling(HierarchyId.Parse("/4/1/3/"), "Dudo", 1311),
    new Halfling(HierarchyId.Parse("/1/3/1/1/"), "Lotho", 1310),
    new Halfling(HierarchyId.Parse("/1/5/1/1/"), "Poppy", 1344),
    new Halfling(HierarchyId.Parse("/3/2/1/1/"), "Ponto", 1346),
    new Halfling(HierarchyId.Parse("/3/2/1/2/"), "Porto", 1348),
    new Halfling(HierarchyId.Parse("/3/2/1/3/"), "Peony", 1350),
    new Halfling(HierarchyId.Parse("/4/1/2/1/"), "Frodo", 1368),
    new Halfling(HierarchyId.Parse("/4/1/3/1/"), "Daisy", 1350),
    new Halfling(HierarchyId.Parse("/3/2/1/1/1/"), "Angelica", 1381),
    new Halfling(HierarchyId.Parse("/3/2/1/1/2/"), "Tomtom", 1378));

await dbContext.SaveChangesAsync();

Console.WriteLine("Query a specific generation");
var generation = await dbContext.Halflings.Where(
    halfling => halfling.PathFromPatriarch.GetLevel() == 4).ToListAsync();

foreach( var halfling in generation)
{
    Console.WriteLine(halfling);
}

Console.WriteLine("Query all descendants");
var descendants = await dbContext.Halflings.Where(
        descendent => descendent.PathFromPatriarch.GetAncestor(2) == dbContext.Halflings
            .Single(ancestor => ancestor.Name == "Dudo").PathFromPatriarch).ToListAsync();

foreach (var halfling in descendants)
{
    Console.WriteLine(halfling);
}


Console.ReadKey();


public class Halfling
{
    public Halfling() { }

    public Halfling(HierarchyId pathFromPatriarch, string name, int? yearOfBirth = null)
    {
        PathFromPatriarch = pathFromPatriarch;
        Name = name;
        YearOfBirth = yearOfBirth;
    }

    public int Id { get; private set; }
    public HierarchyId PathFromPatriarch { get; set; }
    public string Name { get; set; }
    public int? YearOfBirth { get; set; }

    public override string ToString()
    {
        return $"{Name} {YearOfBirth} {PathFromPatriarch}";
    }
}


public class BusinessDbContext : DbContext
{
    public DbSet<Halfling> Halflings { get; set; }

    public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Debug.WriteLine(message));

        base.OnConfiguring(optionsBuilder);
    }
}