using System.Numerics;
using MoggleEngine.Input;
using Spectre.Console;

namespace MoggleEngine;

/// <summary>
/// Central game engine singleton that coordinates rendering, updating and level management.
/// Manages separate threads for update and render loops and exposes runtime metrics such as framerate and delta time.
/// </summary>
public class GameEngine
{
    /// <summary>
    /// Singleton instance of the game engine.
    /// </summary>
    public static GameEngine Instance = new();

    /// <summary>
    /// The currently loaded level, if any.
    /// </summary>
    public Level? Level;

    private Thread renderThread;

    private bool renderThreadCancel;


    private Thread updateThread;
    private bool updateThreadCancel;

    private GameEngine()
    {
        this.RenderEngine = RenderEngine.Instance;
        this.updateThread = new Thread(UpdateLoop);
        this.renderThread = new Thread(RenderLoop);
    }

    /// <summary>
    /// The render engine used by this game engine.
    /// </summary>
    public RenderEngine RenderEngine { get; }
    

    /// <summary>
    /// Target frames per second for rendering.
    /// </summary>
    public int TargetFramerate { get; private set; }

    /// <summary>
    /// Current measured framerate.
    /// </summary>
    public int Framerate { get; private set; }

    /// <summary>
    /// Total frames processed since engine start.
    /// </summary>
    public int FrameTotal { get; private set; }

    /// <summary>
    /// Whether the engine is currently running.
    /// </summary>
    public bool Running { get; set; } = true;

    /// <summary>
    /// Target update loop iterations per second.
    /// </summary>
    public int TargetUpdaterate { get; private set; }

    /// <summary>
    /// Delta time in seconds measured during the last update loop iteration.
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    /// Current measured update rate.
    /// </summary>
    public int UpdateRate { get; set; }

    /// <summary>
    /// Initialize the engine with viewport and timing parameters.
    /// </summary>
    public void Init(int height, int width, int pixelWidth, int targetFramerate, int targetUpdaterate)
    {
        this.TargetFramerate = targetFramerate;
        this.TargetUpdaterate = targetUpdaterate;
        this.RenderEngine.Init(height, width, pixelWidth, new Vector2(width, height));
    }


    /// <summary>
    /// Main render loop executed on the render thread. It renders the loaded level and updates the live console output.
    /// </summary>
    public void RenderLoop()
    {
        int sampleCount = this.TargetFramerate;
        double[] framerateSamples = new double[sampleCount];

        DateTime lastTime;
        float uncorrectedSleepDuration = 1000f / this.TargetFramerate;

        int frameCounter = 0;


        if (this.Level is null) return;
        AnsiConsole.Live(this.Level.UiRenderable).Start(ctx =>
        {
            while (!this.renderThreadCancel)
            {
                lastTime = DateTime.UtcNow;

                frameCounter++;
                frameCounter = frameCounter % sampleCount;
                
                this.Level.Render();
                ctx.UpdateTarget(this.Level.UiRenderable);
                ctx.Refresh();


                float computingDuration = (float)(DateTime.UtcNow - lastTime).TotalMilliseconds;
                int sleepDuration = (int)(uncorrectedSleepDuration - computingDuration);
                if (sleepDuration > 0) Thread.Sleep(sleepDuration);

                //increases total frames
                this.FrameTotal++;

                TimeSpan diff = DateTime.UtcNow - lastTime;
                this.Framerate = (int)(1000 / diff.TotalMilliseconds);

                framerateSamples[frameCounter] = diff.TotalSeconds;
            }
        });
        this.renderThreadCancel = false;
    }


    /// <summary>
    /// Main update loop executed on the update thread. It updates inputs and the loaded level.
    /// </summary>
    public void UpdateLoop()
    {
        int sampleCount = this.TargetUpdaterate;
        double[] updaterateSamples = new double[sampleCount];

        DateTime lastTime;
        float uncorrectedSleepDuration = 1000f / this.TargetUpdaterate;

        int updateCounter = 0;
        if (this.Level is null) return;

        while (!this.updateThreadCancel)
        {
            lastTime = DateTime.UtcNow;

            updateCounter++;
            updateCounter = updateCounter % sampleCount;

            InputHandler.Instance.Update();
            this.Level.Update();

            float computingDuration = (float)(DateTime.UtcNow - lastTime).TotalMilliseconds;
            int sleepDuration = (int)(uncorrectedSleepDuration - computingDuration);
            if (sleepDuration > 0) Thread.Sleep(sleepDuration);

            //increases total frames
            this.FrameTotal++;

            TimeSpan diff = DateTime.UtcNow - lastTime;
            this.UpdateRate = (int)(1000 / diff.TotalMilliseconds);
            this.DeltaTime = (float)diff.TotalSeconds;


            updaterateSamples[updateCounter] = diff.TotalSeconds;
        }

        this.updateThreadCancel = false;
        ConsoleInputHandler.Reset();
    }

    private void CancelRenderThread()
    {
        if (this.renderThread.IsAlive)
        {
            this.renderThreadCancel = true;
            if (!Thread.CurrentThread.Equals(this.renderThread))
                this.renderThread.Join();
        }
    }

    private void CancelUpdateThread()
    {
        if (this.updateThread.IsAlive)
        {
            this.updateThreadCancel = true;
            if (!Thread.CurrentThread.Equals(this.updateThread))
                this.updateThread.Join();
        }
    }

    private void StartRenderThread()
    {
        this.renderThread = new Thread(RenderLoop);
        this.renderThread.Start();
    }

    private void StartUpdateThread()
    {
        this.updateThread = new Thread(UpdateLoop);
        this.updateThread.Start();
    }

    /// <summary>
    /// Load a level asynchronously and start the engine loops for it.
    /// </summary>
    public void LoadLevel(Level level)
    {
        Thread loadThread = new(() => LoadLevelThread(level));
        loadThread.Start();
    }

    private void LoadLevelThread(Level level)
    {
        CancelRenderThread();
        CancelUpdateThread();

        this.Level?.Unload();
        level.Load();
        this.Level = level;

        StartRenderThread();
        StartUpdateThread();
    }
}