using Apprentice.Effects;
using Apprentice.World;
using GoRogue;
using GoRogue.Random;

namespace Apprentice.Spells
{
    class GateHome : Spell
    {
        public GateHome() : base("Gate Home") { }

        protected override void OnCast()
        {
            Coord demiPlanePos = Map.RandomOpenPosition(ApprenticeGame.World.DemiPlane, SingletonRandom.DefaultRNG);
            new Teleport().Trigger(new TeleportEffectArgs(Owner, ApprenticeGame.World.DemiPlane, demiPlanePos));
        }
    }
}
