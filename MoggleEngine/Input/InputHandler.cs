using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

/// <summary>
/// Central input handler that keeps track of registered inputs and drives their update calls each frame.
/// Also delegates to the console input helper to capture key presses from Spectre.Console.
/// </summary>
public class InputHandler
{
    private readonly List<IUpdatableInput> inputs = new();
    
    private readonly object inputsLock = new();


    private InputHandler()
    {
    }

    /// <summary>
    /// Singleton instance of the input handler.
    /// </summary>
    public static InputHandler Instance { get; } = new();

    /// <summary>
    /// Calls Update on all registered inputs and processes console key events.
    /// </summary>
    public void Update()
    {
        List<IUpdatableInput> snapshot;
        lock (this.inputsLock)
        {
            // Make a stable copy while holding the lock
            snapshot = new List<IUpdatableInput>(this.inputs);
        }

        // Iterate the snapshot outside the lock to avoid blocking registrations
        foreach (IUpdatableInput input in snapshot) input.Update();

        ConsoleInputHandler.Update();
    }

    /// <summary>
    /// Registers a <see cref="DigitalInput"/> so it will be polled each update cycle.
    /// </summary>
    public void RegisterInput(DigitalInput input)
    {
        this.inputs.Add(input);
    }
}