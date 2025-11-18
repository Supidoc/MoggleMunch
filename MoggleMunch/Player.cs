using System.Numerics;
using MoggleEngine;
using MoggleMunch.Inputs;
using Spectre.Console;

namespace MoggleMunch;

public class Player : GameObject
{
    private Vector2 inputForce = Vector2.Zero;
    
    private float inputForceValue = 50f;

    public Player()
    {
        this.Position = new Vector2(5f,5f);
        LeftInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce(new Vector2(-1f, 0f));
        RightInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce (new Vector2(1f, 0f));
        UpInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce( new Vector2(0f, 1f));
        DownInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce( new Vector2(0f, -1f));

        LeftInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce( new Vector2(1f, 0f));
        RightInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce( new Vector2(-1f, 0f));
        UpInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce( new Vector2(0f, -1f));
        DownInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce( new Vector2(0f, 1f));

        this.Visible = true;
        this.FrictionCoefficient = 4f;
        this.Mass = 0.5f;
        this.ZIndex = 99;
    }


    public override void Render()
    {
        GameEngine.Instance.RenderEngine.DrawCircle(this.Position, 4, Color.Orange1, 3);
    }


    public void UpdateInputForce(Vector2 newInputForce)
    {
        Vector2 normalizedInputForce = this.inputForce.Length() == 0f ? Vector2.Zero : this.inputForce / this.inputForce.Length();
        this.Force -= normalizedInputForce*this.inputForceValue;
        float newInputForceLength = (this.inputForce + newInputForce).Length();
        Vector2 normalizedNewInputForce = newInputForceLength == 0f ? Vector2.Zero : (this.inputForce + newInputForce) / newInputForceLength;
        this.Force += normalizedNewInputForce*inputForceValue;
        this.inputForce += newInputForce;
    }
    
    public override void Update()
    {
        this.UpdatePhysics();
        Gui.Instance.GuiItems["X:"] = this.Position.X.ToString("0.00");
        Gui.Instance.GuiItems["Y:"] = this.Position.Y.ToString("0.00");

        GameEngine.Instance.RenderEngine.CameraPos = this.Position - 0.5f * new Vector2(GameEngine.Instance.RenderEngine.Width,GameEngine.Instance.RenderEngine.Height);
    }
}