using Apprentice.World;
using GoRogue;

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
                System.Console.WriteLine($"{_enemiesSeen} enemies seen!"); // TODO: May eliminate, or move fov spot out
            }
        }

        public Components.Research Research { get; private set; }

        public Player(Coord position)
            : base(position, Map.Layer.Monsters, '@', false, true)
        {
            Combat = new Components.Combat(this, 10);
            Caster = new Components.Caster(this);

            _enemiesSeen = 0;
            Research = new Components.Research(this);

            Moved += (s, e) => { if (CurrentMap != null) CurrentMap.FOVNeedsRecalc = true; };
        }
    }
}
