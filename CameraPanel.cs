using GoRogue;
using RLNET;
using WinMan;
using Apprentice.GameObjects;
using Apprentice.World;
using System;

namespace Apprentice
{
    // Designed to display a portion of specified map to its console, respecting the map's FOV as it does so.
    class CameraPanel : Panel
    {
        private Map _mapToRender;
        public Map MapToRender
        {
            get => _mapToRender;
            set
            {
                if (_mapToRender != value)
                {
                    _mapToRender = value;
                    recalcActualPosition();
                }
            }
        }

        private Coord _cameraPosition;
        public Coord CameraPosition
        {
            get => _cameraPosition;
            set
            {
                if (_cameraPosition != value)
                {
                    _cameraPosition = value;
                    recalcActualPosition();
                }
            }
        }

        public Coord ActualCameraPosition { get => cameraBounds.Center; }

        private Rectangle cameraBounds;



        public CameraPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height, Map mapToRender)
            : base(rootX, rootY, width, height, false, true)
        {
            _mapToRender = mapToRender;
            cameraBounds = new Rectangle(0, 0, 0, 0);
            // Make sure camera stays centered when window is resized.
            OnResize += (s, e) => recalcActualPosition();
        }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            if (_mapToRender != null)
            {
                // Render terrain
                for (int x = cameraBounds.X; x <= cameraBounds.MaxX; x++)
                    for (int y = cameraBounds.Y; y <= cameraBounds.MaxY; y++)
                    {
                        if (_mapToRender.FOVAt(x, y) > 0.0)    // In FOV; render normally
                            renderGameObject(_mapToRender.Terrain[x, y], x - cameraBounds.X, y - cameraBounds.Y, _mapToRender.Terrain[x, y].Foreground,
                                             _mapToRender.BackgroundColors[x, y]);
                        else if (_mapToRender.IsExplored(x, y)) // Not in FOV but explored; render with grey foreground color, no background
                            renderGameObject(_mapToRender.Terrain[x, y], x - cameraBounds.X, y - cameraBounds.Y, RLColor.Gray, null);

                    }

                // Render everything else in FOV
                foreach (var gObject in _mapToRender.Entities.Items)
                    if (_mapToRender.FOVAt(gObject.Position) > 0.0)
                        renderGameObject(gObject, gObject.Position.X - cameraBounds.X, gObject.Position.Y - cameraBounds.Y, gObject.Foreground,
                                         _mapToRender.BackgroundColors[gObject.Position]);
            }
        }

        private void recalcActualPosition()
        {
            if (_mapToRender != null)
            {
                cameraBounds.MinCorner = Coord.Get(Math.Max(Math.Min(Math.Max(0, _cameraPosition.X - (Width / 2)), _mapToRender.Width - Width), 0),
                                               Math.Max(Math.Min(Math.Max(0, _cameraPosition.Y - (Height / 2)), _mapToRender.Height - Height), 0));

                cameraBounds.Width = Math.Min(_mapToRender.Width, Width);
                cameraBounds.Height = Math.Min(_mapToRender.Height, Height);
            }
        }

        // Renders game object, overriding colors with the ones specified.
        private void renderGameObject(GameObject gObject, int consoleX, int consoleY, RLColor foreColor, RLColor? backColor)
        {
            console.Set(consoleX, consoleY, foreColor, backColor, gObject.Character, (int)gObject.Layer);
        }
    }
}
