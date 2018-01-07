using System.Collections.Generic;
using GoRogue;

namespace Apprentice.World
{
    class NewMap
    {
        static public readonly int CHUNK_SIZE = 64;

        private Dictionary<Coord, Chunk> _chunks;

        private HashSet<Coord> _existingChunkPositions;
        public IReadOnlyCollection<Coord> ExistingChunkPositions { get => _existingChunkPositions; }

        public int Width { get; private set; }
        public int Height { get; private set; }


        public NewMap(int width, int height)
        {
            Width = MathHelpers.RoundToMultiple(width, CHUNK_SIZE);
            Height = MathHelpers.RoundToMultiple(height, CHUNK_SIZE);

            _chunks = new Dictionary<Coord, Chunk>();
            _existingChunkPositions = new HashSet<Coord>();
        }

        // Gets the grid chunk coordinate that contains the specified position.
        public Coord ChunkPositionFor(Coord position) => Coord.Get(position.X / CHUNK_SIZE, position.Y / CHUNK_SIZE);

        // If chunk at given chunk coordinate is loaded.
        public bool IsChunkLoaded(Coord chunkPosition) => _chunks.ContainsKey(chunkPosition);

        // Whether or not the chunk containing the specified position is loaded.
        public bool IsLoaded(Coord position) => _chunks.ContainsKey(ChunkPositionFor(position));

        // Whether a chunk for the given chunk grid position has ever been generated.
        public bool ChunkExists(Coord chunkPosition) => _existingChunkPositions.Contains(chunkPosition);

        // Whether a chunk containing data for the given position has ever been generated.
        public bool Exists(Coord position) => _existingChunkPositions.Contains(ChunkPositionFor(position));
    }
}
