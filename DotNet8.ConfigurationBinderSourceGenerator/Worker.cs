using Microsoft.Extensions.Options;

namespace DotNet8.ConfigurationBinderSourceGenerator
{
    public class Worker : BackgroundService
    {
        private readonly IOptions<SectionAConfiguration> _configuration;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IOptions<SectionAConfiguration> configuration,
            ILogger<Worker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _logger.LogInformation($"{{{nameof(_configuration.Value.Property1)}}}", _configuration.Value.Property1);
                    _logger.LogInformation($"{{{nameof(_configuration.Value.Property2)}}}", _configuration.Value.Property2);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
