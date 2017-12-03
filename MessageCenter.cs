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
    }
}
