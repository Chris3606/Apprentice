using Apprentice.GameObjects;
using Apprentice.World;
using GoRogue;
using RLNET;
using System;
using System.Collections.Generic;
using WinMan;

namespace Apprentice
{
    // Main camera panel in game view -- follows main map, syncs with player, and handles player/main map input.  Also displays messages at the top
    class MainCameraPanel : CameraPanel
    {
        // TODO: Add messages being hidden when MessagePanel shows
        static private readonly string MORE_SYMBOL = " -- MORE --";
        static private readonly int MESSAGE_WIDTH = 60;
        static private readonly int MESSAGE_X = 0;
        static private readonly int MESSAGE_Y = 0;

        private List<string> splitMessages;

        // Takes messagerecall panel to indicate dependency on being shown
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

            // Keep up with messages as well
            splitMessages = new List<string>();
            MessageCenter.MessageWritten += onMessageWritten;
        }

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (splitMessages.Count != 0) // Any input goes to next message
                splitMessages.RemoveAt(0);
            else
            {
                Direction dirToMove = Direction.NONE;
                switch (Input.ActionFor(e.KeyPress))
                {
                    case InputAction.UP:
                        dirToMove = Direction.UP;
                        e.Cancel = true;
                        break;
                    case InputAction.UP_RIGHT:
                        dirToMove = Direction.UP_RIGHT;
                        e.Cancel = true;
                        break;
                    case InputAction.RIGHT:
                        dirToMove = Direction.RIGHT;
                        e.Cancel = true;
                        break;
                    case InputAction.DOWN_RIGHT:
                        dirToMove = Direction.DOWN_RIGHT;
                        e.Cancel = true;
                        break;
                    case InputAction.DOWN:
                        dirToMove = Direction.DOWN;
                        e.Cancel = true;
                        break;
                    case InputAction.DOWN_LEFT:
                        dirToMove = Direction.DOWN_LEFT;
                        e.Cancel = true;
                        break;
                    case InputAction.LEFT:
                        dirToMove = Direction.LEFT;
                        e.Cancel = true;
                        break;
                    case InputAction.UP_LEFT:
                        dirToMove = Direction.UP_LEFT;
                        e.Cancel = true;
                        break;
                    case InputAction.DOWN_STAIR:
                        // Check for stairy-stuff.  Probably need to split this off, teleporter may go away since direction would be diff (Up gate vs down-gate would be seperate classes).
                        foreach (var gObject in MapToRender.ObjectsAt(ApprenticeGame.Player.Position))
                            if (gObject is Teleporter teleporter)
                            {
                                teleporter.Traverse(ApprenticeGame.Player);
                                e.Cancel = true;
                                break;
                            }
                        break;
                    case InputAction.ACTIVATE: // Activate top layer of whatever is below us, if something can be activated
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

                    case InputAction.SPELLS_SCREEN:
                        ApprenticeGame.GameScreen.Hide();
                        ApprenticeGame.SpellsPanel.Show();
                        e.Cancel = true;
                        break;

                    case InputAction.MESSAGE_RECALL_SCREEN:
                        ApprenticeGame.GameScreen.Hide();
                        ApprenticeGame.MessageRecallPanel.Show();
                        e.Cancel = true;
                        break;
                }


                if (dirToMove != Direction.NONE)
                    if (!ApprenticeGame.Player.MoveIn(dirToMove))
                        ApprenticeGame.Player.Combat.AttackIn(dirToMove);
            }
        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            if (MapToRender != null)
                MapToRender.CalculateFOVIfNeeded(ApprenticeGame.Player.Position, 10, Radius.CIRCLE);

            base.UpdateLayout(sender, e);

            // Print messages, if necessary, on top of everything
            if (splitMessages.Count != 0)
            {
                string message = (splitMessages.Count > 1) ? splitMessages[0] + MORE_SYMBOL : splitMessages[0];
                console.Print(MESSAGE_X, MESSAGE_Y, message, RLColor.White);
            }
        }

        private void onMessageWritten(object s, MessageWrittenEventArgs e)
        {
            if (e.WasConsolidated) // Must be at least 1 message already in log
                splitMessages.RemoveAt(splitMessages.Count - 1); // If we consolidated we actually edited the last message.  So remove it so we re-split it.

            foreach (var split in MessageCenter.SplitMessage(e.Message, MESSAGE_WIDTH - MORE_SYMBOL.Length))
                splitMessages.Add(split);
        }
    }
}
