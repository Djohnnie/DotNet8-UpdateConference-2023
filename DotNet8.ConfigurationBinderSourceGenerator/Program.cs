using DotNet8.ConfigurationBinderSourceGenerator;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configurationSection = builder.Configuration.GetSection("SectionA");
builder.Services.Configure<SectionAConfiguration>(configurationSection);

var host = builder.Build();
host.Run();