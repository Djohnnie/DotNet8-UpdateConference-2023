using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<BusinessDbContext>(options =>
    options.UseSqlite("Data Source=efdb.db"));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var dbContext = serviceProvider.GetRequiredService<BusinessDbContext>();
await dbContext.Database.EnsureCreatedAsync();
await dbContext.Businesses.ExecuteDeleteAsync();
dbContext.Businesses.Add(new Business
{
    Name = "Local Butcher Shop",
    StartOfYearlyHoliday = DateOnly.MinValue,
    EndOfYearlyHoliday = DateOnly.MinValue,
    Score = -1
});
dbContext.Businesses.Add(new Business
{
    Name = "Local Bakery",
    StartOfYearlyHoliday = DateOnly.MinValue,
    EndOfYearlyHoliday = DateOnly.MinValue,
    Score = 0
});
dbContext.Businesses.Add(new Business
{
    Name = "Local Wine Shop",
    StartOfYearlyHoliday = DateOnly.MinValue,
    EndOfYearlyHoliday = DateOnly.MinValue,
    Score = 10
});
await dbContext.SaveChangesAsync();


foreach(var business in dbContext.Businesses)
{
    Console.WriteLine($"{business.Name}");
}


Console.ReadKey();


public class Business
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartOfYearlyHoliday { get; set; }
    public DateOnly EndOfYearlyHoliday { get; set; }
    public int Score { get; set; }
}


public class BusinessDbContext : DbContext
{
    public DbSet<Business> Businesses { get; set; }

    public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
      : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Debug.WriteLine(message));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>()
            .Property(x => x.StartOfYearlyHoliday).HasDefaultValueSql("date('now')").HasSentinel(DateOnly.MinValue);
        modelBuilder.Entity<Business>()
            .Property(x => x.EndOfYearlyHoliday).HasDefaultValueSql("date('now')").HasSentinel(DateOnly.MinValue);
        modelBuilder.Entity<Business>()
            .Property(x => x.Score).HasDefaultValue(5).HasSentinel(-1);

        base.OnModelCreating(modelBuilder);
    }
}