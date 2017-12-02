using GoRogue;
using Apprentice.World;

namespace Apprentice.GameObjects.Terrain
{
    // Learns gate-home spell
    // TODO: If altars are an actual thing we would genericize this and give it an ability.
    class AltarOfGating : GameObject, IActivatable
    {
        public AltarOfGating(Coord position)
            : base(position, Map.Layer.Terrain, 'T') { }

        public void Activate()
        {
            // TODO: Handle duplicates better, etc.
            if (ApprenticeGame.Player.Caster.KnowsSpell("Gate Home"))
                MessageCenter.Write("You have learned all you can from these runes.");
            else
            {
                ApprenticeGame.Player.Caster.AddToLearned(new Spells.GateHome());
                MessageCenter.Write("You study the magic runes, and learn the spell Gate Home!");
            }
        }
    }
}
