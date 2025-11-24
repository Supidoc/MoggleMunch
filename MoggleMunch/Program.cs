using System.Diagnostics;
using System.Runtime.CompilerServices;
using GpioHat;
using MoggleEngine;
using MoggleMunch.Inputs;
using NLog;

namespace MoggleMunch;

/// <summary>
/// Application entry point. Parses command-line arguments, initializes hardware if available and starts the initial menu level.
/// </summary>
internal class Program
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();


    private static void Main(string[] args)
    {
        if (args.Length > 0 && args.Any(arg => arg == "--debug"))
        {
            Log.Info("application started in debug mode");
            Console.WriteLine("Waiting for debugger ...");
            while (!Debugger.IsAttached) Thread.Sleep(500);
        }

        string username = "Player1";



        int usernameIndex = args.ToList().FindIndex(s => s == "--username");
        if (usernameIndex != -1) username = args[usernameIndex + 1];
        
        ScoreBoard.Instance.PlayerName = username;

        
        int pixelWidth = 1;


        int pixelWidthIndex = args.ToList().FindIndex(s => s == "--pixelWidth");
        if (pixelWidthIndex != -1)
        {
            bool parseSuccesful = int.TryParse(args[pixelWidthIndex + 1], out pixelWidth);
            if (!parseSuccesful)
            {
                Log.Error("Could not parse pixel width argument, using default of 1");
                pixelWidth = 1;
            }
        }



        if (Raspberry.Instance.GpioSupport()) Raspberry.Instance.Init(HardwareAccess.Raspberry);

        int w = Console.WindowWidth;
        int h = Console.WindowHeight;
        
        int canvasWidth = (w - 5) / pixelWidth;
        
        ScoreboardMenuLevel scoreboardMenuLevelLevel = new();
        

        GameEngine.Instance.Init(25, canvasWidth, pixelWidth, 30, 15);

        GameEngine.Instance.LoadLevel(scoreboardMenuLevelLevel);

        CenterInput.Instance.KeyDownEvent += (sender, eventArgs) =>
        {
            if (scoreboardMenuLevelLevel.Loaded)
            {
                MainGameLevel mainGameLevel = new();
                mainGameLevel.GameEnded += (sender, i) => GameEngine.Instance.LoadLevel(scoreboardMenuLevelLevel);

                GameEngine.Instance.LoadLevel(mainGameLevel);
            }
        };


        for (;;) Thread.Sleep(100);
    }
}