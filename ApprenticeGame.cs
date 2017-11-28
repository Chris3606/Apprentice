using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public Map ActiveMap;
        static public GameScreen GameScreen;

        static private KeyHandler globalKeyHandler; // handles global keys like fullscreen and exit

        static void Main()
        {
            var settings = new RLSettings
            {
                BitmapFile = "terminal8x8.png",
                CharWidth = 8,
                CharHeight = 8,
                Width = 80,
                Height = 60,
                ResizeType = RLResizeType.ResizeCells,    // Display more tiles on screen if the console gets bigger, dont just increase size of existing
                Scale = 1f,
                StartWindowState = RLWindowState.Normal,
                WindowBorder = RLWindowBorder.Resizable,
                Title = "Apprentice"
            };

            Engine.Init(settings);

            // Instantiate map -- this should go elsewhere later.
            ActiveMap = new DemiPlane(Engine.RootConsole.Width, Engine.RootConsole.Height);

            // Global key commands setup
            globalKeyHandler = new GlobalKeyHandler();
            globalKeyHandler.StartHandling();

            // Main game screen setup
            GameScreen = new GameScreen(ActiveMap);
            GameScreen.Show();

            Engine.Run();
        }
    }
}
