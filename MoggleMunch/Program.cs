using System.Diagnostics;
using System.Runtime.CompilerServices;
using GpioHat;
using NLog;
using MoggleEngine;
using Spectre.Console;

namespace MoggleMunch;

internal class Program
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--debug")
        {
            Log.Info("application started in debug mode");
            Console.WriteLine("Waiting for debugger ...");
            while (!Debugger.IsAttached) Thread.Sleep(500);
        }

        if (Raspberry.Instance.GpioSupport())
        {
            Raspberry.Instance.Init(HardwareAccess.Raspberry);
        }
        
        int w = Console.WindowWidth;
        int h = Console.WindowHeight;

        GameEngine.Instance.Init(25, 87, 3, 30, 30);
        Player player = new();
        TestDrawings testDrawings = new TestDrawings();
        for (;;) Thread.Sleep(100);
    }
}