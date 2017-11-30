using GoRogue;
using Apprentice.World;

namespace Apprentice.GameObjects.Terrain
{
    // Learns create vector ability
    // TODO: If altars are an actual thing we would genericize this and give it an ability.
    class AltarOfVectoring : GameObject, IActivatable
    {
        public AltarOfVectoring(Coord position)
            : base(position, Map.Layer.Terrain, 'T') { }

        public void Activate()
        {
            // TODO: Set player up to gain research
            System.Console.WriteLine("Player should gain the vectoring ability here");
        }
    }
}
