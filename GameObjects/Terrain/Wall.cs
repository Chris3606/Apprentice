using Apprentice.Maps;
using RLNET;
using GoRogue;

namespace Apprentice.GameObjects.Terrain
{
    class Wall : GameObject
    {
        public Wall(Coord position)
            : base(position, Map.Layer.Terrain, '#', RLColor.White, null, false, false)
        { }
    }
}
