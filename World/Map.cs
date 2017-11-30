using GoRogue;
using GoRogue.Random;
using Apprentice.GameObjects;
using Apprentice.MapOfProviders;
using RLNET;
using System.Collections.Generic;

namespace Apprentice.World
{
    abstract class Map
    {
        public enum Layer { Terrain, Items, Monsters };

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle Bounds { get => new Rectangle(0, 0, Width, Height); }
        public bool FOVNeedsRecalc { get; set; }

        private ArrayMapOf<GameObject> _terrain;
        public IMapOf<GameObject> Terrain { get => _terrain; }

        // Possibly temp, this may be more complicated later (layers)... or perhaps not.  Could just multi-spatial map everything.
        private MultiSpatialMap<GameObject> _entities;
        public IReadOnlySpatialMap<GameObject> Entities { get => _entities.AsReadOnly(); }

        private ArrayMapOf<RLColor?> _backgroundColors;
        public IMapOf<RLColor?> BackgroundColors { get => _backgroundColors; }

        private FOVProvider fovProvider;
        private LOS fov;
        RadiusAreaProvider losAreaProvider;
        private bool[,] explored;
        
        protected Map(int width, int height)
        {
            Width = width;
            Height = height;

            _terrain = new ArrayMapOf<GameObject>(width, height);
            _entities = new MultiSpatialMap<GameObject>();
            _backgroundColors = new ArrayMapOf<RLColor?>(width, height);
            FOVNeedsRecalc = true;

            fovProvider = new FOVProvider(this);
            fov = new LOS(fovProvider);
            losAreaProvider = new RadiusAreaProvider(Coord.Get(0, 0), 0, Radius.DIAMOND, Bounds);
            explored = new bool[Width, Height];
        }

        // Tells the map to generate itself.  Needs to be called somewhere else other than Map, Map's constructor can't call it because it's abstract,
        // and probably shouldn't anyway because of need for map loading.
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

        public IEnumerable<GameObject> ObjectsAt(Coord position)
        {
            if (_terrain[position] != null)
                yield return _terrain[position];

            foreach (var gObject in _entities.GetItems(position))
                yield return gObject;
        }

        public void CalculateFOVIfNeeded(Coord position, int radius, Radius radiusShape)
        {
            if (FOVNeedsRecalc)
            {
                FOVNeedsRecalc = false;
                losAreaProvider.Center = position;
                losAreaProvider.Radius = radius; // Library automatically checks if this actually changed before doing anything, pre-emptive set ok here
                fov.Calculate(position.X, position.Y, radius, radiusShape);

                // For everything in FOV, it's explored.
                foreach (var pos in losAreaProvider.Positions())
                    if (fov[pos] > 0.0) 
                        explored[pos.X, pos.Y] = true;
            }
        }

        public bool IsExplored(int x, int y) => explored[x, y];
        public bool IsExplored(Coord position) => explored[position.X, position.Y];

        public double FOVAt(Coord position) => fov[position];
        public double FOVAt(int x, int y) => fov[x, y];

        // Sets map bg color to specified value
        protected void SetBackgroundColor(RLColor color)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _backgroundColors[x, y] = color;
        }

        protected void SetBackgroundColor(Coord pos, RLColor color)
        {
            _backgroundColors[pos] = color;
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
