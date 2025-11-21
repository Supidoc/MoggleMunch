using System.Numerics;
using System.Security.Cryptography;
using MoggleEngine;

namespace MoggleMunch;

/// <summary>
/// Consumable object that can be collected by the player. Chooses a random console color and draws a single pixel.
/// </summary>
public class Food : GameObject
{
    private readonly int randomConosoleColor;

    public Food()
    {
        this.Visible = true;
        this.ZIndex = 0;
        this.randomConosoleColor = RandomNumberGenerator.GetInt32(0, 256);
    }

    public override void Render()
    {
        GameEngine.Instance.RenderEngine.DrawPixel(this.Position, this.randomConosoleColor);
    }

    public override void Update()
    {
    }

    /// <summary>
    /// Set the world position of the food item.
    /// </summary>
    public void SetPosition(float x, float y)
    {
        this.Position = new Vector2(x, y);
    }
}