using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aisjam.ThemeGenerator.Editor
{
    public sealed class ThemeGeneratorWindow : EditorWindow
    {
        private ThemeDefinition definition;
        private PanelSettings targetPanel;
        private bool applyToPanel;
        private Vector2 scroll;
        private string status = "Create or select a Theme Definition to begin.";

        [MenuItem("Aisjam/Style/ThemeGenerator")]
        public static void Open() => GetWindow<ThemeGeneratorWindow>("Aisjam Theme Generator");

        private void OnGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("AISJAM / THEME GENERATOR", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Three identity colours + two game feedback colours. Each preset uses the same semantic USS classes.", MessageType.Info);
            definition = (ThemeDefinition)EditorGUILayout.ObjectField("Theme Definition", definition, typeof(ThemeDefinition), false);
            if (GUILayout.Button("Create Theme Definition Asset")) CreateDefinition();
            if (definition == null) { EditorGUILayout.EndScrollView(); return; }

            var so = new SerializedObject(definition);
            so.Update();
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("VISUAL IDENTITY (3)", EditorStyles.boldLabel);
            DrawColor(so, "baseColour", "Base / Surface");
            DrawColor(so, "primaryColour", "Primary / Selection");
            DrawColor(so, "accentColour", "Accent / Focus");
            EditorGUILayout.Space(7);
            EditorGUILayout.LabelField("GAME SEMANTICS (2)", EditorStyles.boldLabel);
            DrawColor(so, "actionableColour", "Actionable / Valid");
            DrawColor(so, "blockedColour", "Blocked / Prohibited");
            EditorGUILayout.HelpBox("Disabled is derived from the base palette, NOT from Blocked. Use icons/text as well as colours for accessibility.", MessageType.None);

            EditorGUILayout.Space(7);
            EditorGUILayout.LabelField("TYPOGRAPHY", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(so.FindProperty("headingFont"), new GUIContent("Heading Font"));
            EditorGUILayout.PropertyField(so.FindProperty("bodyFont"), new GUIContent("Body Font"));
            EditorGUILayout.Space(7);
            EditorGUILayout.LabelField("DESIGN LANGUAGE", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(so.FindProperty("style"), new GUIContent("Style Preset"));
            so.ApplyModifiedProperties();
            EditorGUILayout.HelpBox(StyleSummary(definition.style), MessageType.None);
            EditorGUILayout.Space(7);
            EditorGUILayout.LabelField("OUTPUT", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(so.FindProperty("outputFolder"), new GUIContent("Generate Into"));
            so.ApplyModifiedProperties();

            targetPanel = (PanelSettings)EditorGUILayout.ObjectField("Target Panel Settings", targetPanel, typeof(PanelSettings), false);
            applyToPanel = EditorGUILayout.Toggle("Apply To Panel On Generate", applyToPanel);
            DrawPreview();
            EditorGUILayout.Space(8);
            if (GUILayout.Button("Generate / Apply Theme", GUILayout.Height(36)))
            {
                try
                {
                    status = "Generated " + ThemeGeneratorWriter.Generate(definition, targetPanel, applyToPanel);
                    EditorGUIUtility.PingObject(definition);
                    Debug.Log("[Aisjam Theme Generator] " + status);
                }
                catch (Exception ex)
                {
                    status = "Generation failed: " + ex.Message;
                    Debug.LogException(ex);
                }
            }
            EditorGUILayout.HelpBox(status, MessageType.None);
            EditorGUILayout.EndScrollView();
        }

        private static void DrawColor(SerializedObject so, string key, string label)
        {
            EditorGUILayout.PropertyField(so.FindProperty(key), new GUIContent(label));
        }

        private void CreateDefinition()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create Theme Definition", "AisjamThemeDefinition", "asset", "Choose a location inside Assets.");
            if (string.IsNullOrEmpty(path)) return;
            definition = CreateInstance<ThemeDefinition>();
            AssetDatabase.CreateAsset(definition, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = definition;
        }

        private void DrawPreview()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("PALETTE PREVIEW", EditorStyles.boldLabel);
            Rect strip = GUILayoutUtility.GetRect(10, 52, GUILayout.ExpandWidth(true));
            Color[] colors = { definition.baseColour, definition.primaryColour, definition.accentColour, definition.actionableColour, definition.blockedColour };
            for (int i = 0; i < colors.Length; ++i)
            {
                Rect r = new Rect(strip.x + strip.width * i / colors.Length, strip.y, strip.width / colors.Length + 1, strip.height);
                EditorGUI.DrawRect(r, colors[i]);
            }
            EditorGUILayout.LabelField("BASE             PRIMARY           ACCENT            ACTIONABLE          BLOCKED", EditorStyles.miniLabel);
            Rect area = GUILayoutUtility.GetRect(10, 110, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(area, definition.baseColour);
            float btnWidth = Mathf.Max(60, (area.width - 48) / 3);
            DrawSample(new Rect(area.x + 12, area.y + 33, btnWidth, 40), definition.primaryColour, "PRIMARY");
            DrawSample(new Rect(area.x + 24 + btnWidth, area.y + 33, btnWidth, 40), definition.actionableColour, "VALID");
            DrawSample(new Rect(area.x + 36 + btnWidth * 2, area.y + 33, btnWidth, 40), definition.blockedColour, "BLOCKED");
            EditorGUILayout.HelpBox("This preview represents colours only. View actual shape, typography and hover states in UI Builder or Play Mode.", MessageType.None);
        }

        private static void DrawSample(Rect rect, Color color, string label)
        {
            EditorGUI.DrawRect(rect, color);
            var s = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            s.normal.textColor = ThemeColourRules.ReadableText(color);
            GUI.Label(rect, label, s);
        }

        private static string StyleSummary(ThemeStyle s)
        {
            switch (s)
            {
                case ThemeStyle.Flat: return "Flat: crisp fills, simple rectangles and clear hierarchy.";
                case ThemeStyle.Glassmorphism: return "Glassmorphism: translucent surfaces and luminous strokes (no real background blur in USS alone).";
                case ThemeStyle.Material: return "Material-inspired: tonal surfaces, rounded cards and bold primary actions.";
                case ThemeStyle.Neumorphism: return "Neumorphism-inspired: low-contrast soft surfaces; shadows require extra rendering.";
                case ThemeStyle.Brutalist: return "Brutalist: hard blocks, thick borders and assertive contrast.";
                case ThemeStyle.Industrial: return "Industrial: compact controls, squared edges and technical outlines.";
                case ThemeStyle.Minimal: return "Minimal: airy spacing, quiet dividers and restrained visual emphasis.";
                default: return "";
            }
        }
    }
}
