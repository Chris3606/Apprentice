using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects.Terrain
{
    class Tree : GameObject
    {
        public Tree(Coord position)
            : base(position, Map.Layer.Terrain, (int)FontChars.TREE, RLColor.Green, false, false)
        { }
    }
}
