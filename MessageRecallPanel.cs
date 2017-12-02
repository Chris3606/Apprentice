using RLNET;
using WinMan;
using System;

namespace Apprentice
{
    // Writes as many messages to the screen as will fit, updating whenever it's shown/when new msg is written)
    class MessageRecallPanel : Panel
    {
        static private readonly int ROWS_BEFORE_MESSAGES = 2;
        public MessageRecallPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, true, false) // Need to accept keyboard input
        {
            MessageCenter.MessageWritten += onMessageWritten;
        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            console.Print(0, 0, "MESSAGE RECALL", RLColor.White);

            if (MessageCenter.Messages.Count == 0)
                console.Print(0, ROWS_BEFORE_MESSAGES, "No Messages", RLColor.White);

            else
            {
                var positionToPrintFrom = findFittingMessages(Width, Height - ROWS_BEFORE_MESSAGES); // Top is used up elsewhere, plus 1 for margin
                int i = positionToPrintFrom.Item1;
                int y = ROWS_BEFORE_MESSAGES;
                // Whatever portion of this msg fits.  TODO: Or just cull this msg entirely?  Either way.  Advance line count by this many.
                y += console.Print(0, y, MessageCenter.Messages[i].Substring(positionToPrintFrom.Item2), RLColor.White, null, Width); 
                i++;
                for (; i < MessageCenter.Messages.Count; i++)
                    y += console.Print(0, y, MessageCenter.Messages[i], RLColor.White, null, Width);
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
        }

        private void onMessageWritten(object s, MessageWrittenEventArgs e)
        {
            if (Shown)
                UpdateLayout(this, new UpdateEventArgs(0.0));
        }

        // Finds index of message in MessageCenter, along with index inside that message to start at, to print from such that maximum
        // messages are fit on the screen.
        private static Tuple<int, int> findFittingMessages(int width, int height)
        {
            int rows = 0;
            for (int i = MessageCenter.Messages.Count - 1; i >= 0; i--)
            {
                int linesNeeded = (int)Math.Ceiling(MessageCenter.Messages[i].Length / (double)width);
                if (rows + linesNeeded >= height) // We're out of space.  Split as necessary
                    return new Tuple<int, int>(i, startPointForMessage(MessageCenter.Messages[i], width * (height - rows)));

                rows += linesNeeded;
            }

            return new Tuple<int, int>(0, 0); // All messages
        }

        // Just returns position in original string to start at to get close to the characters you want.  Right now its exact, but
        // TODO: in the future it will split based on closest word, probably.
        private static int startPointForMessage(string message, int availableCharacters)
        {
            return Math.Max(0, message.Length - availableCharacters);
        }
    }
}
