using Apprentice.GameObjects;
using GoRogue;

namespace Apprentice.Effects
{
    class DamageEffectArgs : TargetableEffectArgs
    {
        public int Damage { get; private set; }

        public DamageEffectArgs(GameObject target, int damage)
            : base(target)
        {
            Damage = damage;
        }
    }
    class PhysicalDamage : Effect<DamageEffectArgs>
    {
        public PhysicalDamage()
            : base("Physical damange", INSTANT) { }

        protected override void OnTrigger(DamageEffectArgs e)
        {
            if (e.Target.Combat == null)
                throw new System.Exception("Tried to damage non damageable target...");

            // Resistances would happen here but we dont have those yet.
            e.Target.Combat.HP -= e.Damage;
        }
    }
}
