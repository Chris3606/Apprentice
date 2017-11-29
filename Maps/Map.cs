using GoRogue;
using GoRogue.Random;
using Apprentice.GameObjects;

namespace Apprentice.Maps
{
    abstract class Map
    {
        public enum Layer { Terrain, Items, Monsters };

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle Bounds { get => new Rectangle(0, 0, Width, Height); }

        private ArrayMapOf<GameObject> _terrain;
        public IMapOf<GameObject> Terrain { get => _terrain; }

        // Possibly temp, this may be more complicated later (layers)... or perhaps not.  Could just multi-spatial map everything.
        private MultiSpatialMap<GameObject> _entities;
        public IReadOnlySpatialMap<GameObject> Entities { get => _entities.AsReadOnly(); }

        protected Map(int width, int height)
        {
            Width = width;
            Height = height;

            _terrain = new ArrayMapOf<GameObject>(width, height);
            _entities = new MultiSpatialMap<GameObject>();
        }

        // Tells the map to generate itself.  Needs to be called somewhere else other than Map, Map's constructor can't call it because it's abstract.
        abstract public void Generate();

        // Adds entity to the map.
        public bool Add(GameObject gObject)
        {
            // Outside of map
            if (!Bounds.Contains(gObject.Position))
                return false;

            if (gObject.CollidesAt(gObject.Position, this))
                return false;

            if (gObject.CurrentMap != null)
                Remove(gObject);

            if (gObject.Layer == Layer.Terrain)
                _terrain[gObject.Position] = gObject;
            else // Is entity, need to keep track of spatial map position updates
            {
                _entities.Add(gObject, gObject.Position);
                gObject.Moved += onEntityMoved;
            }

            gObject.CurrentMap = this;
            return true;
        }

        // Removes entity from the map.
        public void Remove(GameObject gObject)
        {
            if (gObject.Layer == Layer.Terrain)
            {
                if (_terrain[gObject.Position] == gObject)
                {
                    _terrain[gObject.Position] = null;
                    gObject.CurrentMap = null;
                }
                else
                    System.Console.WriteLine("WARNING: Tried to remove a GameObject from a map of which it was not a part.  This is a bug...");
            }
            else if (_entities.Remove(gObject))
            {
                gObject.Moved -= onEntityMoved;
                gObject.CurrentMap = null;
            }
            else
                System.Console.WriteLine("WARNING: Tried to remove a GameObject from a map of which it was not a part.  This is a bug...");
        }

        public GameObject CollidingObjectAt(Coord position)
        {
            // Terrain always blocks.
            if (Terrain[position] != null && !Terrain[position].IsWalkable)
                return Terrain[position];

            // Return any non-walkable object, assuming no collisions.
            foreach (var gObject in Entities.GetItems(position))
                if (!gObject.IsWalkable)
                    return gObject;

            return null;
        }

        private void onEntityMoved(object s, MovedEventArgs e)
        {
            var gObject = (GameObject)s;
            _entities.Move(gObject, e.NewPosition);
        }

        // Chooses a position with no colliding objects.
        public static Coord RandomOpenPosition(Map map, IRandom rng)
        {
            Coord pos = Coord.Get(rng.Next(map.Width - 1), rng.Next(map.Height - 1));
            while (map.CollidingObjectAt(pos) != null)
                pos = Coord.Get(rng.Next(map.Width - 1), rng.Next(map.Height - 1));

            return pos;
        }

        // Takes MapOf that tells it whether it can take a certain position or not.
        public static Coord RandomOpenPosition(IMapOf<bool> map, IRandom rng)
        {
            Coord pos = Coord.Get(rng.Next(map.Width - 1), rng.Next(map.Height - 1));
            while (!map[pos])
                pos = Coord.Get(rng.Next(map.Width - 1), rng.Next(map.Height - 1));

            return pos;
        }
    }
}
