using Apprentice.Effects;
using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects.Terrain
{
    // Piece of terrain that basically uses dimensional teleport effect.
    class Gate : GameObject
    {
        public Map DestinationMap;
        public Coord DestinationPosition;

        public Gate(Coord position, Map destinationMap, Coord destinationPosition)
            : base(position, Map.Layer.Terrain, '>', RLColor.Blue, true, true)
        {
            DestinationMap = destinationMap;
            DestinationPosition = destinationPosition;
        }

        // Call to send given object to destination.
        // TODO: Really this may end up wanting to do collision detection here to figure out who/what to send, in case monsters want to use.
        public void Traverse(GameObject gObject)
        {
            new Teleport().Trigger(new TeleportEffectArgs(gObject, DestinationMap, DestinationPosition));
        }
    }
}
