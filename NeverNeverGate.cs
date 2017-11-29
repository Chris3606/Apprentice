using Apprentice.GameObjects;
using Apprentice.Maps;
using GoRogue;
using RLNET;

namespace Apprentice
{
    class NeverNeverGate : GameObject
    {
        public NeverNeverGate(Coord position)
            : base(position, Map.Layer.Terrain, '>', RLColor.Blue, null, true, true)
        {

        }

        // Call to go to never-never at spawn point.
        // TODO: Later the gates will need to connect to specific positions inside the never-never, instead of just using the one spawn point.
        public void Traverse()
        {
            // TODO: Later this may need to join with thread to make sure initial generation is done.  Not sure if we'll generate dynamically or not, since multiple gates will be a thing.
            ApprenticeGame.ChangeActiveMap(ApprenticeGame.NeverNever, ApprenticeGame.NeverNever.SpawnPoint);
        }
    }
}
