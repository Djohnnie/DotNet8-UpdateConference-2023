using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
//serviceCollection.AddDbContext<BusinessDbContext>(options =>
//    options.UseSqlite("Data Source=efdb.db"));
serviceCollection.AddDbContext<BusinessDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLDEV;Database=efdb1;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var dbContext = serviceProvider.GetRequiredService<BusinessDbContext>();
await dbContext.Database.EnsureCreatedAsync();
await dbContext.Businesses.ExecuteDeleteAsync();
dbContext.Businesses.Add(new Business
{
    Name = "Local Butcher Shop",
    StartOfYearlyHoliday = new DateOnly(2023, 11, 1),
    EndOfYearlyHoliday = new DateOnly(2023, 11, 11),
    OpeningHours = new OpeningHours
    {
        OpensAt = new TimeOnly(9, 0),
        ClosesAt = new TimeOnly(18, 0),
    }
});
await dbContext.SaveChangesAsync();




Console.ReadKey();


public class Business
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartOfYearlyHoliday { get; set; }
    public DateOnly EndOfYearlyHoliday { get; set; }
    public OpeningHours OpeningHours { get; set; }
}

public class OpeningHours
{
    public TimeOnly OpensAt { get; set; }
    public TimeOnly ClosesAt { get; set; }
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
        modelBuilder.Entity<Business>().OwnsOne(x => x.OpeningHours);

        base.OnModelCreating(modelBuilder);
    }
}