using System.Numerics;
using Spectre.Console;

namespace MoggleMunch.Engine;

public class GameEngine
{
    private readonly RenderEngine renderEngine;
    private readonly Thread renderThread;
    private readonly Thread updateThread;
    private readonly List<IRenderable> renderables = new();

    private readonly List<IUpdatable> updatables = new();

    public GameEngine(int height, int width, int pixelWidth, int targetFramerate, int targetUpdaterate)
    {
        this.TargetFramerate = targetFramerate;
        this.TargetUpdaterate = targetUpdaterate;
        this.renderEngine = new RenderEngine(height, width, pixelWidth, new Vector2(height, width));
        this.updateThread = new Thread(UpdateLoop);
        this.renderThread = new Thread(RenderLoop);
        this.renderThread.Start();
        this.updateThread.Start();
    }

    public int TargetFramerate { get; }

    public int Framerate { get; private set; }

    public int FrameTotal { get; private set; }

    public bool Running { get; set; } = true;

    public int TargetUpdaterate { get; }

    public float DeltaTime { get; private set; }

    public int UpdateRate { get; set; }

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


        AnsiConsole.Live(this.renderEngine.Canvas).Start(ctx =>
        {
            while (this.Running)
            {
                lastTime = DateTime.UtcNow;

                frameCounter++;
                frameCounter = frameCounter % sampleCount;

                this.renderEngine.PreRender();
                Render();
                this.renderEngine.PostRender(ctx);

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
        foreach (IRenderable renderable in this.renderables) renderable.Render(this.renderEngine);
    }

    public void Update()
    {
        foreach (IUpdatable updatable in this.updatables) updatable.Update(this);
    }
}