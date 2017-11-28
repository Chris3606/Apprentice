using GoRogue;
using Apprentice.GameObjects;

namespace Apprentice
{
    abstract class Map
    {
        public enum Layer { Terrain, Items, Monsters };

        public int Width { get; private set; }
        public int Height { get; private set; }

        protected ArrayMapOf<GameObject> _terrain;
        public IMapOf<GameObject> Terrain { get => _terrain; }

        // Possibly temp, this may be more complicated later (layers)... or perhaps not.  Could just multi-spatial map everything.
        protected MultiSpatialMap<GameObject> _entities;
        public IReadOnlySpatialMap<GameObject> Entities { get => _entities.AsReadOnly(); }

        protected Map(int width, int height)
        {
            Width = width;
            Height = height;

            _terrain = new ArrayMapOf<GameObject>(width, height);
            _entities = new MultiSpatialMap<GameObject>();
        }

        // Tells the map to generate itself.  Needs to be called somewhere else other than Map, map's constructor can't call it.
        abstract public void Generate();
    }
}
