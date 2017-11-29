using RLNET;

namespace Apprentice
{
    static class Extensions
    {
        public static RLColor Multiply(this RLColor color, float multiplier)
        {
            return new RLColor(color.r * multiplier, color.g * multiplier, color.b * multiplier);
        }

        public static RLColor ConvertToGreyscale(this RLColor color)
        {
            float brightness = (color.r + color.g + color.b) / 3.0f;
            return new RLColor(brightness, brightness, brightness);
        }
    }
}
