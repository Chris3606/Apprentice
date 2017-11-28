using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using WinMan;

namespace Apprentice
{
    class ApprenticeGame
    {
        static public GameScreen GameScreen;

        static void Main()
        {
            var settings = new RLSettings
            {
                BitmapFile = "terminal8x8.png",
                CharWidth = 8,
                CharHeight = 8,
                Width = 80,
                Height = 60,
                ResizeType = RLResizeType.ResizeCells, // Display more on screen if the console gets bigger, dont just increase size of existing
                Scale = 1f,
                StartWindowState = RLWindowState.Normal,
                WindowBorder = RLWindowBorder.Resizable,
                Title = "Apprentice"
            };

            Engine.Init(settings);

            GameScreen = new GameScreen();
            GameScreen.Show();

            Engine.Run();
        }
    }
}
