using Apprentice.World;
using GoRogue;


namespace Apprentice.GameObjects.Items
{
    class Corpse : GameObject
    {
        public Corpse(Coord position)
            : base(position, Map.Layer.Items, '%')
        {

        }
    }
}
