using WinMan;

namespace Apprentice
{
    class GameScreen : Screen
    {
        public CameraPanel MainCameraPanel;

        public GameScreen(Map mapToRender)
        {
            MainCameraPanel = new CameraPanel(SizeC(0), SizeC(0), WidthMinus(0), HeightMinus(0), mapToRender);

            addPanel(MainCameraPanel);
        }
    }
}
