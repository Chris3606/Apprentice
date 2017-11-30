using Apprentice.Effects;
using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects
{
    // Object uses dimensional teleport effect -- things like stairs, gates, etc.
    class Teleporter : GameObject
    {
        public Map DestinationMap;
        public Coord DestinationPosition;

        public Teleporter(Coord position, Map.Layer layer, int character, RLColor foreColor, Map destinationMap, Coord destinationPosition)
            : base(position, layer, character, foreColor, true, true)
        {
            DestinationMap = destinationMap;
            DestinationPosition = destinationPosition;
        }

        // Call to send given object to destination.
        public void Traverse(GameObject gObject)
        {
            new Teleport().Trigger(new TeleportEffectArgs(gObject, DestinationMap, DestinationPosition));
        }
    }
}
