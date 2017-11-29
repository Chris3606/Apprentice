using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects.Terrain
{
    class Tree : GameObject
    {
        public Tree(Coord position)
            : base(position, Map.Layer.Terrain, 6, RLColor.Green, RLColor.Black, false, false)
        { }
    }
}
