using Apprentice.World;
using GoRogue;

namespace Apprentice.MapOfProviders
{
    class FOVProvider : IMapOf<double>
    {
        public Map MapRepresented { get; private set; }

        public int Width { get => MapRepresented.Width; }
        public int Height { get => MapRepresented.Height; }

        public FOVProvider(Map mapRepresented)
        {
            MapRepresented = mapRepresented;
        }

        public double this[int x, int y]
        {
            get => (MapRepresented.Terrain[x, y].IsTransparent) ? 0.0 : 1.0;
        }

        public double this[Coord pos]
        {
            get => (MapRepresented.Terrain[pos].IsTransparent) ? 0.0 : 1.0;
        }
    }
}
