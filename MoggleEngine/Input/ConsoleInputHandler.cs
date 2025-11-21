using Spectre.Console;

namespace MoggleEngine.Input;

/// <summary>
/// Helper that captures key presses from Spectre.Console's input and raises key down/up events. Adds a delay to capture repeated key presses as held keys.
/// Also exposes Reset to force all keys to a released state.
/// </summary>
public static class ConsoleInputHandler
{
    private static Dictionary<ConsoleKey, DateTime> lastKeyPresses = new();

    private static readonly TimeSpan KeyUpDelay = TimeSpan.FromMilliseconds(400);

    /// <summary>
    /// Raised when a key is detected as pressed.
    /// </summary>
    public static event EventHandler<ConsoleKey>? KeyDownEvent;

    /// <summary>
    /// Raised when a key is considered released after the configured KeyUpDelay.
    /// </summary>
    public static event EventHandler<ConsoleKey>? KeyUpEvent;

    /// <summary>
    /// Force all tracked keys to the up state and clear internal history.
    /// </summary>
    public static void Reset()
    {
        foreach (KeyValuePair<ConsoleKey, DateTime> lastKeyPress in lastKeyPresses) OnKeyUp(lastKeyPress.Key);
        lastKeyPresses = new Dictionary<ConsoleKey, DateTime>();
    }
    
    internal static void Update()
    {
        while (AnsiConsole.Console.Input.IsKeyAvailable())
        {
            ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(true);
            if (keyInfo.HasValue)
            {
                ConsoleKey key = keyInfo.Value.Key;
                if (!lastKeyPresses.ContainsKey(key)) OnKeyDown(key);
                lastKeyPresses[keyInfo.Value.Key] = DateTime.Now;
            }
        }

        Dictionary<ConsoleKey, DateTime> expiredKeyPresses =
            lastKeyPresses.Where(keyPress => DateTime.Now - keyPress.Value > KeyUpDelay).ToDictionary();
        foreach (KeyValuePair<ConsoleKey, DateTime> expiredKeyPress in expiredKeyPresses)
        {
            OnKeyUp(expiredKeyPress.Key);
            lastKeyPresses.Remove(expiredKeyPress.Key);
        }
    }

    private static void OnKeyUp(ConsoleKey key)
    {
        KeyUpEvent?.Invoke(null, key);
    }

    private static void OnKeyDown(ConsoleKey key)
    {
        KeyDownEvent?.Invoke(null, key);
    }
}