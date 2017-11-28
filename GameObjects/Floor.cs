﻿using RLNET;
using GoRogue;

namespace Apprentice.GameObjects
{
    class Floor : GameObject
    {
        public Floor(Coord position)
            : base(position, Map.Layer.Terrain, null, null, RLColor.Black, true, true)
        { }
    }
}
