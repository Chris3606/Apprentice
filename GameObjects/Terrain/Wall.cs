using Apprentice.World;
using RLNET;
using GoRogue;

namespace Apprentice.GameObjects.Terrain
{
    class Wall : GameObject
    {
        public Wall(Coord position)
            : base(position, Map.Layer.Terrain, '#', false, false)
        { }
    }
}
