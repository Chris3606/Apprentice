using Apprentice.World;
using GoRogue;
using RLNET;

namespace Apprentice.GameObjects
{
    class Player : GameObject
    {
        private int _enemiesSeen;
        public int EnemiesSeen
        {
            get => _enemiesSeen;

            set
            {
                _enemiesSeen = value;

                System.Console.WriteLine($"{_enemiesSeen} enemies seen!");
            }
        }

        public Player(Coord position)
            : base(position, Map.Layer.Monsters, '@', false, true)
        {
            Combat = new Components.Combat(this, 10);

            _enemiesSeen = 0;

            Moved += (s, e) => { if (CurrentMap != null) CurrentMap.FOVNeedsRecalc = true; };
        }
    }
}
