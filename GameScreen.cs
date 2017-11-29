using Apprentice.World;
using WinMan;

namespace Apprentice
{
    class GameScreen : Screen
    {
        public CameraPanel MainCameraPanel;

        public GameScreen()
        {
            MainCameraPanel = new MainCameraPanel(SizeC(0), SizeC(0), WidthMinus(0), HeightMinus(0));

            addPanel(MainCameraPanel);
        }
    }
}
