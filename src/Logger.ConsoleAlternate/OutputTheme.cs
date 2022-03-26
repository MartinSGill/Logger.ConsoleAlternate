namespace MartinSGill.Logger.ConsoleAlternate;

using JetBrains.Annotations;
using Spectre.Console;

/// <summary>
/// Theme definition for controlling output style
/// </summary>
[PublicAPI]
public class OutputTheme
{
    public Style AbbreviatedName { get; set; } = new(Color.DarkGoldenrod);
    public Style Critical { get; set; } = new(Color.Red);
    public Style Debug { get; set; } = new(Color.Teal);
    public Style DefaultText { get; set; } = new(Color.Silver);
    public Style Error { get; set; } = new(Color.Red);
    public Style Info { get; set; } = new(Color.Grey100);
    public Style Name { get; set; } = new(Color.LightGoldenrod3);
    public Style Parameter { get; set; } = new(Color.Magenta1);
    public Style Time { get; set; } = new(Color.Grey63);
    public Style Trace { get; set; } = new(Color.Grey50);
    public Style Warning { get; set; } = new(Color.Orange1);
}
