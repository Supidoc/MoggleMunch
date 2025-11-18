using System.Numerics;
using MoggleEngine.Input;
using MoggleEngine.Interfaces;
using Spectre.Console;

namespace MoggleEngine;

public class GameEngine
{
    public static GameEngine Instance = new();

    private readonly List<IRenderable> renderables = new();
    private readonly Thread renderThread;

    private readonly List<IUpdatable> updatables = new();
    private readonly Thread updateThread;

    private GameEngine()
    {
        this.RenderEngine = RenderEngine.Instance;
        this.updateThread = new Thread(UpdateLoop);
        this.renderThread = new Thread(RenderLoop);
    }

    public RenderEngine RenderEngine { get; private set; }

    public GuiEngine Gui { get; } = GuiEngine.Instance;

    public int TargetFramerate { get; private set; }

    public int Framerate { get; private set; }

    public int FrameTotal { get; private set; }

    public bool Running { get; set; } = true;

    public int TargetUpdaterate { get; private set; }

    public float DeltaTime { get; private set; }

    public int UpdateRate { get; set; }

    public void Init(int height, int width, int pixelWidth, int targetFramerate, int targetUpdaterate)
    {
        this.TargetFramerate = targetFramerate;
        this.TargetUpdaterate = targetUpdaterate;
        this.Gui.SetViewportHeight(height);
        this.RenderEngine.Init(height, width, pixelWidth, new Vector2(width, height));

        this.renderThread.Start();
        this.updateThread.Start();
    }

    public void RegisterUpdatable(IUpdatable updatable)
    {
        this.updatables.Add(updatable);
    }

    public void RegisterRenderable(IRenderable renderable)
    {
        this.renderables.Add(renderable);
    }

    public void RenderLoop()
    {
        int sampleCount = this.TargetFramerate;
        double[] framerateSamples = new double[sampleCount];

        DateTime lastTime;
        float uncorrectedSleepDuration = 1000f / this.TargetFramerate;

        int frameCounter = 0;


        AnsiConsole.Live(this.RenderEngine.Canvas).Start(ctx =>
        {
            while (this.Running)
            {
                lastTime = DateTime.UtcNow;

                frameCounter++;
                frameCounter = frameCounter % sampleCount;

                this.RenderEngine.PreRender();
                Render();
                this.RenderEngine.RenderCanvas();
                this.Gui.UpdateViewport(this.RenderEngine.Canvas);
                ctx.UpdateTarget(this.Gui.Layout);
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
    }


    public void UpdateLoop()
    {
        int sampleCount = this.TargetUpdaterate;
        double[] updaterateSamples = new double[sampleCount];

        DateTime lastTime;
        float uncorrectedSleepDuration = 1000f / this.TargetUpdaterate;

        int updateCounter = 0;


        while (this.Running)
        {
            lastTime = DateTime.UtcNow;

            updateCounter++;
            updateCounter = updateCounter % sampleCount;

            InputHandler.Instance.Update();
            Update();

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
    }


    public void Render()
    {
        foreach (IRenderable r in this.renderables.Where(r => r.Visible ).OrderBy(r => r.ZIndex)) r.Render();
    }

    public void Update()
    {
        foreach (IUpdatable updatable in this.updatables) updatable.Update();
    }
}