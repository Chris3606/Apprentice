using Apprentice.GameObjects;
using Apprentice.World;
using GoRogue;
using GoRogue.Random;
using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public GameWorld World { get; private set; }

        static public GameScreen GameScreen;
        static public SpellsPanel SpellsPanel;
        static public MessageRecallPanel MessageRecallPanel;
        static public Player Player { get; private set; }
        static public Map ActiveMap
        {
            get
            {
                if (Player == null)
                    return null;

                return Player.CurrentMap;
            }
        }

        static private KeyHandler globalKeyHandler; // handles global keys like fullscreen and exit

        static void Main()
        {
            var settings = new RLSettings
            {
                BitmapFile = "font14x14.png",
                CharWidth = 14,
                CharHeight = 14,
                Width = 80,
                Height = 45,
                ResizeType = RLResizeType.ResizeCells,    // Display more tiles on screen if the console gets bigger, dont just increase size of existing
                Scale = 1f,
                StartWindowState = RLWindowState.Normal,
                WindowBorder = RLWindowBorder.Resizable,
                Title = "Apprentice"
            };

            Engine.Init(settings);

            // Generate new world
            World = new GameWorld(30, 30, 50, 50);

            // For now we just spawn player at random position in the demi-plane
            Coord playerSpawn = Map.RandomOpenPosition(World.DemiPlane, SingletonRandom.DefaultRNG);
            Player = new Player(playerSpawn);
            World.DemiPlane.Add(Player); // Changes active map too

            // Global key commands setup
            globalKeyHandler = new GlobalKeyHandler();
            globalKeyHandler.StartHandling();

            // UI setup
            GameScreen = new GameScreen();
            SpellsPanel = new SpellsPanel(Screen.SizeC(0), Screen.SizeC(0), Screen.WidthMinus(0), Screen.HeightMinus(0));
            MessageRecallPanel = new MessageRecallPanel(Screen.SizeC(0), Screen.SizeC(0), Screen.WidthMinus(0), Screen.HeightMinus(0));

            // Show UI and get game underway
            GameScreen.Show();
            Engine.Run();
        }
    }
}
