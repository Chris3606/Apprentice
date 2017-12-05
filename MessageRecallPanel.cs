using RLNET;
using WinMan;
using System;
using System.Collections.Generic;

namespace Apprentice
{
    // Writes as many messages to the screen as will fit, updating whenever it's shown/when new msg is written)
    class MessageRecallPanel : Panel
    {
        static private int ROWS_BEFORE_MESSAGES = 2;

        private List<string> splitMessages; // List of all messages split appropriately.
        private int _indexOfLastDisplayed;
        private int indexOfLastDisplayed
        {
            get => _indexOfLastDisplayed;
            set
            {
                int max = (splitMessages.Count == 0) ? 0 : splitMessages.Count - 1;
                int min = Math.Min(splitMessages.Count - 1, (Height - ROWS_BEFORE_MESSAGES) - 1);
                _indexOfLastDisplayed = value;
                _indexOfLastDisplayed = Math.Min(_indexOfLastDisplayed, max);
                _indexOfLastDisplayed = Math.Max(_indexOfLastDisplayed, min);
            }
        }

        private int indexOfFirstDisplayed { get => Math.Max(0, (indexOfLastDisplayed + 1) - (Height - ROWS_BEFORE_MESSAGES)); }
        // TODO: Later this above will take into account limited messages.

        public MessageRecallPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, true, false) // Need to accept keyboard input
        {
            MessageCenter.MessageWritten += onMessageWritten;
            OnResize += onResize;

            splitMessages = new List<string>();
            indexOfLastDisplayed = 0;

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

                // Determine scroll bar colors
                RLColor upArrowColor = (indexOfFirstDisplayed != 0) ? RLColor.White : RLColor.Gray; // If we can't display all the messages we have, display up arrow white
                // If we can't display most recent ones, display down arrow white
                RLColor downArrowColor = (indexOfLastDisplayed != splitMessages.Count - 1) ? RLColor.White : RLColor.Gray;
                // Scroll bar is white unless its unused entirely then its gray, like both arrows
                RLColor scrollColor = RLColor.White;
                if (indexOfFirstDisplayed == 0 && indexOfLastDisplayed == splitMessages.Count - 1) // No need for scroll bar
                    scrollColor = RLColor.Gray;

                // Print scroll-bar.
                console.Set(Width - 1, ROWS_BEFORE_MESSAGES, upArrowColor, null, (int)FontChars.UP_ARROW);
                for (int yScroll = ROWS_BEFORE_MESSAGES + 1; yScroll < Height; yScroll++)
                    console.Set(Width - 1, yScroll, scrollColor, null, '|');
                console.Set(Width - 1, Height - 1, downArrowColor, null, (int)FontChars.DOWN_ARROW);


                // Print messages
                for (int i = indexOfFirstDisplayed; i <= indexOfLastDisplayed; i++)
                {
                    console.Print(0, y, splitMessages[i], RLColor.White);
                    y++; // Each message is guaranteed to fit on one line because they were split to
                }
            }
        }

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch(Input.ActionFor(e.KeyPress))
            {
                case InputAction.BACK:
                    e.Cancel = true;
                    Hide();
                    ApprenticeGame.GameScreen.Show();
                    break;

                case InputAction.UP:
                    e.Cancel = true;
                    indexOfLastDisplayed--;
                    UpdateLayout(this, new UpdateEventArgs(0.0));
                    break;

                case InputAction.DOWN:
                    e.Cancel = true;
                    indexOfLastDisplayed++;
                    UpdateLayout(this, new UpdateEventArgs(0.0));
                    break;
            }
        }

        private void onMessageWritten(object s, MessageWrittenEventArgs e)
        {
            if (e.WasConsolidated) // Must be at least 1 message already in log
                splitMessages.RemoveAt(splitMessages.Count - 1); // If we consolidated we actually edited the last message.  So remove it so we re-split it.

            foreach (var split in MessageCenter.SplitMessage(e.Message, Width - 1))
                splitMessages.Add(split);

            indexOfLastDisplayed = splitMessages.Count - 1;

            if (Shown)
                UpdateLayout(this, new UpdateEventArgs(0.0));
        }

        private void onResize(object sender,EventArgs e)
        {
            // Clear and re-split
            splitMessages.Clear();

            foreach (var message in MessageCenter.Messages)
                foreach (var split in MessageCenter.SplitMessage(message, Width - 1))
                    splitMessages.Add(split);

            indexOfLastDisplayed = splitMessages.Count - 1;
        }
    }
}
