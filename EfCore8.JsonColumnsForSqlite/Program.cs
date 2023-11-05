using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<BusinessDbContext>(options =>
    options.UseSqlite("Data Source=efdb.db"));

var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope1 = serviceProvider.CreateAsyncScope())
{
    using var dbContext = scope1.ServiceProvider.GetRequiredService<BusinessDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Businesses.ExecuteDeleteAsync();
    dbContext.Businesses.Add(new Business
    {
        Name = "Local Butcher Shop",
        StartOfYearlyHoliday = new DateOnly(2023, 11, 1),
        EndOfYearlyHoliday = new DateOnly(2023, 11, 11),
        OpeningHours = new List<OpeningHours>
        {
            new OpeningHours
            {
                DayOfWeek = DayOfWeek.Tuesday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(18, 0)
            },
            new OpeningHours
            {
                DayOfWeek = DayOfWeek.Wednesday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(18, 0)
            },
            new OpeningHours
            {
                DayOfWeek = DayOfWeek.Thursday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(18, 0)
            },
            new OpeningHours
            {
                DayOfWeek = DayOfWeek.Friday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(22, 0)
            },
            new OpeningHours
            {
                DayOfWeek = DayOfWeek.Saturday,
                OpensAt = new TimeOnly(14, 0),
                ClosesAt = new TimeOnly(21, 0)
            }
        }
    });
    await dbContext.SaveChangesAsync();
}

using (var scope2 = serviceProvider.CreateAsyncScope())
{
    using var dbContext = scope2.ServiceProvider.GetRequiredService<BusinessDbContext>();

    var result = await dbContext.Businesses.ToListAsync();

    foreach (var business in result)
    {
        Console.WriteLine($" - {business.Name}");

        foreach (var openingHours in business.OpeningHours)
        {
            Console.WriteLine($"    [{openingHours.DayOfWeek}]\t{openingHours.OpensAt} - {openingHours.ClosesAt}");
        }
    }
}

Console.ReadKey();


public class Business
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartOfYearlyHoliday { get; set; }
    public DateOnly EndOfYearlyHoliday { get; set; }
    public List<OpeningHours> OpeningHours { get; set; }
}

public class OpeningHours
{
    public DayOfWeek DayOfWeek { get; set; }
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
        modelBuilder.Entity<Business>().OwnsMany(x => x.OpeningHours, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToJson();
            ownedNavigationBuilder.Property(x => x.DayOfWeek).HasConversion(new EnumToStringConverter<DayOfWeek>());
        });

        base.OnModelCreating(modelBuilder);
    }
}