# Aisjam Theme Generator

Unity UI Toolkit (USS/TSS) theme generator. A standalone Unity Package Manager (UPM) package authored by **Aisjam**.

## Install from GitHub

Unity **Window > Package Manager > + > Add package from git URL**:

```
https://github.com/Aisjam/Unity-Theme-Generator.git
```

Or add this to an existing project's `Packages/manifest.json` dependencies:

```json
"com.aisjam.themegenerator": "https://github.com/Aisjam/Unity-Theme-Generator.git"
```

Requires Unity 6 (6000.0+) and UI Toolkit. Unity 6.3+ is recommended. Do not put the package's source files under `Assets/`.

## Usage

1. Open **Aisjam > Style > ThemeGenerator**.
2. Create or select a `ThemeDefinition` ScriptableObject.
3. Choose three identity colours (**Base, Primary, Accent**) and two interaction feedback colours (**Actionable, Blocked**).
4. Assign optional heading and body fonts.
5. Select one of seven style references.
6. Choose an output directory within `Assets/`.
7. Optionally assign a `PanelSettings` asset, and enable **Apply To Panel On Generate**.
8. Click **Generate / Apply Theme**.

The generator writes `AisjamTokens.uss`, `AisjamTypography.uss`, `AisjamComponents.uss`, and `AisjamTheme.tss` to your chosen output folder. Re-running overwrites these generated files; edit the ThemeDefinition instead.

## Seven USS style references

- Flat — crisp flat surfaces and restrained borders
- Glassmorphism — translucent-style surfaces and luminous strokes (true scene blur not supported by USS alone)
- Material-inspired — rounded tonal surfaces and prominent actions
- Neumorphism-inspired — soft surfaces (true soft shadows require extra rendering)
- Brutalist — hard edges and heavy borders
- Industrial — technical squared controls and compact spacing
- Minimal — open spacing and restrained visual detail

All seven presets use the same semantic class names, including `.ais-root`, `.ais-panel`, `.ais-card`, `.ais-button`, `.ais-button--accent`, `.ais-button--actionable`, `.ais-button--blocked`, `.ais-input`, `.ais-heading`, `.ais-label`, and `.ais-shape--pill`.

## Design rules

**Base, Primary, Accent** define the identity. **Actionable** is explicit valid interactive feedback. **Blocked** communicates prohibited/invalid actions; disabled controls remain neutral. Always reinforce feedback with text/icons, and verify contrast in the target UI.

The editor preview shows colours only; full style rendering is visible in UI Builder or Play Mode.

## Status

Version 0.1.0 is an initial implementation and has **not been compiled or tested in Unity**. Verify the USS import and TSS Panel Settings assignment in your target Unity version before production use.

Author: Aisjam.
