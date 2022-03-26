namespace MartinSGill.Logger.ConsoleAlternate;

using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("ColorConsole")]
public sealed class ConsoleAlternateLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleAlternateLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly IDisposable _onChangeToken;
    private ConsoleAlternateConfiguration _currentConfig;

    public ConsoleAlternateLoggerProvider(IOptionsMonitor<ConsoleAlternateConfiguration> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new ConsoleAlternateLogger(name, GetCurrentConfig));
    }

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken.Dispose();
    }

    private ConsoleAlternateConfiguration GetCurrentConfig()
    {
        return _currentConfig;
    }
}
