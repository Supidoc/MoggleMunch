using System.Numerics;
using MoggleEngine.Interfaces;

namespace MoggleEngine;

public abstract class GameObject: IUpdatable, IRenderable
{
    public GameObject()
    {
        GameEngine.Instance.RegisterRenderable(this);
        GameEngine.Instance.RegisterUpdatable(this);
    }
    
    public Vector2 Position { get; protected set; }
    
    public Vector2 Velocity { get; protected set; }
    
    public Vector2 Acceleration { get; protected set; }
    public Vector2 Force { get; protected set; }
    
    public bool Visible { get; protected set; }
    public int ZIndex { get; set; } = 0;

    public float Mass { get; protected set; }
    
    public float FrictionCoefficient { get; protected set; }
    
    

    public abstract void Update();

    public abstract void Render();
    
    public virtual void UpdatePhysics()
    {
        if (Mass <= 0f)
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