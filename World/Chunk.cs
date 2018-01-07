using Apprentice.GameObjects;
using GoRogue;
using RLNET;

namespace Apprentice.World
{
    // Public data struct that represents a chunk.  Used internally by map to implement chunking
    class Chunk
    {
        public int Size { get; private set; }
        public ArrayMapOf<GameObject> Terrain { get; private set; }
        // Possibly temp, this may be more complicated later (layers)... or perhaps not.  Could just multi-spatial map everything.
        public MultiSpatialMap<GameObject> Entities { get; private set; }
        public IMapOf<RLColor?> BackgroundColors { get; private set; }

        public Chunk(int size)
        {
            Size = size;

            Terrain = new ArrayMapOf<GameObject>(size, size);
            Entities = new MultiSpatialMap<GameObject>();
            BackgroundColors = new ArrayMapOf<RLColor?>(size, size);
        }
    }
}
