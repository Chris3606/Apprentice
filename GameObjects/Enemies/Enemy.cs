using Apprentice.World;
using GoRogue;
using RLNET;


namespace Apprentice.GameObjects.Enemies
{
    class Enemy : GameObject
    {
        public bool BeenSeen { get; set; }

        public Enemy(Coord position, int character, RLColor foreground)
            : base(position, Map.Layer.Monsters, character, foreground, false, true)
        {
            BeenSeen = false;
        }
    }
}
