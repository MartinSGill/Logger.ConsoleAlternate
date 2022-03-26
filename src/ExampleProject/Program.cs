// See https://aka.ms/new-console-template for more information

#pragma warning disable CA1848

using System.Reflection;
using ExampleProject;
using MartinSGill.Logger.ConsoleAlternate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Load Configuration
var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                     .AddJsonFile("appsettings.json", true, true)
                                                     .AddEnvironmentVariables();

// Build Configuration
var configuration = configurationBuilder.Build();

// Add Logging
var serviceCollection = new ServiceCollection()
    .AddLogging(
        c =>
            c
                .AddConfiguration(
                    configuration
                        .GetRequiredSection("Logging"))
                .AddConsoleAlternate());

// Add DI Configuration
serviceCollection.Configure<ConsoleAlternateConfiguration>(configuration.GetSection("Logging:SpectrumLogger"));
serviceCollection.AddSingleton<ServiceLoggingExample>();

var services = serviceCollection.BuildServiceProvider();

// Get A logger
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("TestLogger");

// Log some Messages
logger.LogTrace("Log Trace");
logger.LogDebug("Log Debug");
logger.LogInformation("Log Information");
logger.LogWarning("Log Warning");
logger.LogError("Log Error");
logger.LogCritical("Log Critical");

// Create a Scope
using var scope = logger.BeginScope("{Scope}", "I am Scope");

// Create a Scoped log message, with a parameter
logger.LogInformation(
    "Log {AssemblyName}",
    Assembly.GetExecutingAssembly()
            .FullName);

// Show an Exception
logger.LogError(new NotSupportedException("Some Random Thing we don't support"), "Example Error with exception");

// Example service with logging
// Mostly shows off how names are shortened, and exception stack traces
var serviceExample = services.GetRequiredService<ServiceLoggingExample>();

serviceExample.Go();

namespace ExampleProject
{
    using System.Diagnostics.CodeAnalysis;

    internal class ServiceLoggingExample
    {
        private readonly ILogger<ServiceLoggingExample> _logger;

        public ServiceLoggingExample(ILogger<ServiceLoggingExample> logger)
        {
            _logger = logger;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public void Go()
        {
            _logger.LogTrace("Log Trace");
            _logger.LogDebug("Log Debug");
            _logger.LogInformation("Log Information");
            _logger.LogWarning("Log Warning");
            _logger.LogError("Log Error");
            _logger.LogCritical("Log Critical");

            try
            {
                StackLevelOne(12, "Hello");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Somewhere deep in the code...");
            }
        }

        private void StackLevelOne(int someInt, string someText)
        {
            StackLevelTwo(someText);
        }

        private void StackLevelThree()
        {
            throw new NotImplementedException();
        }

        private void StackLevelTwo(string someText)
        {
            StackLevelThree();
        }
    }
}
