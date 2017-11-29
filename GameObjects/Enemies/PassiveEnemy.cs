using Apprentice.World;
using GoRogue;
using RLNET;


namespace Apprentice.GameObjects.Enemies
{
    class PassiveEnemy : Enemy
    {
        public PassiveEnemy(Coord position)
            : base(position, 'i', RLColor.Yellow)
        {
            Combat = new Components.Combat(this, 5);
        }
    }
}
