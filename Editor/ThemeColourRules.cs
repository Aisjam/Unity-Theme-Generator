using UnityEngine;

namespace Aisjam.ThemeGenerator.Editor
{
    internal static class ThemeColourRules
    {
        internal static Color Mix(Color a, Color b, float t) => Color.Lerp(a, b, Mathf.Clamp01(t));
        internal static float Luminance(Color c)
        {
            Color l = c.linear;
            return 0.2126f * l.r + 0.7152f * l.g + 0.0722f * l.b;
        }
        internal static float Contrast(Color a, Color b)
        {
            float x = Luminance(a), y = Luminance(b);
            return (Mathf.Max(x, y) + 0.05f) / (Mathf.Min(x, y) + 0.05f);
        }
        internal static Color ReadableText(Color c) => Contrast(c, Color.white) >= Contrast(c, Color.black) ? Color.white : Color.black;
        internal static Color Raise(Color c, float amount) => Mix(c, Luminance(c) < 0.24f ? Color.white : Color.black, amount);
        internal static string Hex(Color c)
        {
            Color32 b = c;
            return $"#{b.r:X2}{b.g:X2}{b.b:X2}";
        }
    }
}
