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
                System.Console.WriteLine("Nothing can be learned here.");
            else
            {
                ApprenticeGame.Player.Caster.AddToLearned(new Spells.GateHome());
                System.Console.WriteLine("Player has learned the spell Gate Home!");
            }
        }
    }
}
