# ColorScore

**ColorScore** is a simple, open-source Windows application that adds colors to SVG sheet music files. It does not modify the musical content — it just helps you visualize different musical elements (like staves or clefs) by colorizing the spaces between the lines. This can be helpful for educational purposes, visual grouping, or accessibility.

## 🎯 Features

- Load one or multiple `.svg` music score files
- Automatically detects staff groups and clef positions
- Add colored rectangles between staff lines
- Custom color palette for both regular and extra layers
- Save result as `.svg`, `.html`, or `.pdf` (requires Inkscape for PDF)
- Tooltips and user-friendly UI with visual feedback

## 🧪 What it does exactly

The program:
1. Parses the SVG structure of music notation (produced e.g. by Audiveris or similar software)
2. Identifies groups of staff lines
3. Checks for clef types (e.g. F clef) using path data
4. Inserts colored `<rect>` elements between the staff lines
5. Outputs the result as a modified SVG (or HTML/PDF)

No internet connection is required. No data is collected. Everything runs locally.

## 💾 How to use it

1. Open the application
2. Click `File -> Open` and select one or more `.svg` files
3. Choose colors using the color buttons
4. Click **Apply** to preview
5. Save your modified file via the "Save as" buttons

## 🔧 Requirements

- Windows 10 or later
- .NET Framework (tested on 4.8+)
- Optional: [Inkscape](https://inkscape.org) (for converting SVG to PDF)

## ⚠️ Windows Defender Warning

Since this is a small unsigned open source project, Windows might warn you when launching the app. This is **normal** for unsigned software. See below how to run it safely:

> **"Windows protected your PC"**  
> Click **More Info** → **Run anyway**

If needed:
- Right-click the `.exe` → **Properties** → check **"Unblock"**
- Run as adminstrator to be able to save files on your computer.

You can also [build it yourself](#build-instructions) from source.

## 🛠 Build instructions

1. Open the `ColorScore.sln` in Visual Studio
2. Make sure `index.html` and other required resources are in the output folder
3. Build and run

## 🗂 File overview

- `Form1.cs`: The main logic for UI and SVG manipulation
- `index.html`: Shown on startup
- `settings.ini`: Stores user-defined color palette

## 🌐 License

MIT – free to use, modify, and distribute. Contributions are welcome!

---

**Created by a musician for musicians. Enjoy and color your scores!**
