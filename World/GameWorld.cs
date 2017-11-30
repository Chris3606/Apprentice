using GoRogue;
using GoRogue.Random;
using Apprentice.GameObjects.Terrain;

namespace Apprentice.World
{
    class GameWorld
    {
        public DemiPlane DemiPlane { get; private set; }
        public NeverNever NeverNever { get; private set; }
        public CaveofLearning CaveofLearning { get; private set; } // TEMPPPPPP

        // Generate new world with the given parameters.
        public GameWorld(int demiPlaneWidth, int demiPlaneHeight, int neverNeverWidth, int neverNeverHeight)
        {
            // Generate new DemiPlane
            DemiPlane = new DemiPlane(demiPlaneWidth, demiPlaneHeight);
            DemiPlane.Generate();

            // Generate new NeverNever
            NeverNever = new NeverNever(neverNeverWidth, neverNeverHeight);
            NeverNever.Generate();

            // Generate cave that teaches player to gate
            CaveofLearning = new CaveofLearning(11, 11);
            CaveofLearning.Generate();

            // Generate initial plane connectivity
            generateInitialConnections();
        }

        private void generateInitialConnections()
        {
            // Connect DemiPlane to NeverNever
            Coord gateSourcePosition = Map.RandomOpenPosition(DemiPlane, SingletonRandom.DefaultRNG);
            Coord gateDestinationPosition = Map.RandomOpenPosition(NeverNever, SingletonRandom.DefaultRNG);

            //DemiPlane.Remove(DemiPlane.Terrain[gateSourcePosition]);
            // will auto-replace terrain since we know it doesn't collide
            DemiPlane.Add(new Gate(gateSourcePosition, NeverNever, gateDestinationPosition));

            // Connect NeverNever to cave of learning via stairwell (for now just a gate, thoughthat's temp bc not sure
            // how far to split these classes bc its all visual controlled).
            Coord stairwellPosition = Map.RandomOpenPosition(NeverNever, SingletonRandom.DefaultRNG);
            Coord inCavePosition = Map.RandomOpenPosition(CaveofLearning, SingletonRandom.DefaultRNG);
            //DemiPlane.Remove(DemiPlane.Terrain[stairwellPosition]);
            NeverNever.Add(new Gate(stairwellPosition, CaveofLearning, inCavePosition));
        }
    }
}
