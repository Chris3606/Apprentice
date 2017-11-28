using RLNET;
using WinMan;
using Apprentice.GameObjects;

namespace Apprentice
{
    // Designed to display a portion of specified map to its console.  Right now it just displays the player bc lazy and no map.
    class CameraPanel : Panel
    {
        public Map MapToRender { get; set; }

        public CameraPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height, Map mapToRender)
            : base(rootX, rootY, width, height, true)
        {
            MapToRender = mapToRender;
        }
        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            // Render terrain
            for (int x = 0; x < MapToRender.Width; x++)
                for (int y = 0; y < MapToRender.Height; y++)
                    renderGameObject(MapToRender.Terrain[x, y]);

            // Render everything else
            foreach (var gObject in MapToRender.Entities.Items)
                renderGameObject(gObject);
        }

        private void renderGameObject(GameObject gObject)
        {
            if (gObject.Character.HasValue)
                console.SetChar(gObject.Position.X, gObject.Position.Y, gObject.Character.Value, (int)gObject.Layer);

            if (gObject.Foreground.HasValue)
                console.SetColor(gObject.Position.X, gObject.Position.Y, gObject.Foreground.Value, (int)gObject.Layer);

            if (gObject.Background.HasValue)
                console.SetBackColor(gObject.Position.X, gObject.Position.Y, gObject.Background.Value, (int)gObject.Layer);
        }
    }
}
