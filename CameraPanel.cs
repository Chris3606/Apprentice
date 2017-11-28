using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using WinMan;

namespace Apprentice
{
    // Designed to display a portion of the currently active map to its console.  Right now it just displays the player bc lazy and no map.
    class CameraPanel : Panel
    {
        public CameraPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, true)
        {

        }
        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            console.Set(Width / 2, Height / 2, RLColor.White, RLColor.Black, '@');
        }
    }
}
