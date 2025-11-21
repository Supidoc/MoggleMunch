using System.Numerics;
using System.Security.Cryptography;
using MoggleEngine;

namespace MoggleMunch;

/// <summary>
/// Main gameplay level that manages game objects, world size and game lifecycle (start/end).
/// </summary>
public class MainGameLevel : GameLevel
{
    public bool GameRunning;

    public int LastScore;

    private DateTime startTime;
    public Vector2 WorldSize = new(100, 100);

    public MainGameLevel()
    {
        this.GameGui = new GameGui();
        this.GameGui.SetViewportHeight(25);
        this.StatusBar = new StatusBar();

        Player player = new(this);
        TestDrawings testDrawings = new();

        AddGameObject(player);
        AddGameObject(testDrawings);
        for (int i = 0; i < 100; i++)
            SpawnFood();
    }

    public sealed override GameGui GameGui { get; }

    public StatusBar StatusBar { get; }

    /// <summary>
    /// Event raised when the game ends. The integer payload is the final score.
    /// </summary>
    public event EventHandler<int> GameEnded;

    /// <summary>
    /// Starts the gameplay timer and sets the running flag.
    /// </summary>
    public void StartGame()
    {
        this.startTime = DateTime.Now;
        this.GameRunning = true;
    }

    /// <summary>
    /// Ends the game and computes the final score then raises <see cref="GameEnded"/>.
    /// </summary>
    public void EndGame()
    {
        if (this.GameRunning)
        {
            this.GameRunning = false;
            TimeSpan gameTime = DateTime.Now - this.startTime;
            this.LastScore = (int)(100 / gameTime.TotalSeconds);
            OnGameEnded();
        }
    }

    private void OnGameEnded()
    {
        GameEnded.Invoke(this, this.LastScore);
    }

    public override void Load()
    {
        base.Load();
    }

    public override void Update()
    {
        base.Update();
        this.GameGui.UpdateGui(this.StatusBar.Renderable);
    }

    public override void Render()
    {
        this.StatusBar.Render();
        base.Render();
    }

    public void SpawnFood()
    {
        Food food = new();
        int x = RandomNumberGenerator.GetInt32((int)(-this.WorldSize.X / 2), (int)(this.WorldSize.X / 2));
        int y = RandomNumberGenerator.GetInt32((int)(-this.WorldSize.Y / 2), (int)(this.WorldSize.Y / 2));
        food.SetPosition(x, y);
        AddGameObject(food);
    }
}