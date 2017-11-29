using Apprentice.GameObjects;
using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice
{
    class Gate : GameObject
    {
        public Map DestinationMap;
        public Coord DestinationPosition;

        public Gate(Coord position, Map destinationMap, Coord destinationPosition)
            : base(position, Map.Layer.Terrain, '>', RLColor.Blue, RLColor.Black, true, true)
        {
            DestinationMap = destinationMap;
            DestinationPosition = destinationPosition;
        }

        // Call to send given object to destination.
        // TODO: This should be an effect later.
        public void Traverse(GameObject gObject)
        {
            // TODO: Later this may need to join with thread to make sure initial generation is done.  Not sure if we'll generate dynamically or not, since multiple gates will be a thing.
            if (CurrentMap != DestinationMap)
            {
                CurrentMap.Remove(gObject); // Remove from where the gate is
                gObject.Position = DestinationPosition;
                DestinationMap.Add(gObject);
            }
            else
                gObject.Position = DestinationPosition;
        }
    }
}
