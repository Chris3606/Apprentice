using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects
{
    class Player : GameObject
    {
        public Player(Coord position)
            : base(position, Map.Layer.Monsters, '@', false, true)
        {
            Moved += (s, e) => { if (CurrentMap != null) CurrentMap.FOVNeedsRecalc = true; };
        }
    }
}
