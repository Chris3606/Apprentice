using GoRogue;
using Apprentice.World;
using RLNET;

namespace Apprentice.GameObjects.Terrain
{
    class Gate : Teleporter
    {
        public Gate(Coord position, Map destinationMap, Coord destinationPosition)
            : base(position, Map.Layer.Terrain, '>', RLColor.Blue, destinationMap, destinationPosition) { }
    }
}
