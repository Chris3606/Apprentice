using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.Random;
using Apprentice.GameObjects.Terrain;

namespace Apprentice.Maps
{
    class DemiPlane : Map
    {
        public DemiPlane(int width, int height)
            : base(width, height)
        { }

        // Generate basic terrain, no connectivity stuff
        public sealed override void Generate()
        {
            var terrainGen = new ArrayMapOf<bool>(Width, Height);
            new RectangleMapGenerator(terrainGen).Generate();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (terrainGen[x, y])
                        Add(new Floor(Coord.Get(x, y)));
                    else
                        Add(new Wall(Coord.Get(x, y)));

            
            
        }
    }
}
