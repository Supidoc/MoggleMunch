# MoggleMunch
MoggleMunch is a console game inspired by the popular online game Agar.io. The game was made for the semester performace of the module APROG at university for applied science lucerne. The aim was to create a simple console game with a scoreboard and the possibility to be played on the APROG-Hat for the Raspberry Pi 4. 
## How to play
To play you simply need to build the project and the run it with dotnet. There are some arguments to configure the instance:
- `--playerName [playerName]` This argument is used to set the playername for which the scores will be recorded. The default value is Player1
- `--pixelWidth [pixelWidth]` This argument is used to set how many characters wide one pixel is. The reason is that different consoles use different fonts with different character widths. From experience this Value should be around 2 to 3 for a correct rendering (The player should be a circle and not an ellipse). The default value is 3.
- `--debug` This argument is used to make the game wait until a debugger is attached. This is used mainly for remote debugging on the Raspberry Pi.

The aim of the game is to eat as much as possible in as little time as possible. You can use the arrow keys or the joystick of the APROG-Hat (if available) to control you character. The progressbar on the top will show you how much you still need to eat.  
