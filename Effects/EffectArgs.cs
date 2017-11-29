using Apprentice.GameObjects;
using GoRogue;

namespace Apprentice.Effects
{
    class TargetableEffectArgs : EffectArgs
    {
        public GameObject Target { get; private set; }

        public TargetableEffectArgs(GameObject target) { Target = target; }
    }

    
}
