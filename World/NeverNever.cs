using Apprentice.GameObjects.Enemies;
using Apprentice.GameObjects.Terrain;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.Random;
using RLNET;

namespace Apprentice.World
{
    // Might be a temp class.  For now just generates the pleasant part that they player goes to.
    class NeverNever : Map
    {
        public NeverNever(int width, int height)
            : base(width, height)
        { }

        // Use generate here for now, loading can be implemented by another function later in place of generate.
        public sealed override void Generate()
        {
            var terrainGen = new ArrayMapOf<bool>(Width, Height);
            new RectangleMapGenerator(terrainGen).Generate();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (terrainGen[x, y])
                    {
                        if (SingletonRandom.DefaultRNG.Next(1, 100) > 95)
                            Add(new Tree(Coord.Get(x, y)));
                        else
                            Add(new Floor(Coord.Get(x, y)));
                    }
                    else
                        Add(new Wall(Coord.Get(x, y)));

            SetBackgroundColor(new RLColor(0, .05f, 0)); // Dark green, need to pull this out.

            // Generate enemies
            for (int i = 0; i < 5; i++)
            {
                Coord enemyPos = RandomOpenPosition(this, SingletonRandom.DefaultRNG);
                Add(new PassiveEnemy(enemyPos));
            }
        }
    }
}
