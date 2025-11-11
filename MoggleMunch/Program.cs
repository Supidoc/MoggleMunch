using System.Numerics;
using System.Runtime.CompilerServices;
using MoggleMunch.Engine;
using Spectre.Console;

namespace MoggleMunch;

class Program
{
    private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
    
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--debug")
        {
            Log.Info("application started in debug mode");
            Console.WriteLine("Waiting for debugger ...");
            while(!System.Diagnostics.Debugger.IsAttached) {
                System.Threading.Thread.Sleep(500);
            }
        }

        GameEngine gameEngine = new GameEngine(25, 25, 3,30, 30);
        TestDrawings testDrawings = new TestDrawings();
        gameEngine.RegisterRenderable(testDrawings);
        gameEngine.RegisterUpdatable(testDrawings);
        
        for (;;)
        {        
            
            Thread.Sleep(100);


        }



    }
    


}