namespace MartinSGill.Logger.ConsoleAlternate;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddConsoleAlternate(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, ConsoleAlternateLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <ConsoleAlternateConfiguration, ConsoleAlternateLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddConsoleAlternate(
        this ILoggingBuilder builder,
        Action<ConsoleAlternateConfiguration> configure)
    {
        builder.AddConsoleAlternate();
        builder.Services.Configure(configure);

        return builder;
    }
}
