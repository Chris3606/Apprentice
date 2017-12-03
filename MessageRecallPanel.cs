using RLNET;
using WinMan;
using System;
using System.Collections.Generic;

namespace Apprentice
{
    // Writes as many messages to the screen as will fit, updating whenever it's shown/when new msg is written)
    class MessageRecallPanel : Panel
    {
        static private readonly int ROWS_BEFORE_MESSAGES = 2;
        static private readonly string MORE_INDICATOR = "-- MORE --";

        private List<string> splitMessages; // List of all messages split appropriately.
        // TODO: Later this above will take into account limited messages.

        public MessageRecallPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, true, false) // Need to accept keyboard input
        {
            MessageCenter.MessageWritten += onMessageWritten;
            OnResize += onResize;

            splitMessages = new List<string>();

        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            console.Print(0, 0, "MESSAGE RECALL", RLColor.White);

            if (MessageCenter.Messages.Count == 0)
                console.Print(0, ROWS_BEFORE_MESSAGES, "No Messages", RLColor.White);
            else
            {
                int y = ROWS_BEFORE_MESSAGES;
                int i = Math.Max(0, splitMessages.Count - (Height - ROWS_BEFORE_MESSAGES));

                if (i != 0) // We can't display all the messages we have, display more indicator
                {
                    i++; // We can display one less because we have the more indicator
                    console.Print((Width / 2) - (MORE_INDICATOR.Length / 2), y, MORE_INDICATOR, RLColor.White);
                    y++;
                }

                while (i < splitMessages.Count)
                {
                    console.Print(0, y, splitMessages[i], RLColor.White);
                    y++; // Each message is guaranteed to fit on one line because they were split to
                    i++;
                }
            }
        }

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyPress.Key == RLKey.Escape)
            {
                e.Cancel = true;
                Hide();
                ApprenticeGame.GameScreen.Show();
            }
            // TODO: Temp, Test messages
            else if (e.KeyPress.Key == RLKey.A)
            {
                MessageCenter.Write("This is an alternative message!");
                e.Cancel = true;
            }
            else if (e.KeyPress.Key == RLKey.B)
            {
                MessageCenter.Write("This is a 2-line message.  It does not fit on one line because it is really long, and therefore should be automatically split as needed to fit in the message window.");
                e.Cancel = true;
            }
            else if (e.KeyPress.Key == RLKey.C)
            {
                MessageCenter.Write("This is a secondarily alternative message.");
                e.Cancel = true;
            }
        }

        private void onMessageWritten(object s, MessageWrittenEventArgs e)
        {
            if (e.WasConsolidated)
                splitMessages.RemoveAt(splitMessages.Count - 1); // If we consolidated we actually edited the last message.  So remove it so we re-split it.

            foreach (var split in splitMessage(e.Message))
                splitMessages.Add(split);

            if (Shown)
                UpdateLayout(this, new UpdateEventArgs(0.0));
        }

        private void onResize(object sender,EventArgs e)
        {
            // Clear and re-split
            splitMessages.Clear();

            foreach (var message in MessageCenter.Messages)
                foreach (var split in splitMessage(message))
                    splitMessages.Add(split);
        }

        // Split based on word if we can find a word, otherwise return an exact split.
        // TODO: Can make more priorities like hyphens, etc later
        private IEnumerable<string> splitMessage(string message)
        {
            while (message != "")
            {
                if (message.Length <= Width)
                {
                    yield return message;
                    break;
                }

                int widthOfSubstring = Width;
                int exactWidth = widthOfSubstring;


                while (widthOfSubstring >= 0 && message[widthOfSubstring] != ' ')
                    widthOfSubstring--;

                if (widthOfSubstring == 0) // We couldn't find a word boundary, so arbitrarily take as much as we can find
                    widthOfSubstring = exactWidth;

                yield return message.Substring(0, widthOfSubstring);
                message = message.Substring(widthOfSubstring + 1); // Get rid of part we just returned, and add 1 to ignore the space.
            }
        }


    }
}
