using Apprentice.World;
using RLNET;
using GoRogue;

namespace Apprentice.GameObjects.Terrain
{
    class Floor : GameObject
    {
        public Floor(Coord position)
            : base(position, Map.Layer.Terrain, null, null, RLColor.Black, true, true)
        { }
    }
}
