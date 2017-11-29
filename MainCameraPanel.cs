using Apprentice.GameObjects.Terrain;
using GoRogue;
using RLNET;
using WinMan;

namespace Apprentice
{
    // Follows main map, syncs with player, and handles player/main map input.  Basically always handles the map the player is on.
    class MainCameraPanel : CameraPanel
    {
        public MainCameraPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, ApprenticeGame.ActiveMap)
        {
            AcceptsKeyboardInput = true;

            // Sync player manually the first time since they won't have moved after creation of this panel to trigger below lambda.
            CameraPosition = ApprenticeGame.Player.Position;

            // Keep our rendered map in sync with player.
            ApprenticeGame.Player.RemovedFromMap += (s, e) => MapToRender = null;
            ApprenticeGame.Player.AddedToMap += (s, e) => MapToRender = e.Map;

            // Keep rendering position in sync with player when they move.
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
                case RLKey.Period:
                    if (e.KeyPress.Shift) // ">" key; check for gate to go.
                    {
                        // Huh.  This code does need to know that the gate it's looking for is terrain.  Not sure how I feel about that.
                        if (MapToRender.Terrain[ApprenticeGame.Player.Position] is Gate gate)
                            gate.Traverse(ApprenticeGame.Player); // TODO: Gate traverse is probably not cool here, really need to have gate determine who goes where?
                    }
                    else
                        e.Cancel = false;
                    break;
                default:
                    e.Cancel = false;
                    break;
            }

            if (dirToMove != Direction.NONE)
            {
                // Later this will likely be moveOrAttack.
                ApprenticeGame.Player.Combat.MoveOrAttackIn(dirToMove);
            }
        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            if (MapToRender != null)
                MapToRender.CalculateFOVIfNeeded(ApprenticeGame.Player.Position, 10, Radius.DIAMOND);

            base.UpdateLayout(sender, e);
        }
    }
}
