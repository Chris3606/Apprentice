using RLNET;
using GoRogue;

namespace Apprentice.GameObjects
{
    class Wall : GameObject
    {
        public Wall(Coord position)
            : base(position, Map.Layer.Terrain, '#', RLColor.White, null, false, false)
        { }
    }
}
