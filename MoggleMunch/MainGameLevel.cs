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
    public Vector2 WorldSize = new(300, 300);
    
    private const int FoodCount = 600;

    private Player player;

    public MainGameLevel()
    {
        this.GameGui = new GameGui();
        this.StatusBar = new StatusBar();

        Player player = new(this);
        TestDrawings testDrawings = new();
        
        this.player = player;

        AddGameObject(player);
        AddGameObject(testDrawings);
        for (int i = 0; i < FoodCount; i++)
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
            this.LastScore = CalculateScore();
            OnGameEnded();
        }
    }
    
private int CalculateScore()
{
    double elapsed = (DateTime.Now - this.startTime).TotalSeconds;
    const double maxScore = 100.0;
    const double tau = 30.0; // characteristic time in seconds before score starts dropping noticeably
    const double k = 2.0;    // exponent > 1 => slower decrease at the beginning
    double score = maxScore / (1.0 + Math.Pow(elapsed / tau, k));
    return (int)Math.Max(0, score);
}

    private void OnGameEnded()
    {
        GameEnded.Invoke(this, this.LastScore);
        ScoreBoard.Instance.SetScore(this.LastScore, DateTime.Now);
    }

    public override void Load()
    {
        base.Load();
        this.StatusBar.GuiItems["Player:"] = ScoreBoard.Instance.PlayerName;
    }

    public override void Update()
    {
        base.Update();
        this.StatusBar.Progress = this.player.FoodLevel/(Player.NeeededFood / 100f);
        this.StatusBar.Score = CalculateScore();
        this.GameGui.UpdateGui(this.StatusBar.Renderable);
        this.GameGui.SetViewportHeight(GameEngine.Instance.RenderEngine.Height);
        this.GameGui.SetGuiHeight(this.StatusBar.GuiItems.Count+2);
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