using Apprentice.World;
using Apprentice.GameObjects.Components;
using GoRogue;
using RLNET;
using System;

namespace Apprentice.GameObjects
{
    class MovedEventArgs : EventArgs
    {
        public Coord OldPosition { get; private set; }
        public Coord NewPosition { get; private set; }

        public MovedEventArgs(Coord oldPosition, Coord newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }

    class MapChangedEventArgs : EventArgs
    {
        public Map Map { get; private set; }

        public MapChangedEventArgs(Map map)
        {
            Map = map;
        }
    }

    class GameObject : IHasID
    {
        static private readonly IDGenerator idGen = new IDGenerator();

        private Coord _position;
        public Coord Position
        {
            get => _position;
            set
            {
                if (_position != value && !CollidesAt(value))
                {
                    Coord oldPos = _position;
                    _position = value;
                    Moved?.Invoke(this, new MovedEventArgs(oldPos, _position));
                }
            }
        }

        public Map.Layer Layer { get; private set; }
        public int Character { get; set; }
        public RLColor Foreground { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsTransparent { get; private set; }
        public uint ID { get; private set; }

        // Components
        public Combat Combat { get; protected set; }

        private Map _currentMap;
        // NOBODY but map should change this.
        public Map CurrentMap
        {
            get => _currentMap;
            internal set
            {
                if (_currentMap != value)
                {
                    if (_currentMap != null)
                        RemovedFromMap?.Invoke(this, new MapChangedEventArgs(_currentMap));

                    _currentMap = value; // We assume this has effectively already happened, eg. map is verified.  Do NOT call this set if you are not the all-knowing map!

                    if (_currentMap != null)
                        AddedToMap?.Invoke(this, new MapChangedEventArgs(_currentMap));
                }
            }
        }

        public event EventHandler<MovedEventArgs> Moved;
        public event EventHandler<MapChangedEventArgs> AddedToMap;
        public event EventHandler<MapChangedEventArgs> RemovedFromMap;

        public GameObject(Coord position, Map.Layer layer, int character, RLColor foreground, bool isWalkable = true, bool isTransparent = true)
        {
            Position = position;
            Layer = layer;
            Character = character;
            Foreground = foreground;
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
            ID = idGen.UseID();
            _currentMap = null;

            Moved = null;
            AddedToMap = null;
            RemovedFromMap = null;
        }

        public GameObject(Coord position, Map.Layer layer, int character, bool isWalkable = true, bool isTransparent = true)
            : this(position, layer, character, RLColor.White, isWalkable, isTransparent)
        { }

        public bool MoveIn(Direction direction)
        {
            Coord oldPos = _position;
            Position += direction;
            return oldPos != _position;
        }

        public bool CollidesAt(Coord position) => CollidesAt(position, _currentMap);
        public bool CollidesAt(Coord position, Map map) => CollidingObject(position, map) != null;

        public GameObject CollidingObject(Coord position) => CollidingObject(position, _currentMap);
        public GameObject CollidingObject(Coord position, Map map)
        {
            if (map == null)
                return null;

            // Get colliding object; if there is none we can't collide.
            var collidingObject = map.CollidingObjectAt(position);
            if (collidingObject == null)
                return null;

            // Terrain always collides
            if (collidingObject.Layer == Map.Layer.Terrain)
                return collidingObject;

            // If we're walkable, we can't collide
            if (IsWalkable)
                return null;

            // If we're not walkable, that other entity must block us
            return collidingObject;
        }
    }
}
