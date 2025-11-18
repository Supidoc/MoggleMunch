using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

public class InputHandler
{
    private readonly List<IUpdatableInput> inputs = new();

    private InputHandler()
    {
    }

    public static InputHandler Instance { get; } = new();

    public void Update()
    {
        foreach (IUpdatableInput input in this.inputs) input.Update();
        ConsoleInputHandler.Update();
    }

    public void RegisterInput(DigitalInput input)
    {
        this.inputs.Add(input);
    }
}