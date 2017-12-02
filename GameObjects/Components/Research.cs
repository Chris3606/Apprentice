using System.Collections.Generic;
using Apprentice.Research;

namespace Apprentice.GameObjects.Components
{
    // Allows a class to participate in (track) research.  Typically only attached to a player
    class Research : Component
    {
        private List<ResearchItem> _researchItems;
        public IList<ResearchItem> ResearchItems { get => _researchItems; }

        public Research(GameObject parent)
            : base(parent)
        {
            _researchItems = new List<ResearchItem>();
        }

        public void Add(ResearchItem item)
        {
            if (item.Parent == this)
            {
                System.Console.WriteLine("WARNING: Tried to add item to research twice.  This is a bug...");
                return;
            }

            if (item.Parent != null)
                item.Parent.Remove(item);

            _researchItems.Add(item);
            item.StartTracking();
        }

        public void Remove(ResearchItem item)
        {
            if (item.Parent != this)
            {
                System.Console.WriteLine("WARNING: Tried to remove research item that doesn't exist.  This is a bug...");
                return;
            }

            _researchItems.Remove(item);
            item.StopTracking();
        }
    }
}
