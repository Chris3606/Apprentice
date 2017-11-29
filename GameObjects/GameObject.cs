﻿using GoRogue;
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
        public int? Character { get; set; }
        public RLColor? Foreground { get; private set; }
        public RLColor? Background { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsTransparent { get; private set; }
        public uint ID { get; private set; }

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

        public GameObject(Coord position, Map.Layer layer, int? character, RLColor? foreground, RLColor? background, bool isWalkable = true, bool isTransparent = true)
        {
            Position = position;
            Layer = layer;
            Character = character;
            Foreground = foreground;
            Background = background;
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
            ID = idGen.UseID();
            _currentMap = null;

            Moved = null;
            AddedToMap = null;
            RemovedFromMap = null;
        }

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

            // Terrain always blocks.
            if (map.Terrain[position] != null && !map.Terrain[position].IsWalkable)
                return map.Terrain[position];

            // Otherwise it can't possibly collide if it's walkable.
            if (IsWalkable)
                return null;

            // If it isn't walkable, then it collides iff there is another colliding object already present.
            foreach (var gObject in map.Entities.GetItems(position))
                if (!gObject.IsWalkable)
                    return gObject;

            return null;
        }
    }
}
