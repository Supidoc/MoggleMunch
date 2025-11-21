using System.Numerics;
using MoggleEngine;
using MoggleMunch.Inputs;
using Spectre.Console;

namespace MoggleMunch;

/// <summary>
/// Player avatar controlled by directional inputs. Handles input->force translation, physics update and collision with food.
/// </summary>
public class Player : GameObject
{
    private readonly float inputForceValue = 50f;
    private readonly MainGameLevel level;
    private Vector2 inputForce = Vector2.Zero;

    private int radius = 1;

    public Player(MainGameLevel level)
    {
        this.level = level;
        this.Position = new Vector2(5f, 5f);

        LeftInput.Instance.KeyDownEvent += OnPlayerStarted;
        RightInput.Instance.KeyDownEvent += OnPlayerStarted;
        UpInput.Instance.KeyDownEvent += OnPlayerStarted;
        DownInput.Instance.KeyDownEvent += OnPlayerStarted;

        LeftInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce(new Vector2(-1f, 0f));
        RightInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce(new Vector2(1f, 0f));
        UpInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce(new Vector2(0f, 1f));
        DownInput.Instance.KeyDownEvent += (sender, args) => UpdateInputForce(new Vector2(0f, -1f));

        LeftInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce(new Vector2(1f, 0f));
        RightInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce(new Vector2(-1f, 0f));
        UpInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce(new Vector2(0f, -1f));
        DownInput.Instance.KeyUpEvent += (sender, args) => UpdateInputForce(new Vector2(0f, 1f));

        this.Visible = true;
        this.FrictionCoefficient = 4f;
        this.Mass = 0.5f;
        this.ZIndex = 99;
    }

    /// <summary>
    /// Current amount of food collected by the player.
    /// </summary>
    public int FoodLevel { get; set; }

    /// <summary>
    /// Render the player as a circle using the engine's RenderEngine.
    /// </summary>
    public override void Render()
    {
        GameEngine.Instance.RenderEngine.DrawCircle(this.Position, this.radius, Color.Orange1, this.radius - 1);
    }

    /// <summary>
    /// Translate directional inputs into forces and update physics; .
    /// </summary>
    public void UpdateInputForce(Vector2 newInputForce)
    {
        Vector2 normalizedInputForce =
            this.inputForce.Length() == 0f ? Vector2.Zero : this.inputForce / this.inputForce.Length();
        this.Force -= normalizedInputForce * this.inputForceValue;
        float newInputForceLength = (this.inputForce + newInputForce).Length();
        Vector2 normalizedNewInputForce = newInputForceLength == 0f
            ? Vector2.Zero
            : (this.inputForce + newInputForce) / newInputForceLength;
        this.Force += normalizedNewInputForce * this.inputForceValue;
        this.inputForce += newInputForce;
    }

    /// <summary>
    /// Updates physics; handles food pickup and game end conditions.
    /// </summary>
    public override void Update()
    {
        UpdatePhysics();
        this.level.StatusBar.GuiItems["X:"] = this.Position.X.ToString("0.00");
        this.level.StatusBar.GuiItems["Y:"] = this.Position.Y.ToString("0.00");
        this.level.StatusBar.GuiItems["Food:"] = this.FoodLevel.ToString();
        

        GameEngine.Instance.RenderEngine.CameraPos = this.Position -
                                                     0.5f * new Vector2(GameEngine.Instance.RenderEngine.Width,
                                                         GameEngine.Instance.RenderEngine.Height);

        foreach (GameObject food in ((GameLevel)GameEngine.Instance.Level!).GameObjects.Where(o => o is Food).ToList()!)
            if (Vector2.Distance(food.Position, this.Position) <= this.radius)
            {
                this.FoodLevel++;
                ((GameLevel)GameEngine.Instance.Level!).RemoveGameObject(food);
                if (GameEngine.Instance.Level is MainGameLevel) ((MainGameLevel)GameEngine.Instance.Level).SpawnFood();

                if (this.FoodLevel % 10 == 0) this.radius++;
            }

        if (this.FoodLevel > 39) this.level.EndGame();
    }

    private void OnPlayerStarted(object? sender, EventArgs eventArgs)
    {
        this.level.StartGame();
        LeftInput.Instance.KeyDownEvent -= OnPlayerStarted;
        RightInput.Instance.KeyDownEvent -= OnPlayerStarted;
        UpInput.Instance.KeyDownEvent -= OnPlayerStarted;
        DownInput.Instance.KeyDownEvent -= OnPlayerStarted;
    }
}