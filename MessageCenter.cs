using System;
using System.Collections.Generic;

namespace Apprentice
{
    
    class MessageWrittenEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public bool WasConsolidated { get; private set; }

        public MessageWrittenEventArgs(string message, bool wasConsolidated)
        {
            Message = message;
            WasConsolidated = wasConsolidated;
        }
    }

    // Will also deal with maximum message log stored, though this could make rendering a bit of a pain.
    static class MessageCenter
    {
        static private List<string> _messages = new List<string>();
        static public IList<string> Messages { get => _messages.AsReadOnly(); }
        static public event EventHandler<MessageWrittenEventArgs> MessageWritten;

        static private string lastMessage;
        static private int lastMessageCount;
        

        static public void Write(string message)
        {
            bool consolidated;
            if (message == lastMessage) // Can only happen when at least 1 msg exists since this is null to start.
            {
                lastMessageCount++;
                _messages[_messages.Count - 1] = lastMessage + $"(x{lastMessageCount})";
                consolidated = true;
            }
            else
            {
                _messages.Add(message);
                lastMessage = message;
                lastMessageCount = 1;
                consolidated = false;
            }
            MessageWritten?.Invoke(null, new MessageWrittenEventArgs(_messages[_messages.Count - 1], consolidated)); // Is no sender so we just use null here.
        }

        // Split based on word if we can find a word, otherwise return an exact split.
        // TODO: Can make more priorities like hyphens, etc later
        static public IEnumerable<string> SplitMessage(string message, int maxLineLength)
        {
            while (message != "")
            {
                if (message.Length <= maxLineLength)
                {
                    yield return message;
                    break;
                }

                int widthOfSubstring = maxLineLength;
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
