using GoRogue;
using RLNET;
using WinMan;

namespace Apprentice
{
    // Follows main map, syncs with player, and handles player/main map input.  Basically always handles the map the player is on.
    class MainCameraPanel : CameraPanel
    {
        public MainCameraPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height, Map mapToRender)
            : base(rootX, rootY, width, height, mapToRender)
        {
            AcceptsKeyboardInput = true;

            // Keep player in sync with player when they move.
            ApprenticeGame.Player.Moved += (s, e) => CameraPosition = e.NewPosition;
        }

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Cancel = true; // Default case will set this back

            Direction dirToMove = Direction.NONE;
            switch (e.KeyPress.Key)
            {
                case RLKey.W:
                case RLKey.Up:
                case RLKey.K:
                    dirToMove = Direction.UP;
                    break;
                case RLKey.S:
                case RLKey.Down:
                case RLKey.J:
                    dirToMove = Direction.DOWN;
                    break;
                case RLKey.A:
                case RLKey.Left:
                case RLKey.H:
                    dirToMove = Direction.LEFT;
                    break;
                case RLKey.D:
                case RLKey.Right:
                case RLKey.L:
                    dirToMove = Direction.RIGHT;
                    break;
                default:
                    e.Cancel = false;
                    break;
            }

            if (dirToMove != Direction.NONE)
            {
                // Later this will likely be moveOrAttack.
                ApprenticeGame.Player.MoveIn(dirToMove);
            }
        }
    }
}
