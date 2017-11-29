using Apprentice.Effects;
using Apprentice.GameObjects.Items;
using GoRogue;
using GoRogue.Random;
using System;

namespace Apprentice.GameObjects.Components
{
    class Combat : Component
    {
        private int _hp;
        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                if (IsDead)
                    Died?.Invoke(this, EventArgs.Empty);
            }
        }
        public int MaxHP { get; private set; }

        public bool IsDead { get => _hp <= 0; }

        public EventHandler Died;

        public Combat(GameObject parent, int maxHP)
            : base(parent)
        {
            MaxHP = _hp = maxHP;

            Died += onDeath;

        }

        public bool MoveOrAttackIn(Direction direction)
        {
            if (Parent.MoveIn(direction))
                return true;

            var collider = Parent.CurrentMap.CollidingObjectAt(Parent.Position + direction);

            // Collider must not be null, we collided!
            if (collider.Combat != null)
            {
                new PhysicalDamage().Trigger(new DamageEffectArgs(collider, SingletonRandom.DefaultRNG.Next(1, 5)));
                return true;
            }

            return false;
        }

        private void onDeath(object sender, EventArgs e)
        {
            if (Parent.CurrentMap != null)
            {
                var map = Parent.CurrentMap;
                map.Remove(Parent);
                map.Add(new Corpse(Parent.Position));
            }
        }
    }
}
