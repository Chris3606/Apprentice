using GoRogue;
using RLNET;

namespace Apprentice.GameObjects
{
    class GameObject : IHasID
    {
        static private IDGenerator idGen = new IDGenerator();

        public Coord Position { get; private set; }
        public Map.Layer Layer { get; private set; }
        public int? Character { get; set; }
        public RLColor? Foreground { get; private set; }
        public RLColor? Background { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool IsTransparent { get; private set; }
        public uint ID { get; private set; }

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
        }
    }
}
