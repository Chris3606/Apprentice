using System;

namespace Apprentice.Research
{
    // Should be subclassed for each research quest, to tell how its completed, etc.
    abstract class ResearchItem
    {
        public EventHandler ResearchComplete;

        public bool Complete { get; private set; }
        public GameObjects.Components.Research Parent { get; private set; }

        public ResearchItem(GameObjects.Components.Research parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            Complete = false;
            ResearchComplete = null;
        }

        // Calls stopTracking -- should be called by subclass to indicate when research has finished.
        protected void FireResearchComplete()
        {
            StopTracking();
            Complete = true;
            ResearchComplete?.Invoke(this, EventArgs.Empty);
        }

        // Implement in subclasses to hook/unhook from necessary events
        abstract public void StartTracking();
        abstract public void StopTracking();
    }
}
