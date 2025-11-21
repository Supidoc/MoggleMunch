using Spectre.Console.Rendering;

namespace MoggleEngine.Interfaces;

/// <summary>
/// Lightweight interface representing a GUI component that exposes a Spectre.Console renderable.
/// </summary>
public interface IGui
{
    /// <summary>
    /// The Spectre.Console renderable used to represent the GUI.
    /// </summary>
    public Spectre.Console.Rendering.IRenderable Renderable { get; }
}