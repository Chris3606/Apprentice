using GoRogue;
using GoRogue.Random;

namespace Apprentice.World
{
    class GameWorld
    {
        public DemiPlane DemiPlane { get; private set; }
        public NeverNever NeverNever { get; private set; }

        // Generate new world with the given parameters.
        public GameWorld(int demiPlaneWidth, int demiPlaneHeight, int neverNeverWidth, int neverNeverHeight)
        {
            // Generate new DemiPlane
            DemiPlane = new DemiPlane(demiPlaneWidth, demiPlaneHeight);
            DemiPlane.Generate();

            // Generate new NeverNever
            NeverNever = new NeverNever(neverNeverWidth, neverNeverHeight);
            NeverNever.Generate();

            // Generate initial plane connectivity
            generateInitialConnections();
        }

        private void generateInitialConnections()
        {
            Coord gateSourcePosition = Map.RandomOpenPosition(DemiPlane, SingletonRandom.DefaultRNG);
            Coord gateDestinationPosition = Map.RandomOpenPosition(NeverNever, SingletonRandom.DefaultRNG);

            DemiPlane.Remove(DemiPlane.Terrain[gateSourcePosition]);
            DemiPlane.Add(new Gate(gateSourcePosition, NeverNever, gateDestinationPosition));
        }
    }
}
