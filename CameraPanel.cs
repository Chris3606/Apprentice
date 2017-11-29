using GoRogue;
using RLNET;
using WinMan;
using Apprentice.GameObjects;
using Apprentice.Maps;
using System;

namespace Apprentice
{
    // Designed to display a portion of specified map to its console.
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
                        renderGameObject(_mapToRender.Terrain[x, y], x - cameraBounds.X, y - cameraBounds.Y);

                // Render everything else
                foreach (var gObject in _mapToRender.Entities.Items)
                    renderGameObject(gObject, gObject.Position.X - cameraBounds.X, gObject.Position.Y - cameraBounds.Y);
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

        private void renderGameObject(GameObject gObject, int consoleX, int consoleY)
        {
            if (gObject.Character.HasValue)
                console.SetChar(consoleX, consoleY, gObject.Character.Value, (int)gObject.Layer);

            if (gObject.Foreground.HasValue)
                console.SetColor(consoleX, consoleY, gObject.Foreground.Value, (int)gObject.Layer);

            if (gObject.Background.HasValue)
                console.SetBackColor(consoleX, consoleY, gObject.Background.Value, (int)gObject.Layer);
        }
    }
}
