namespace MergyClient;

internal static class ConsoleExtensions
{
    internal static void WriteLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}