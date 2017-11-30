using Apprentice.GameObjects;
using Apprentice.World;
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
            Direction dirToMove = Direction.NONE;
            switch (e.KeyPress.Key)
            {
                case RLKey.Keypad8:
                case RLKey.K:
                    dirToMove = Direction.UP;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad9:
                case RLKey.U:
                    dirToMove = Direction.UP_RIGHT;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad6:
                case RLKey.L:
                    dirToMove = Direction.RIGHT;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad3:
                case RLKey.N:
                    dirToMove = Direction.DOWN_RIGHT;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad2:
                case RLKey.J:
                    dirToMove = Direction.DOWN;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad1:
                case RLKey.B:
                    dirToMove = Direction.DOWN_LEFT;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad4:
                case RLKey.H:
                    dirToMove = Direction.LEFT;
                    e.Cancel = true;
                    break;
                case RLKey.Keypad7:
                case RLKey.Y:
                    dirToMove = Direction.UP_LEFT;
                    e.Cancel = true;
                    break;
                case RLKey.Period:
                    if (e.KeyPress.Shift) // ">" key; check for stairy-stuff.  Probably need to split this off, teleporter may go away since direction would be diff.
                    {
                        foreach (var gObject in MapToRender.ObjectsAt(ApprenticeGame.Player.Position))
                            if (gObject is Teleporter teleporter)
                            {
                                teleporter.Traverse(ApprenticeGame.Player);
                                e.Cancel = true;
                                break;
                            }
                    }
                    break;
                case RLKey.Comma: // Activate top layer of whatever is below us, if something can be activated
                    int topLayer = (int)Map.Layer.Terrain;
                    IActivatable activatable = null;
                    foreach (var gObj in MapToRender.ObjectsAt(ApprenticeGame.Player.Position))
                    {
                        if (gObj is IActivatable activate && (int)gObj.Layer >= topLayer)
                        {
                            topLayer = (int)gObj.Layer;
                            activatable = activate;
                        }
                    }

                    if (activatable != null)
                    {
                        activatable.Activate();
                        e.Cancel = true;
                    }
                    break;
            }

            if (dirToMove != Direction.NONE)
                if (!ApprenticeGame.Player.MoveIn(dirToMove))
                    ApprenticeGame.Player.Combat.AttackIn(dirToMove);
        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            if (MapToRender != null)
                MapToRender.CalculateFOVIfNeeded(ApprenticeGame.Player.Position, 10, Radius.CIRCLE);

            base.UpdateLayout(sender, e);
        }
    }
}
