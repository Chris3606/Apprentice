using GoRogue;
using GoRogue.MapGeneration.Generators;
using Apprentice.GameObjects.Terrain;

namespace Apprentice.World
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
            RectangleMapGenerator.Generate(terrainGen);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (terrainGen[x, y])
                        Add(new Floor(Coord.Get(x, y)));
                    else
                        Add(new Wall(Coord.Get(x, y)));

            
            
        }
    }
}
