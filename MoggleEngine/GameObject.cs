using System.Numerics;
using MoggleEngine.Interfaces;

namespace MoggleEngine;

/// <summary>
/// Base type for any object that exists in a game level. Provides position, basic physics fields, visibility and rendering order.
/// Implementations must provide <see cref="Render"/> and <see cref="Update"/> logic.
/// </summary>
public abstract class GameObject : IUpdatable, IRenderable
{
    /// <summary>
    /// World-space position of the object.
    /// </summary>
    public Vector2 Position { get; protected set; }

    /// <summary>
    /// Current linear velocity (units per second).
    /// </summary>
    public Vector2 Velocity { get; protected set; }

    /// <summary>
    /// Current acceleration applied to the object.
    /// </summary>
    public Vector2 Acceleration { get; protected set; }
    public Vector2 Force { get; protected set; }

    /// <summary>
    /// Mass in arbitrary units; used for physics integration. Must be > 0 when calling <see cref="UpdatePhysics"/>.
    /// </summary>
    public float Mass { get; protected set; }

    /// <summary>
    /// Simple friction coefficient applied as a damping force in <see cref="UpdatePhysics"/>.
    /// </summary>
    public float FrictionCoefficient { get; protected set; }

    /// <summary>
    /// Whether the object should be rendered by the level.
    /// </summary>
    public bool Visible { get; protected set; }
    /// <summary>
    /// Rendering order: lower values are drawn first.
    /// </summary>
    public int ZIndex { get; set; } = 0;

    /// <summary>
    /// Render the object into the render engine. Implementations should call into the global <see cref="MoggleEngine.GameEngine"/>'s <c>RenderEngine</c>.
    /// </summary>
    public abstract void Render();


    /// <summary>
    /// Unique identifier for this game object.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
    public abstract void Update();

    /// <summary>
    /// Performs a simple physics integration step based on <see cref="Force"/>, <see cref="Mass"/> and friction.
    /// Throws an <see cref="InvalidDataException"/> if Mass is less than or equal to zero.
    /// </summary>
    public virtual void UpdatePhysics()
    {
        if (this.Mass <= 0f)
            throw new InvalidDataException("Mass must be greater than zero.");

        Vector2 friction = -this.Velocity * this.FrictionCoefficient;

        Vector2 totalForce = this.Force + friction;
        // Calculate acceleration
        this.Acceleration = totalForce / this.Mass;

        // Update velocity
        this.Velocity += this.Acceleration * GameEngine.Instance.DeltaTime;

        // Update position
        this.Position += this.Velocity * GameEngine.Instance.DeltaTime;
    }
}