using GoRogue.MapGeneration;
using GoRogue;
using GoRogue.Random;
using Apprentice.GameObjects.Terrain;

namespace Apprentice.World
{
    // Rectangular Map, simple black background.  Duplicate code with DemiPlane for now, but they'll be seperate later.
    class CaveofLearning : Map
    {
        public CaveofLearning(int width, int height)
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

            Add(new AltarOfVectoring(Coord.Get(Width / 2, Height / 2)));
        }
    }
}
