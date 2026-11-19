using UnityEngine;

namespace Aisjam.ThemeGenerator
{
    public enum ThemeStyle { Flat, Glassmorphism, Material, Neumorphism, Brutalist, Industrial, Minimal }

    [CreateAssetMenu(fileName = "ThemeDefinition", menuName = "Aisjam/Style/Theme Definition")]
    public sealed class ThemeDefinition : ScriptableObject
    {
        [Header("Visual identity (3 colours)")]
        public Color baseColour = new Color32(32, 38, 51, 255);
        public Color primaryColour = new Color32(86, 140, 207, 255);
        public Color accentColour = new Color32(225, 179, 80, 255);

        [Header("Semantic feedback (2 colours)")]
        public Color actionableColour = new Color32(54, 187, 140, 255);
        public Color blockedColour = new Color32(235, 83, 97, 255);

        [Header("Typography")]
        public Font headingFont;
        public Font bodyFont;

        [Header("Design principle")]
        public ThemeStyle style = ThemeStyle.Flat;

        [Tooltip("Where generated assets are written, inside Assets/.")]
        public string outputFolder = "Assets/Aisjam/GeneratedTheme";
    }
}
