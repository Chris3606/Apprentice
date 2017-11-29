using Apprentice.GameObjects;
using Apprentice.Maps;
using GoRogue;
using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public DemiPlane PlayerDemiPlane { get; private set; }
        static public NeverNever NeverNever { get; private set; }
        static public Map ActiveMap { get; private set; }
        static public GameScreen GameScreen;
        static public Player Player { get; private set; }

        static private KeyHandler globalKeyHandler; // handles global keys like fullscreen and exit

        static public void ChangeActiveMap(Map newMap) => ChangeActiveMap(newMap, Player.Position);
        // Changes active map (one player is in, eg. one main camera panel renders), moving the player to the new position specified in the new map.
        // The player is never moved while they are in the old map, and never in a position other than newPlayerPosition while they are added to the new one,
        // for the duration of this function.
        static public void ChangeActiveMap(Map newMap, Coord newPlayerPosition)
        {
            ActiveMap = newMap;

            Player.CurrentMap?.Remove(Player);
            Player.Position = newPlayerPosition;
            newMap.Add(Player);

            if (GameScreen != null) // Should check against new map.
                GameScreen.MainCameraPanel.MapToRender = newMap;
        }
        static void Main()
        {
            var settings = new RLSettings
            {
                BitmapFile = "terminal8x8.png",
                CharWidth = 8,
                CharHeight = 8,
                Width = 80,
                Height = 45,
                ResizeType = RLResizeType.ResizeCells,    // Display more tiles on screen if the console gets bigger, dont just increase size of existing
                Scale = 1f,
                StartWindowState = RLWindowState.Normal,
                WindowBorder = RLWindowBorder.Resizable,
                Title = "Apprentice"
            };

            Engine.Init(settings);

            // Instantiate map -- this may go elsewhere later.
            PlayerDemiPlane = new DemiPlane(30, 30);
            // Prolly be done in thread somewhere later to accellerate game start.
            NeverNever = new NeverNever(80, 80);


            // Global key commands setup
            globalKeyHandler = new GlobalKeyHandler();
            globalKeyHandler.StartHandling();

            
            Player = new Player(Coord.Get(0, 0));
            // // For now we just spawn player in middle of DemiPlane
            ChangeActiveMap(PlayerDemiPlane, Coord.Get(PlayerDemiPlane.Width / 2, PlayerDemiPlane.Height / 2));

            // Main game screen setup
            GameScreen = new GameScreen(ActiveMap);
            GameScreen.Show();

            

            Engine.Run();
        }
    }
}
