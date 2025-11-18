using System.Runtime.CompilerServices;
using Spectre.Console;

namespace MoggleEngine.Input;

public static class ConsoleInputHandler
{
    public static event EventHandler<ConsoleKey>? KeyDownEvent;

    public static event EventHandler<ConsoleKey>? KeyUpEvent;

    private static Dictionary<ConsoleKey, DateTime> lastKeyPresses = new Dictionary<ConsoleKey, DateTime>();

    private static readonly TimeSpan KeyUpDelay = TimeSpan.FromMilliseconds(400);
    
    internal static void Update()
    {
            while (AnsiConsole.Console.Input.IsKeyAvailable())
            {
                ConsoleKeyInfo? keyInfo = AnsiConsole.Console.Input.ReadKey(true);
                if (keyInfo.HasValue)
                {
                    ConsoleKey key = keyInfo.Value.Key;
                    if (lastKeyPresses.ContainsKey(key) == false)
                    {
                        OnKeyDown(key);
                    }
                    lastKeyPresses[keyInfo.Value.Key] = DateTime.Now;
                }
            }

            Dictionary<ConsoleKey, DateTime> expiredKeyPresses =  lastKeyPresses.Where(keyPress => (DateTime.Now - keyPress.Value) > KeyUpDelay).ToDictionary();
            foreach (var expiredKeyPress in expiredKeyPresses)
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