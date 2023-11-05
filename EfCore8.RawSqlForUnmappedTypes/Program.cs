using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite("Data Source=efdb.db"));
var serviceProvider = serviceCollection.BuildServiceProvider();

using var dbContext = serviceProvider.GetRequiredService<ToDoDbContext>();
await dbContext.Database.EnsureCreatedAsync();
await dbContext.ToDoSet.ExecuteDeleteAsync();
dbContext.ToDoSet.Add(new ToDo
{
    Description = "Item 1",
    DueDateTime = DateTime.Now,
    Finished = false
});
dbContext.ToDoSet.Add(new ToDo
{
    Description = "Item 2",
    DueDateTime = DateTime.Now.AddDays(-1),
    Finished = true
});
dbContext.ToDoSet.Add(new ToDo
{
    Description = "Item 3",
    DueDateTime = DateTime.Now.AddDays(-2).AddHours(-3),
    Finished = false
});
await dbContext.SaveChangesAsync();


var before = DateTime.UtcNow.Date;

var result1 = await dbContext.Database.SqlQuery<CustomToDo>($"SELECT * FROM ToDoSet as t WHERE t.DueDateTime < {before}")
    .ToListAsync();
Console.WriteLine($"{result1.Count} results: {string.Join(", ", result1.Select(x => x.Description))}");

var result2 = await dbContext.Database.SqlQuery<CustomToDo>($"SELECT * FROM ToDoSet as t WHERE t.DueDateTime < {before}")
    .Where(x => !x.Finished).ToListAsync();
Console.WriteLine($"{result2.Count} results: {string.Join(", ", result2.Select(x => x.Description))}");





Console.ReadKey();

public class ToDo
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DueDateTime { get; set; }
    public bool Finished { get; set; }
}

public class CustomToDo
{
    public string Description { get; set; }
    public DateTime DueDateTime { get; set; }
    public bool Finished { get; set; }
}


public class ToDoDbContext : DbContext
{
    public DbSet<ToDo> ToDoSet { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Debug.WriteLine(message));

        base.OnConfiguring(optionsBuilder);
    }
}