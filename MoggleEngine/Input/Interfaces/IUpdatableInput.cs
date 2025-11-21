namespace MoggleEngine.Input.Interfaces;

/// <summary>
/// Internal interface used by the input subsystem for pollable input sources.
/// </summary>
internal interface IUpdatableInput
{
    /// <summary>
    /// Called each update cycle by the <see cref="InputHandler"/> implementation.
    /// </summary>
    internal void Update();
}