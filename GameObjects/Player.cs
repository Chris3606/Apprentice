using GoRogue;
using RLNET;

namespace Apprentice.GameObjects
{
    class Player : GameObject
    {
        public Player(Coord position)
            : base(position, Map.Layer.Monsters, '@', RLColor.White, null, false, true)
        { }
    }
}
