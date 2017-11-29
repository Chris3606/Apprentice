using Apprentice.GameObjects;
using GoRogue;
using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public Map ActiveMap;
        static public GameScreen GameScreen;
        static public Player Player { get; private set; }

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
            ActiveMap = new DemiPlane(30, 30);

            // Global key commands setup
            globalKeyHandler = new GlobalKeyHandler();
            globalKeyHandler.StartHandling();

            // For now we just spawn player in middle of map.
            Player = new Player(Coord.Get(ActiveMap.Width / 2, ActiveMap.Height / 2));
            ActiveMap.Add(Player);

            // Main game screen setup
            GameScreen = new GameScreen(ActiveMap);
            GameScreen.MainCameraPanel.CameraPosition = Player.Position; // Need to do this manually the first time, bc player hasn't moved after gamescreen has existed.
            GameScreen.Show();

            

            Engine.Run();
        }
    }
}
