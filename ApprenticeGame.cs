using Apprentice.GameObjects;
using Apprentice.Maps;
using GoRogue;
using GoRogue.Random;
using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public DemiPlane DemiPlane { get; private set; }
        static public NeverNever NeverNever { get; private set; }
        static public Map ActiveMap
        {
            get
            {
                if (Player == null)
                    return null;

                return Player.CurrentMap;
            }
        }
        static public GameScreen GameScreen;
        static public Player Player { get; private set; }

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

            GenerateMaps();

            // For now we just spawn player at random position in the demi-plane
            Coord playerSpawn = Map.RandomOpenPosition(DemiPlane, SingletonRandom.DefaultRNG);
            Player = new Player(playerSpawn);
            DemiPlane.Add(Player); // Changes active map too

            // Global key commands setup
            globalKeyHandler = new GlobalKeyHandler();
            globalKeyHandler.StartHandling();

            // Main game screen setup
            GameScreen = new GameScreen();
            GameScreen.Show();

            

            Engine.Run();
        }

        // For now we have a function to do this.  Probably best later if we have like a WorldManager that can do this, load maps, etc.
        public static void GenerateMaps()
        {
            // Create demi plane
            DemiPlane = new DemiPlane(30, 30);
            DemiPlane.Generate();

            // Create never-never
            NeverNever = new NeverNever(80, 80);
            NeverNever.Generate();

            // Create gate between two.
            Coord gateSourcePosition = Map.RandomOpenPosition(DemiPlane, SingletonRandom.DefaultRNG);
            Coord gateDestinationPosition = Map.RandomOpenPosition(NeverNever, SingletonRandom.DefaultRNG);

            DemiPlane.Remove(DemiPlane.Terrain[gateSourcePosition]);
            DemiPlane.Add(new Gate(gateSourcePosition, NeverNever, gateDestinationPosition));

        }
    }
}
