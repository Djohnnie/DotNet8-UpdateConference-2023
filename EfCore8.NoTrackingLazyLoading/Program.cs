using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite("Data Source=efdb.db"));
var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope1 = serviceProvider.CreateAsyncScope())
{
    using var dbContext = scope1.ServiceProvider.GetRequiredService<ToDoDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.ToDos.ExecuteDeleteAsync();
    await dbContext.Victims.ExecuteDeleteAsync();
    dbContext.ToDos.Add(new ToDo
    {
        Description = "Item 1",
        DueDateTime = DateTime.Now,
        Finished = false,
        Victims = new List<Victim>
        {
            new Victim
            {
                Name = "John Jamesson"
            }
        }
    });
    await dbContext.SaveChangesAsync();
}

using (var scope2 = serviceProvider.CreateAsyncScope())
{
    using var dbContext = scope2.ServiceProvider.GetRequiredService<ToDoDbContext>();

    var result = await dbContext.ToDos.AsNoTracking().ToListAsync();
    Debug.WriteLine($"TODO: {result.Count} results");
    await Task.Delay(5000);
    Debug.WriteLine($"VICTIM: {result.First().Victims.Count} results");
}

Console.ReadKey();

public class ToDo
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DueDateTime { get; set; }
    public bool Finished { get; set; }
    public virtual List<Victim> Victims { get; set; }
}

public class Victim
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ToDo ToDo { get; set; }
}


public class ToDoDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<Victim> Victims { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(b => b.IgnoreNonVirtualNavigations());
        optionsBuilder.LogTo(message => Debug.WriteLine(message));

        base.OnConfiguring(optionsBuilder);
    }
}