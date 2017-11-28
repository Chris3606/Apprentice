using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinMan;

namespace Apprentice
{
    class GameScreen : Screen
    {
        public CameraPanel CameraPanel;

        public GameScreen()
        {
            CameraPanel = new CameraPanel(SizeC(0), SizeC(0), WidthMinus(0), HeightMinus(0));

            addPanel(CameraPanel);
        }
    }
}
