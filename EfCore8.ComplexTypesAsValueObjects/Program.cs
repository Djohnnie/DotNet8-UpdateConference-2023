using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


Console.WriteLine("Hello, World!");



var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlite("Data Source=efdb.db"));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var dbContext = serviceProvider.GetRequiredService<CustomerDbContext>();
await dbContext.Database.EnsureCreatedAsync();
await dbContext.Customers.ExecuteDeleteAsync();

var address = new Address
{
    Street = "Main Street",
    Number = 123,
    City = "Gotham City",
    ZipCode = "GC 1000",
    Country = "Unknown"
};

dbContext.Customers.Add(new Customer
{
    FirstName = "John",
    LastName = "Doe",
    InvoiceAddress = address,
    DeliveryAddress = address
});
await dbContext.SaveChangesAsync();

address.Country = "Somewhere";

await dbContext.SaveChangesAsync();


foreach (var customer in dbContext.Customers)
{
    Console.WriteLine($"{customer.FirstName} {customer.LastName}, {customer.InvoiceAddress}");
}


Console.ReadKey();


public class Customer
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public Address InvoiceAddress { get; set; }
    public Address DeliveryAddress { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public int Number { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public override string ToString()
    {
        return $"{Street} {Number}, {ZipCode} {City}, {Country}";
    }
}


public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
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
        modelBuilder.Entity<Customer>()
            .ComplexProperty(x => x.InvoiceAddress);
        modelBuilder.Entity<Customer>()
            .ComplexProperty(x => x.DeliveryAddress);

        base.OnModelCreating(modelBuilder);
    }
}