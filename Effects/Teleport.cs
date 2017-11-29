using Apprentice.GameObjects;
using Apprentice.World;
using GoRogue;


namespace Apprentice.Effects
{
    class TeleportEffectArgs : TargetableEffectArgs
    {
        public Map DestinationMap { get; private set; }
        public Coord DestinationPosition { get; private set; }

        public TeleportEffectArgs(GameObject target, Map destinationMap, Coord destinationPosition)
            : base(target)
        {
            DestinationMap = destinationMap;
            DestinationPosition = destinationPosition;
        }
    }

    class Teleport : Effect<TeleportEffectArgs>
    {
        public Teleport()
            : base("Teleport", INSTANT) { }

        protected override void OnTrigger(TeleportEffectArgs e)
        {
            if (e.Target.CurrentMap != e.DestinationMap)
            {
                e.Target.CurrentMap.Remove(e.Target); // Remove from where the gate is
                e.Target.Position = e.DestinationPosition;
                e.DestinationMap.Add(e.Target);
            }
            else
                e.Target.Position = e.DestinationPosition;
        }
    }
}
