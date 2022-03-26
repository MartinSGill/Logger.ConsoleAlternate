namespace MartinSGill.Logger.ConsoleAlternate;

using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Spectre.Console;

public sealed class ConsoleAlternateLogger : ILogger
{
    private readonly Func<ConsoleAlternateConfiguration> getCurrentConfig;
    private readonly string name;
    private readonly OutputTheme outputTheme = new();

    /// <summary>
    /// Create new instance of <see cref="ConsoleAlternateLogger" />
    /// </summary>
    /// <param name="name"></param>
    /// <param name="getCurrentConfig"></param>
    public ConsoleAlternateLogger(string name, Func<ConsoleAlternateConfiguration> getCurrentConfig)
    {
        (this.name, this.getCurrentConfig) = (name, getCurrentConfig);
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
    {
        return default!;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        RenderTime();
        RenderSeparator();
        RenderLogLevel(logLevel);
        RenderSeparator();
        RenderName(name);
        RenderSeparator();

        Debug.Assert(formatter != null, nameof(formatter) + " != null");
        RenderMessage(formatter(state, exception));

        AnsiConsole.WriteLine();
        RenderException(exception);
    }

    private static void RenderException(Exception? exception)
    {
        if (exception != null)
        {
            AnsiConsole.WriteException(exception);
        }
    }

    private void RenderLogLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                AnsiConsole.Write(new Markup("trc", outputTheme.Trace));
                break;
            case LogLevel.Debug:
                AnsiConsole.Write(new Markup("dbg", outputTheme.Debug));
                break;
            case LogLevel.Information:
                AnsiConsole.Write(new Markup("inf", outputTheme.Info));
                break;
            case LogLevel.Warning:
                AnsiConsole.Write(new Markup("wrn", outputTheme.Warning));
                break;
            case LogLevel.Error:
                AnsiConsole.Write(new Markup("err", outputTheme.Error));
                break;
            case LogLevel.Critical:
                AnsiConsole.Write(new Markup("crt", outputTheme.Critical));
                break;
            case LogLevel.None:
                break;
        }
    }

    private void RenderMessage(string message)
    {
        AnsiConsole.Write(new Text(message, outputTheme.DefaultText));
    }

    private void RenderName(string name)
    {
        var elements = name.Split('.');
        if (elements.Length <= 1)
        {
            AnsiConsole.Write(new Text(name, outputTheme.Name));
            return;
        }

        var shortened = elements
                        .Select((e, i) => i != elements.Length - 1 ? e[..1] : e)
                        .ToArray();

        AnsiConsole.Write(new Text(string.Join(".", shortened[..^1]) + ".", outputTheme.AbbreviatedName));
        AnsiConsole.Write(new Text(shortened[^1..][0], outputTheme.Name));
    }

    private void RenderSeparator()
    {
        AnsiConsole.Write(new Text("|", outputTheme.DefaultText));
    }

    private void RenderTime()
    {
        AnsiConsole.Write(
            new Markup(
                DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture),
                outputTheme.Time));
    }
}
