using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using mshtml;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.IO;


namespace colorscore
{
    public partial class Form1: Form
    {
		string[] colors = { "transparent", "transparent", "transparent", "transparent", "transparent" };
		string[] additionalColors = { "transparent", "transparent", "transparent", "transparent", "transparent" };
		string page = string.Empty;
		string fileName = string.Empty;
		string fClevSelected = "M25.4063";
		string clefString = string.Empty;
		
		bool clefBrowsing = true;
		public string staffLines { get; private set; }
		public string textFromDocument { get; private set; }
		public string measureNumber { get; private set; }
		public string tieSegment { get; private set; }
		public string barLine { get; private set; }
		public string stem { get; private set; }
		public string hook { get; private set; }
		public string note { get; private set; }
		public string clef { get; private set; }
		public string keySig { get; private set; }
		public string timeSig { get; private set; }
		public string rest { get; private set; }
		public string articulation { get; private set; }
		public string beam { get; private set; }
		public string tempo { get; private set; }
		public string noteDot { get; private set; }
		public string bracket { get; private set; }
		public string ledgerLines { get; private set; }
		public string accidental { get; private set; }

		// Användarens egen färgpalett

		Color[] userPalett = new Color[] {
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent"),
			Color.FromName("transparent")


            // Lägg till fler färger här */
        };
		Color originalColorButton = Color.FromName("transparent");

		Color originalColorButton1 = Color.FromName("transparent");
		Color originalColorButton2 = Color.FromName("transparent");
		Color originalColorButton3 = Color.FromName("transparent");
		Color originalColorButton4 = Color.FromName("transparent");
		Color originalColorButton5 = Color.FromName("transparent");
		Color originalColorButton1b = Color.FromName("transparent");
		Color originalColorButton2b = Color.FromName("transparent");
		Color originalColorButton3b = Color.FromName("transparent");
		Color originalColorButton4b = Color.FromName("transparent");
		Color originalColorButton5b = Color.FromName("transparent");

		
		private string title;
		private WebView2 webView;
		private WebView2 webView2;
		// ...

		public Form1()
		{
			InitializeComponent();
	
			ColorToButton();
			// Load the content of the index.html file into the PreviewWebBrowser
			string indexFilePath = "index.html"; // Adjust the path as needed
			if (System.IO.File.Exists(indexFilePath))
			{
				string indexContent = System.IO.File.ReadAllText(indexFilePath);
				//PreviewWebBrowser.DocumentText = indexContent;
			}
			else
			{
				MessageBox.Show("index.html file not found.");
			}
			InitializeComponentTooltip();
			InitializeWebViewAsync();
			ReadIniFile();
			PopulatePalettPanel();
			menuStrip1.ForeColor = Color.White;
			menuStrip1.BackColor = Color.Black;
			menuStrip1.Margin = new Padding(0, 0, 0, 0);
			GreenPanel.Location = new Point(0, menuStrip1.Height - 3);
			GreenPanel.Width = this.Width;
			GreenPanel.Height = 7;
			PreviewBrowserPanel.Height = this.Height - 80;
			PreviewBrowserPanel.Width = this.Width - PageCreatorPanel.Width - ToolBoxPanel.Width - 30;
			PreviewBrowserPanel.Left = PageCreatorPanel.Width + 5;
			PreviewBrowserPanel.Top = PageCreatorPanel.Top;

			// We must switch to a webview2 control (already installed the package) and set the source to the file name
			//WebView2 webView = new WebView2();
			// Set the source to the file name




			// We must also create a new webview2 control for the clef browsing panel

			//ClefBrowsePanel.Controls.Add(webView2);
			//webView2.BringToFront();



			PageCreatorPanel.Height = this.Height - 80;
			PageCreatorPanel.Left = 5;
			ToolBoxPanel.Height = this.Height - 80;
			ToolBoxPanel.Left = this.Width - ToolBoxPanel.Width - 20;
			ClefPanel.Height = this.Height - 80;
			ClefPanel.Left = this.Width - ToolBoxPanel.Width - ClefPanel.Width - 20;
			ClefBrowsePanel.Dock = DockStyle.Fill;
			GreenPanel.Width = this.Width;
			// Here i want the form be placed in the middle of the screen with 10% margin from the top and left
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2));
			this.ResumeLayout(false);
			this.PerformLayout();
			ClearButton1.Top = ColorButton1.Top + 3;
			ClearButton2.Top = ColorButton2.Top + 3;
			ClearButton3.Top = ColorButton3.Top + 3;
			ClearButton4.Top = ColorButton4.Top + 3;
			ClearButton5.Top = ColorButton5.Top + 3;
			ClearButton1b.Top = ColorButton1b.Top + 3;
			ClearButton2b.Top = ColorButton2b.Top + 3;
			ClearButton3b.Top = ColorButton3b.Top + 3;
			ClearButton4b.Top = ColorButton4b.Top + 3;
			ClearButton5b.Top = ColorButton5b.Top + 3;
			// Set the initial color of the CurrentColorPanel to transparent
			CurrentColorPanel.BackColor = Color.Transparent;
			// Need to add event handler for the ClearButton1 on hover we also must remember the color to the button and set back to original color on mouse leave, the funktion is as follows: when mouse enter we set the color of the colorbutton to black and when mouse leave we set the color of the colorbutton to the original color
			this.ClearButton1.MouseEnter += new System.EventHandler(this.ClearButton1_MouseEnter);
			this.ClearButton1.MouseLeave += new System.EventHandler(this.ClearButton1_MouseLeave);
			this.ClearButton2.MouseEnter += new System.EventHandler(this.ClearButton2_MouseEnter);
			this.ClearButton2.MouseLeave += new System.EventHandler(this.ClearButton2_MouseLeave);
			this.ClearButton3.MouseEnter += new System.EventHandler(this.ClearButton3_MouseEnter);
			this.ClearButton3.MouseLeave += new System.EventHandler(this.ClearButton3_MouseLeave);
			this.ClearButton4.MouseEnter += new System.EventHandler(this.ClearButton4_MouseEnter);
			this.ClearButton4.MouseLeave += new System.EventHandler(this.ClearButton4_MouseLeave);
			this.ClearButton5.MouseEnter += new System.EventHandler(this.ClearButton5_MouseEnter);
			this.ClearButton5.MouseLeave += new System.EventHandler(this.ClearButton5_MouseLeave);
			this.ClearButton1b.MouseEnter += new System.EventHandler(this.ClearButton1b_MouseEnter);
			this.ClearButton1b.MouseLeave += new System.EventHandler(this.ClearButton1b_MouseLeave);
			this.ClearButton2b.MouseEnter += new System.EventHandler(this.ClearButton2b_MouseEnter);
			this.ClearButton2b.MouseLeave += new System.EventHandler(this.ClearButton2b_MouseLeave);
			this.ClearButton3b.MouseEnter += new System.EventHandler(this.ClearButton3b_MouseEnter);
			this.ClearButton3b.MouseLeave += new System.EventHandler(this.ClearButton3b_MouseLeave);
			this.ClearButton4b.MouseEnter += new System.EventHandler(this.ClearButton4b_MouseEnter);
			this.ClearButton4b.MouseLeave += new System.EventHandler(this.ClearButton4b_MouseLeave);
			this.ClearButton5b.MouseEnter += new System.EventHandler(this.ClearButton5b_MouseEnter);
			this.ClearButton5b.MouseLeave += new System.EventHandler(this.ClearButton5b_MouseLeave);
		}


		private async void InitializeWebViewAsync()
		{
			//webView must be initialized before using it
			webView = new WebView2();
			// Set the WebView2 control to fill the entire panel
			webView.Dock = DockStyle.Fill;
			// Add the WebView2 control to the panel
			PreviewBrowserPanel.Controls.Add(webView);
			// Ensure that the WebView2 control is properly initialized.
			await webView.EnsureCoreWebView2Async(null);
			// we need to add current directory to the filepath
			string currentDirectory = Directory.GetCurrentDirectory();
			string indexFilePath = Path.Combine(currentDirectory, "index.html"); // Adjust the path if needed
			//string indexFilePath = "index.html"; // Adjust the path if needed

			// Check if the index.html file exists.
			if (File.Exists(indexFilePath))
			{
				string indexContent = File.ReadAllText(indexFilePath);
				// Load the HTML content into the WebView2 control.
				webView.NavigateToString(indexContent);
			}
			else
			{
				MessageBox.Show("index.html file not found.");
			}

		




		}
		private void PopulatePalettPanel()
		{
			PalettPanel.Controls.Clear();
			// First i want to remove the existing buttons
			// Skapa en panel med färgknappar för användarens egen färgpalett
			int x = 5;
			int y = 5;
			int buttonSize = 30;
			int spacing = 5;
			for (int i = 0; i < userPalett.Length; i++)
			{
				Button button = new Button
				{
					BackColor = userPalett[i],
					Location = new Point(x, y),
					Size = new Size(buttonSize, buttonSize),
					Tag = i
				};
				button.Click += UserColorButton_Click;
				PalettPanel.Controls.Add(button);
				x += buttonSize + spacing;
				if ((i + 1) % 4 == 0)
				{
					x = 5; // Reset x to the initial value
					y += buttonSize + spacing; // Move to the next line
				}
			}
		}

		private void ClearButton1_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton1 = ColorButton1.BackColor;
			// Set the color to black
			ColorButton1.BackColor = Color.Black;
		}

		private void ClearButton1_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton1.BackColor != Color.Transparent)
			{
				ColorButton1.BackColor = originalColorButton1;
			}
		}
		private void ClearButton2_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton2 = ColorButton2.BackColor;
			// Set the color to black
			ColorButton2.BackColor = Color.Black;
		}
		private void ClearButton2_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton2.BackColor != Color.Transparent)
			{
				ColorButton2.BackColor = originalColorButton2;
			}
		}
		private void ClearButton3_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton3 = ColorButton3.BackColor;
			// Set the color to black
			ColorButton3.BackColor = Color.Black;
		}
		private void ClearButton3_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton3.BackColor != Color.Transparent)
			{
				ColorButton3.BackColor = originalColorButton3;
			}
		}
		private void ClearButton4_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton4 = ColorButton4.BackColor;
			// Set the color to black
			ColorButton4.BackColor = Color.Black;
		}
		private void ClearButton4_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton4.BackColor != Color.Transparent)
			{
				ColorButton4.BackColor = originalColorButton4;
			}
		}
		private void ClearButton5_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton5 = ColorButton5.BackColor;
			// Set the color to black
			ColorButton5.BackColor = Color.Black;
		}
		private void ClearButton5_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton5.BackColor != Color.Transparent)
			{
				ColorButton5.BackColor = originalColorButton5;
			}
		}
		private void ClearButton1b_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton1b = ColorButton1b.BackColor;
			// Set the color to black
			ColorButton1b.BackColor = Color.Black;
		}
		private void ClearButton1b_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton1b.BackColor != Color.Transparent)
			{
				ColorButton1b.BackColor = originalColorButton1b;
			}
		}
		private void ClearButton2b_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton2b = ColorButton2b.BackColor;
			// Set the color to black
			ColorButton2b.BackColor = Color.Black;
		}
		private void ClearButton2b_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton2b.BackColor != Color.Transparent)
			{
				ColorButton2b.BackColor = originalColorButton2b;
			}
		}
		private void ClearButton3b_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton3b = ColorButton3b.BackColor;
			// Set the color to black
			ColorButton3b.BackColor = Color.Black;
		}
		private void ClearButton3b_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton3b.BackColor != Color.Transparent)
			{
				ColorButton3b.BackColor = originalColorButton3b;
			}
		}
		private void ClearButton4b_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton4b = ColorButton4b.BackColor;
			// Set the color to black
			ColorButton4b.BackColor = Color.Black;
		}
		private void ClearButton4b_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton4b.BackColor != Color.Transparent)
			{
				ColorButton4b.BackColor = originalColorButton4b;
			}
		}
		private void ClearButton5b_MouseEnter(object sender, EventArgs e)
		{
			// Remember the original color
			originalColorButton5b = ColorButton5b.BackColor;
			// Set the color to black
			ColorButton5b.BackColor = Color.Black;
		}
		private void ClearButton5b_MouseLeave(object sender, EventArgs e)
		{
			// Revert the color back to the original color if it hasn't been set to transparent
			if (ColorButton5b.BackColor != Color.Transparent)
			{
				ColorButton5b.BackColor = originalColorButton5b;
			}
		}

		private void ReadIniFile2()
		{           // Läs innehållet i en INI-fil och skapa en Dictionary med nyckel-värde-par.
			Dictionary<string, string> iniData = new Dictionary<string, string>();
			string iniFilePath = "settings.ini"; // Justera sökvägen om så önskas
			if (System.IO.File.Exists(iniFilePath))
			{
				string[] lines = System.IO.File.ReadAllLines(iniFilePath);
				foreach (string line in lines)
				{
					string[] parts = line.Split('=');
					if (parts.Length == 2)
					{
						string key = parts[0].Trim();
						string value = parts[1].Trim();
						iniData[key] = value;
					}
				}
			}
			else
			{
				MessageBox.Show("settings.ini file not found.");
			}
		}

		private void WriteIniFile()
		{
			// Skapa en INI-fil med nyckel-värde-par.
			Dictionary<string, string> iniData = new Dictionary<string, string>
			{
				{ "Colors", "" },
				{ "Editor", "" }
				
			};
			string iniFilePath = "settings.ini"; // Justera sökvägen om så önskas
			List<string> lines = new List<string>();
			foreach (var pair in iniData)
			{
				lines.Add($"{pair.Key}={pair.Value}");
			}
			System.IO.File.WriteAllLines(iniFilePath, lines);
		}

		private void ColorToButton()
		{
			ColorButton1.BackColor = Color.FromName(colors[0]);
			ColorButton2.BackColor = Color.FromName(colors[1]);
			ColorButton3.BackColor = Color.FromName(colors[2]);
			ColorButton4.BackColor = Color.FromName(colors[3]);
			ColorButton5.BackColor = Color.FromName(colors[4]);
			ColorButton1b.BackColor = Color.FromName(additionalColors[0]);
			ColorButton2b.BackColor = Color.FromName(additionalColors[1]);
			ColorButton3b.BackColor = Color.FromName(additionalColors[2]);
			ColorButton4b.BackColor = Color.FromName(additionalColors[3]);
			ColorButton5b.BackColor = Color.FromName(additionalColors[4]);
		}

		private void Colorize()
		{
			// Läs in SVG-innehållet och skapa ett XDocument.
			string svgContent = System.IO.File.ReadAllText(fileName);
			// Dela upp svgContent i en array med rader (både \n och \r\n hanteras)
			XDocument psvgDoc = XDocument.Parse(svgContent);
			var clefElements = psvgDoc.Descendants()
				.Where(e => e.Name.LocalName == "path" && (string)e.Attribute("class") == "Clef");
			// Here I also want to put the whole row in a varible called "clefString"
			foreach (var element in clefElements)
			{
				clefString += element.ToString() + Environment.NewLine;
				string dValue = (string)element.Attribute("d");
				if (!string.IsNullOrEmpty(dValue) && dValue.Length >= 8)
				{
					string first8 = dValue.Substring(0, 8);
					ClefsRichTextBox.AppendText(first8 + Environment.NewLine);
				
				}
			}



			XDocument svgDoc = XDocument.Parse(svgContent);
		

			// Hitta alla <polyline> med class="StaffLines"
			var staffLineElements = svgDoc.Descendants()
				.Where(e => e.Name.LocalName == "polyline" &&
							(string)e.Attribute("class") == "StaffLines")
				.Select(e => (string)e.Attribute("points"))
				.ToList();

			// Tolka varje polyline – vi antar formatet "x1,y1 x2,y2"
			var staffLines = staffLineElements.Select(p =>
			{
				var pairs = p.Split(' ');
				var firstCoords = pairs[0].Split(',');
				var secondCoords = pairs[1].Split(',');

				double x = double.Parse(firstCoords[0], CultureInfo.InvariantCulture);
				double y = double.Parse(firstCoords[1], CultureInfo.InvariantCulture);
				double x2 = double.Parse(secondCoords[0], CultureInfo.InvariantCulture);
				double width = x2 - x;
				return new { x, y, width };
			})
			.OrderBy(s => s.y)
			.ToList();

			// Gruppindelning baserat på vertikalt avstånd.
			// Vi skapar grupper där gapet mellan stafflines är ungefär detsamma.
			List<List<dynamic>> groups = new List<List<dynamic>>();
			List<dynamic> currentGroup = new List<dynamic>();

			if (staffLines.Any())
			{
				currentGroup.Add(staffLines[0]);
				double currentGroupGap = staffLines.Count > 1 ? staffLines[1].y - staffLines[0].y : 24.8;

				for (int i = 1; i < staffLines.Count; i++)
				{
					double gap = staffLines[i].y - staffLines[i - 1].y;
					if (Math.Abs(gap - currentGroupGap) < 5)
					{
						currentGroup.Add(staffLines[i]);
					}
					else
					{
						groups.Add(currentGroup);
						currentGroup = new List<dynamic>();
						currentGroup.Add(staffLines[i]);
						if (i < staffLines.Count - 1)
							currentGroupGap = staffLines[i + 1].y - staffLines[i].y;
					}
				}
				groups.Add(currentGroup);
			}

			// Sök igenom hela dokumentet efter Clef-element.
			// Identifiera f-klavar genom att kolla på path-data – vi antar att f-klaven
			// har ett "d"-attribut som börjar med "M25.4063".
			
			var fClefElements = svgDoc.Descendants()
				.Where(e => e.Name.LocalName == "path" &&
							(string)e.Attribute("class") == "Clef" &&
							((string)e.Attribute("d")).Contains(fClevSelected))
				.ToList();

			// Skapa en lista med grupper, där vi flaggar om gruppen innehåller en f-klav.
			var groupsWithClef = new List<(List<dynamic> Group, bool IsFClef)>();
			foreach (var group in groups)
			{
				double groupTop = group.First().y;
				double groupBottom = group.Last().y;
				double groupGap = group.Count > 1 ? group[1].y - group[0].y : 24.8;
				double margin = groupGap / 2;
				bool isFClefGroup = false;

				// Kontrollera om något f-klav-element ligger inom gruppens vertikala intervall.
				foreach (var clef in fClefElements)
				{
					string transform = (string)clef.Attribute("transform");
					if (!string.IsNullOrEmpty(transform) && transform.Contains("matrix("))
					{
						// Exempel: matrix(0.992,0,0,0.992,231.198,1874.2)
						string inner = transform.Substring("matrix(".Length).TrimEnd(')');
						var parts = inner.Split(',');
						if (parts.Length == 6)
						{
							double fY = double.Parse(parts[5], CultureInfo.InvariantCulture);
							if (fY >= groupTop - margin && fY <= groupBottom + margin)
							{
								isFClefGroup = true;
								label3.Text = fY.ToString()+" Found!"; 
								break;
							}
						}
					}
				}

				groupsWithClef.Add((group, isFClefGroup));
			}

			// Rita rektanglarna för varje grupp.
			ColorsSpaceRichTextBox.Text += "<g id=\"ColoredSpaces\">" + Environment.NewLine;
			foreach (var groupInfo in groupsWithClef)
			{
				var group = groupInfo.Group;
				bool isFClefGroup = groupInfo.IsFClef;
				double groupGap = group.Count > 1 ? group[1].y - group[0].y : 24.8;
				double x = group[0].x;
				double width = group[0].width;
				double[] positions = new double[5];

				// Bestäm rektangelpositionerna baserat på antalet stafflines.
				if (group.Count == 5)
				{
					for (int i = 0; i < 5; i++)
						positions[i] = group[i].y;
				}
				else if (group.Count == 4)
				{
					positions[0] = group[0].y - groupGap;
					positions[1] = group[0].y;
					positions[2] = group[1].y;
					positions[3] = group[2].y;
					positions[4] = group[3].y;
				}
				else
				{
					int n = Math.Min(group.Count, 5);
					for (int i = 0; i < n; i++)
						positions[i] = group[i].y;
					for (int i = n; i < 5; i++)
						positions[i] = positions[i - 1] + groupGap;
				}

				// Om gruppen är en f-klav grupp, justera rektanglarnas y-position.
				// Lägg till (groupGap x 2) till varje position.
				if (isFClefGroup)
				{
					for (int i = 0; i < positions.Length; i++)
					{
						positions[i] += groupGap;
					}
				}

				// Beräkna extra rektanglarnas positioner.
				double extraHeight = groupGap;
				double[] extraPositions = new double[5];
				extraPositions[0] = positions[0] - (extraHeight / 2);
				for (int i = 1; i < 5; i++)
				{
					extraPositions[i] = positions[i] - (extraHeight / 2);
				}

				// Rita extra rektanglar med en annan färgpalett (additionalColors).
				for (int i = 0; i < 5; i++)
				{
					double rectY = extraPositions[i];
					ColorsSpaceRichTextBox.Text += $"  <rect x=\"{x.ToString(CultureInfo.InvariantCulture)}\" " +
						$"y=\"{rectY.ToString(CultureInfo.InvariantCulture)}\" " +
						$"width=\"{width.ToString(CultureInfo.InvariantCulture)}\" " +
						$"height=\"{extraHeight.ToString(CultureInfo.InvariantCulture)}\" " +
						$"fill=\"{additionalColors[i]}\" />" + Environment.NewLine;
				}

				// Rita de "vanliga" rektanglarna med färgpaletten (colors).
				for (int i = 0; i < 5; i++)
				{
					double rectY = positions[i];
					double rectHeight = (i < 4) ? positions[i + 1] - positions[i] : groupGap;
					ColorsSpaceRichTextBox.Text += $"  <rect x=\"{x.ToString(CultureInfo.InvariantCulture)}\" " +
						$"y=\"{rectY.ToString(CultureInfo.InvariantCulture)}\" " +
						$"width=\"{width.ToString(CultureInfo.InvariantCulture)}\" " +
						$"height=\"{rectHeight.ToString(CultureInfo.InvariantCulture)}\" " +
						$"fill=\"{colors[i]}\" />" + Environment.NewLine;
				}
			}
			ColorsSpaceRichTextBox.Text += "</g>" + Environment.NewLine;
			string browserClefString = HeadRichTextBox.Text + Environment.NewLine + clefString + Environment.NewLine + "</svg>";
			//MessageBox.Show(browserClefString);
			if (browserClefString.StartsWith(Environment.NewLine))
			{
				browserClefString = browserClefString.Substring(Environment.NewLine.Length);
			}
			//MessageBox.Show(browserClefString);

			//Felet var aldrig strängen det är webrowserkontrollen som behöver initieras ordentligt igen är min gissning






			if (clefBrowsing)
			{
				CreateDynamicWebBrowser(browserClefString);
			}
			

			
		}
		// Metod för att skapa en dynamisk webbläsarkontroll och ladda HTML
		private void CreateDynamicWebBrowser(string browserClefString)
		{
			if (clefBrowsing)
			{

				// Skapa en ny instans av WebBrowser
				WebBrowser dynamicBrowser = new WebBrowser();

				// Ställ in egenskaper, t.ex. dockning så att den fyller hela formen eller en container
				dynamicBrowser.Dock = DockStyle.Fill;

				// Lägg till kontrollen på din form (eller i en specifik container, t.ex. en Panel)
				ClefBrowsePanel.Controls.Add(dynamicBrowser);

				// Navigera till en tom sida så att kontrollen är "redo" för nytt innehåll
				dynamicBrowser.DocumentCompleted += (s, e) =>
				{
					// Set the zoom level (100% is 2, 50% is 1, 200% is 4, etc.)
					dynamic axInstance1 = dynamicBrowser.ActiveXInstance;
					object zoomFactor = 50; // 50% zoom level
					axInstance1.ExecWB(OLECMDID.OLECMDID_OPTICAL_ZOOM, OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, zoomFactor, IntPtr.Zero);
				};

				// Hantera DocumentCompleted för att vara säker på att kontrollen är klar innan du laddar in HTML
				dynamicBrowser.DocumentText = browserClefString;
			}
			
			
			
		}

		private void RemoveDynamicWebBrowser()
		{
			// Ta bort WebBrowser-kontrollen från panelen
			foreach (Control control in ClefBrowsePanel.Controls)
			{
				if (control is WebBrowser)
				{
					ClefBrowsePanel.Controls.Remove(control);
					control.Dispose();
					break;
				}
			}
		}
		public void CreatePage(string head, string coloredSpaces,string sheetmusic)
		{
			page = head + Environment.NewLine + coloredSpaces + Environment.NewLine + sheetmusic + Environment.NewLine;
			// HERE :  display page in a WebBrowser control name: "PreviewWebBrowser"
			//PreviewWebBrowser.DocumentText = page;
			// HERE :  display page in a WebView2 control name: "webview"
			if (webView != null && webView.CoreWebView2 != null)
			{
				webView.NavigateToString(page);
			}
			else
			{
				MessageBox.Show("WebView2 is not initialized.");
			}

		}
		public void Save(string head, string coloredSpaces, string sheetmusic)
		{
			
			title = "output";
			
			// Kombinera de tre delarna med radbrytningar.
			string page = head + Environment.NewLine + coloredSpaces + Environment.NewLine + sheetmusic + Environment.NewLine;

			// Ange sökvägen och filnamnet där sidan ska sparas.
			string outputFilePath = fileName + "_new.svg"; // Du kan ändra sökvägen om så önskas.

			// Spara innehållet till filen.
			System.IO.File.WriteAllText(outputFilePath, page);
		}
		

		public void ReadTagsToNames(string xmlContent)
		{
			// Nollställ variablerna
			staffLines = "";
			textFromDocument = "";
			measureNumber = "";
			tieSegment = "";
			barLine = "";
			stem = "";
			hook = "";
			note = "";
			clef = "";
			keySig = "";
			timeSig = "";
			rest = "";
			articulation = "";
			beam = "";
			tempo = "";
			noteDot = "";
			bracket = "";
			ledgerLines = "";
			accidental = "";

			// Ladda XML-dokumentet //comment to english
			// Load the XML document
			XDocument doc = XDocument.Parse(xmlContent);

			// Gå igenom alla element i dokumentet
			// Iterate through all elements in the document
			foreach (var element in doc.Descendants())
			{
				// Hämta värdet på "class"-attributet (om det finns)
				// Get the value of the "class" attribute (if it exists)
				string className = element.Attribute("class")?.Value;
				if (string.IsNullOrEmpty(className))
					continue;

				// Beroende på värdet läggs elementets XML-sträng till i rätt variabel
				// Depending on the value, the element's XML string is added to the correct variable
				switch (className)
				{
					
					case "StaffLines":
						staffLines += element.ToString() + Environment.NewLine;
						break;
					case "Text":
						textFromDocument += element.ToString() + Environment.NewLine;
						break;
					case "MeasureNumber":
						measureNumber += element.ToString() + Environment.NewLine;
						break;
					case "TieSegment":
						tieSegment += element.ToString() + Environment.NewLine;
						break;
					case "BarLine":
						barLine += element.ToString() + Environment.NewLine;
						break;
					case "Stem":
						stem += element.ToString() + Environment.NewLine;
						break;
					case "Hook":
						hook += element.ToString() + Environment.NewLine;
						break;
					case "Note":
						note += element.ToString() + Environment.NewLine;
						break;
					case "Clef":
						clef += element.ToString() + Environment.NewLine;
						break;
					case "KeySig":
						keySig += element.ToString() + Environment.NewLine;
						break;
					case "TimeSig":
						timeSig += element.ToString() + Environment.NewLine;
						break;
					case "Rest":
						rest += element.ToString() + Environment.NewLine;
						break;
					case "Articulation":
						articulation += element.ToString() + Environment.NewLine;
						break;
					case "Beam":
						beam += element.ToString() + Environment.NewLine;
						break;
					case "Tempo":
						tempo += element.ToString() + Environment.NewLine;
						break;
					case "NoteDot":
						noteDot += element.ToString() + Environment.NewLine;
						break;
					case "Bracket":
						bracket += element.ToString() + Environment.NewLine;
						break;
					
					case "LedgerLines":
						ledgerLines += element.ToString() + Environment.NewLine;
						break;

					case "Accidental":
					accidental += element.ToString() + Environment.NewLine;
						break;
					

					default:
					
						// Do nothing

						break;
				}

			}

			// SheetRichTextBox.Text = staffLines + Environment.NewLine + textFromDocument + Environment.NewLine + measureNumber + Environment.NewLine + tieSegment + Environment.NewLine + barLine + Environment.NewLine + stem + Environment.NewLine + hook + Environment.NewLine + note + Environment.NewLine + clef + Environment.NewLine + keySig + Environment.NewLine + timeSig + Environment.NewLine + rest + Environment.NewLine + articulation + Environment.NewLine + beam + Environment.NewLine + tempo + Environment.NewLine + noteDot + Environment.NewLine + bracket + Environment.NewLine + ledgerLines + Environment.NewLine + accidental+Environment.NewLine+"</svg>";
		}

		private void SaveAndApplyMultipleFiles()
		{
			ClefsRichTextBox.Clear();
			ColorsSpaceRichTextBox.Clear();
			var selectedFiles = FilesSelectedRichTextBox.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
			for (int i = 0; i < selectedFiles.Count; i++)
			{
				SubmitProgressBar.Value = (i + 1);
				string selectedFile = selectedFiles[i];
				string directoryPath = System.IO.Path.GetDirectoryName(selectedFile);
				if (selectedFile.Length > 2)
				{
					int randomNum = new Random().Next(1000, 9999);
					string multiFileName = directoryPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + i.ToString() + randomNum.ToString() + ".svg";
					// Read the content of the selected file
					string svgContent = System.IO.File.ReadAllText(selectedFile);
					XDocument svgDoc = XDocument.Parse(svgContent);

					// Manipulate the file
					fileName = selectedFile; // Set the fileName to the current file being processed
					ReadTagsToNames(svgContent); // Read tags to names
					var match = Regex.Match(svgContent, @"^(.*</desc>)", RegexOptions.Singleline);
					string header = "";
					if (match.Success)
					{
						header = match.Groups[1].Value;
					}
					else
					{
						// If <desc> is not found, capture the opening <svg> tag with its attributes
						match = Regex.Match(svgContent, @"^(.*?>)", RegexOptions.Singleline);
						header = match.Success ? match.Groups[1].Value : "";
					}

					string body = svgContent.StartsWith(header) ? svgContent.Substring(header.Length) : svgContent;
					SheetRichTextBox.Text = body;  // Display everything except the header
					HeadRichTextBox.Text = header;
					Colorize();
					CreatePage(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text);

					// Save the manipulated content to a new file with multiFileName
					SaveMultipleFilesName(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text, multiFileName);
					//MessageBox.Show("File saved: " + multiFileName);
					ColorsSpaceRichTextBox.Clear();
				}
			}
			SubmitProgressBar.Value = SubmitProgressBar.Maximum;
		}
		public void SaveMultipleFilesName(string head, string coloredSpaces, string sheetmusic, string mfileName)
		{
			// Combine the three parts with line breaks.
			string page = head + Environment.NewLine + coloredSpaces + Environment.NewLine + sheetmusic + Environment.NewLine;

			// Specify the path and filename where the page should be saved.
			string outputFilePath = mfileName; // Use the provided mfileName

			// Save the content to the file.
			System.IO.File.WriteAllText(outputFilePath, page);
			FilesCreatedRichTextBox.AppendText(outputFilePath + Environment.NewLine);
		}
		private void Apply()
		{
			RemoveDynamicWebBrowser();
			ClefsRichTextBox.Clear();
			if (label1.Text.Length > 2)
			{
				fileName = label1.Text;
				ColorsSpaceRichTextBox.Clear();
				Colorize();
				CreatePage(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Apply();
		}

		private void ColorButton1_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				colors[0] = htmlColor;
				

			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorButton1.BackColor = ColorTranslator.FromHtml(colors[0]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton2_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				colors[1] = htmlColor;

			}
			ColorButton2.BackColor = ColorTranslator.FromHtml(colors[1]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();

		}

		private void ColorButton3_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				colors[2] = htmlColor;

			}
			ColorButton3.BackColor = ColorTranslator.FromHtml(colors[2]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton4_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				colors[3] = htmlColor;

			}
			ColorButton4.BackColor = ColorTranslator.FromHtml(colors[3]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton5_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				colors[4] = htmlColor;

			}
			ColorButton5.BackColor = ColorTranslator.FromHtml(colors[4]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//FilesCreatedRichTextBox.Clear();
			//FilesSelectedRichTextBox.Clear();
			ClefsRichTextBox.Clear();
			clefBrowsing = true;
			clefString = string.Empty;
			colors[0] = "transparent";
			colors[1] = "transparent";
			colors[2] = "transparent";
			colors[3] = "transparent";
			colors[4] = "transparent";
			additionalColors[0] = "transparent";
			additionalColors[1] = "transparent";
			additionalColors[2] = "transparent";
			additionalColors[3] = "transparent";
			additionalColors[4] = "transparent";

			SvgOpenFileDialog.Filter = "SVG files (*.svg)|*.svg"; // Only accept SVG files
			SvgOpenFileDialog.FilterIndex = 1;
			SvgOpenFileDialog.RestoreDirectory = true;

			if (SvgOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				foreach (string fileName in SvgOpenFileDialog.FileNames)
				{
					FilesSelectedRichTextBox.AppendText(fileName + Environment.NewLine);
				}
				SubmitProgressBar.Value = 0;
				SubmitProgressBar.Maximum = FilesSelectedRichTextBox.Lines.Length;
				ApplyMultpleFilesButton.Enabled = true;

				string sheet = System.IO.File.ReadAllText(SvgOpenFileDialog.FileName);
				ReadTagsToNames(sheet);
				var match = Regex.Match(sheet, @"^(.*</desc>)", RegexOptions.Singleline);
				string header = "";
				if (match.Success)
				{
					header = match.Groups[1].Value;
				}
				else
				{
					// Om inte <desc> hittas, fånga istället den öppnande <svg>-taggen med dess attribut
					match = Regex.Match(sheet, @"^(.*?>)", RegexOptions.Singleline);
					header = match.Success ? match.Groups[1].Value : "";
				}

				string body = sheet.StartsWith(header) ? sheet.Substring(header.Length) : sheet;

				SheetRichTextBox.Text = body;  // Här visas allt utom headern
				HeadRichTextBox.Text = header;
				RemoveDynamicWebBrowser();
				fileName = SvgOpenFileDialog.FileName;
				label1.Text = fileName;
				ColorsSpaceRichTextBox.Clear();
				ClefsRichTextBox.Clear();
				Colorize();
				CreatePage(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text);
			}
		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			//PreviewWebBrowser.Height = this.Height - 80;
			//PreviewWebBrowser.Width = this.Width - PageCreatorPanel.Width - ToolBoxPanel.Width;
			PageCreatorPanel.Height = this.Height - 80;
			PageCreatorPanel.Left = 5;
			ToolBoxPanel.Height = this.Height - 80;
			ToolBoxPanel.Left = this.Width - ToolBoxPanel.Width - 20;
			ClefPanel.Height = this.Height - 80;
			ClefPanel.Left = this.Width - ToolBoxPanel.Width - ClefPanel.Width - 20;
			ClefBrowsePanel.Dock = DockStyle.Fill;
			GreenPanel.Width = this.Width;
			PreviewBrowserPanel.Height = this.Height;
			PreviewBrowserPanel.Width = this.Width - PageCreatorPanel.Width - ToolBoxPanel.Width -30;

		}

		private void ColorButton1b_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.CustomColors = customColors;
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				additionalColors[0] = htmlColor;
				

			}
			ColorButton1b.BackColor = ColorTranslator.FromHtml(additionalColors[0]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton2b_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.CustomColors = customColors;
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				additionalColors[1] = htmlColor;	
				

			}
			ColorButton2b.BackColor = ColorTranslator.FromHtml(additionalColors[1]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton3b_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				additionalColors[2] = htmlColor;
				

			}
			ColorButton3b.BackColor = ColorTranslator.FromHtml(additionalColors[2]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton4b_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				additionalColors[3] = htmlColor;
				

			}
			ColorButton4b.BackColor = ColorTranslator.FromHtml(additionalColors[3]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}

		private void ColorButton5b_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.Color = ColorTranslator.FromHtml("lightblue");
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				string htmlColor = ColorTranslator.ToHtml(ColorDialog.Color);
				additionalColors[4] = htmlColor;
				

			}
			ColorButton5b.BackColor = ColorTranslator.FromHtml(additionalColors[4]);
			CurrentColorPanel.BackColor = ColorDialog.Color;
			Apply();
		}



		

void ClearButton1_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton1.BackColor = Color.Transparent;
			colors[0] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton1 = Color.Transparent;
		}

		private void ClearButton2_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton2.BackColor = Color.Transparent;
			colors[1] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton2 = Color.Transparent;
		}

		private void ClearButton3_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton3.BackColor = Color.Transparent;
			colors[2] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton3 = Color.Transparent;
		}

		private void ClearButton4_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton4.BackColor = Color.Transparent;
			colors[3] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton4 = Color.Transparent;
		}

		private void ClearButton5_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton5.BackColor = Color.Transparent;
			colors[4] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton5 = Color.Transparent;
		}

		private void ClearButton1b_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton1b.BackColor = Color.Transparent;
			additionalColors[0] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton1b = Color.Transparent;
		}

		private void ClearButton2b_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton2b.BackColor = Color.Transparent;
			additionalColors[1] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton2b = Color.Transparent;
		}

		private void ClearButton3b_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton3b.BackColor = Color.Transparent;
			additionalColors[2] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton3b = Color.Transparent;
		}

		private void ClearButton4b_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton4b.BackColor = Color.Transparent;
			additionalColors[3] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton4b = Color.Transparent;
		}

		private void ClearButton5b_Click(object sender, EventArgs e)
		{
			// Set the color to transparent
			ColorButton5b.BackColor = Color.Transparent;
			additionalColors[4] = "transparent";
			Apply();
			CurrentColorPanel.BackColor = ColorTranslator.FromHtml("transparent");
			// Update the original color to transparent
			originalColorButton5b = Color.Transparent;
		}

		private void SaveToSingleSvgButton_Click(object sender, EventArgs e)
		{
			Save(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text);
		}

	
		

		private void SaveToMultipleSvgButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "SVG files (*.svg)|*.svg";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = title;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string outputFilePath = saveFileDialog.FileName;
				System.IO.File.WriteAllText(outputFilePath, page);
			}
		}
		private void SaveToMultiplePDFButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = title;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string outputFilePath = saveFileDialog.FileName;
				string svgFilePath = title + ".svg";
				string pdfFilePath = title + ".pdf";
				System.IO.File.WriteAllText(svgFilePath, page);
				Process.Start("cmd.exe", "/C inkscape -f " + svgFilePath + " -A " + pdfFilePath);
			}
		}

		private void SaveToSingleHTMLButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "HTML files (*.html)|*.html";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			 
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string outputFilePath = saveFileDialog.FileName;
				string html = "<!DOCTYPE html>" + Environment.NewLine + "<html>" + Environment.NewLine + "<head>" + Environment.NewLine + "<title>" + title + "</title>" + Environment.NewLine + "</head>" + Environment.NewLine + "<body>" + Environment.NewLine + page + Environment.NewLine + "</body>" + Environment.NewLine + "</html>";
				System.IO.File.WriteAllText(outputFilePath, html);
			}

		}

		private void SaveToSinglePDFButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = title;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string outputFilePath = saveFileDialog.FileName;
				string svgFilePath = label1.Text;
				string pdfFilePath = saveFileDialog.FileName;
				System.IO.File.WriteAllText(svgFilePath, page);
				Process.Start("cmd.exe", "/C inkscape -f " + svgFilePath + " -A " + pdfFilePath);
			}
		}

		private void ClefsRichTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			int charIndex = ClefsRichTextBox.GetCharIndexFromPosition(e.Location);

			// Hitta vilket radnummer tecknet tillhör //comment to english
			// Get the line index from the character index

			int lineIndex = ClefsRichTextBox.GetLineFromCharIndex(charIndex);

			// Hämta hela raden från Lines-arrayen
			// Get the entire line from the Lines array
			string selectedLine = ClefsRichTextBox.Lines[lineIndex];

			// Nu finns den klickade raden i variabeln selectedLine
			// Now the clicked line is in the variable selectedLine

			fClevSelected = selectedLine;
			
			label2.Text = fClevSelected;
			Apply();
		}

		private void SelectFilesButton_Click(object sender, EventArgs e)
		{
			if (SvgOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				// Clear the RichTextBox before adding new filenames
				//FilesSelectedRichTextBox.Clear();

				// Add each selected filename to the RichTextBox
				foreach (string fileName in SvgOpenFileDialog.FileNames)
				{
					FilesSelectedRichTextBox.AppendText(fileName + Environment.NewLine);
				}
			}
			SubmitProgressBar.Value = 0;
			SubmitProgressBar.Maximum = FilesSelectedRichTextBox.Lines.Length;
			ApplyMultpleFilesButton.Enabled = true;
		}

		private void ApplyMultpleFilesButton_Click(object sender, EventArgs e)
		{
			clefBrowsing = false;
			SaveAndApplyMultipleFiles();
			
			MakeHtmlButton.Enabled = true;
		}

		private void showDebugPanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DebugPanel.Visible)
			{
				DebugPanel.Visible = false;
				showDebugPanelToolStripMenuItem.Checked = false;
			}
			else
			{
				DebugPanel.Visible = true;
				DebugPanel.BringToFront();
				showDebugPanelToolStripMenuItem.Checked = true;
			}
		}

		private void WriteIniButton_Click(object sender, EventArgs e)
		{
			WriteIniFile();
		}

		private void ReadIniButton_Click(object sender, EventArgs e)
		{
			
		}

		private void CurrentColorPanel_Click(object sender, EventArgs e)
		{
			// Cast sender to Panel
			Panel clickedPanel = sender as Panel;
			if (clickedPanel != null)
			{
				// Get the background color from the clicked panel
				Color selectedColor = clickedPanel.BackColor;
				//MessageBox.Show($"Selected color: {selectedColor.Name}");
				for (int i = 0; i < userPalett.Length; i++)
				{
					if (userPalett[i] == Color.Transparent)
					{
						userPalett[i] = selectedColor;
						break;
					}
				}

			}

				// Write the updated userPalett array to the INI file
				WriteUserPalettToIniFile();
			    PopulatePalettPanel();

		}

		private void ReadIniFile()
		{
			// Read the INI file
			string iniFilePath = "settings.ini"; // Adjust the path as needed
			if (System.IO.File.Exists(iniFilePath))
			{
				// Read all lines from the INI file
				string[] lines = System.IO.File.ReadAllLines(iniFilePath);
				// Create a dictionary to store the INI data
				Dictionary<string, string> iniData = new Dictionary<string, string>();
				// Parse each line and add it to the dictionary
				foreach (string line in lines)
				{
					string[] parts = line.Split('=');
					if (parts.Length == 2)
					{
						iniData[parts[0]] = parts[1];
					}
				}
				// Read the UserPalett value from the dictionary
				if (iniData.ContainsKey("UserPalett"))
				{
					string userPalettString = iniData["UserPalett"];
					string[] userPalettParts = userPalettString.Split(',');
					for (int i = 0; i < userPalettParts.Length; i++)
					{
						userPalett[i] = ColorTranslator.FromHtml(userPalettParts[i]);
					}
				}
			}
		}

		private void WriteUserPalettToIniFile()
		{
			// Create a dictionary to store the INI data
			Dictionary<string, string> iniData = new Dictionary<string, string>();

			// Convert the userPalett array to a string
			string userPalettString = string.Join(",", userPalett.Select(c => ColorTranslator.ToHtml(c)));

			// Add the userPalett string to the dictionary
			iniData["UserPalett"] = userPalettString;

			// Write the dictionary to the INI file
			string iniFilePath = "settings.ini"; // Adjust the path as needed
			List<string> lines = new List<string>();
			foreach (var pair in iniData)
			{
				lines.Add($"{pair.Key}={pair.Value}");
			}
			System.IO.File.WriteAllLines(iniFilePath, lines);
		}

		private void MakeHtmlButton_Click(object sender, EventArgs e)
		{
			// Create a StringBuilder to build the HTML content
			StringBuilder htmlContent = new StringBuilder();

			// Start the HTML document
			htmlContent.AppendLine("<!DOCTYPE html>");
			htmlContent.AppendLine("<html>");
			htmlContent.AppendLine("<head>");
			htmlContent.AppendLine("<title>SVG Files</title>");
			htmlContent.AppendLine("</head>");
			htmlContent.AppendLine("<body>");

			// Add each SVG file content to the HTML document
			foreach (string filePath in FilesCreatedRichTextBox.Lines)
			{
				if (!string.IsNullOrWhiteSpace(filePath) && System.IO.File.Exists(filePath))
				{
					string svgContent = System.IO.File.ReadAllText(filePath);
					htmlContent.AppendLine(svgContent);
				}
			}

			// End the HTML document
			htmlContent.AppendLine("</body>");
			htmlContent.AppendLine("</html>");

			// Specify the directory path where the HTML file should be saved
			string directoryPath = System.IO.Path.GetDirectoryName(FilesCreatedRichTextBox.Lines.FirstOrDefault());
			string htmlFilePath = directoryPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_output.html";

			System.IO.File.WriteAllText(htmlFilePath, htmlContent.ToString());

			// Optionally, show a message to the user
			MessageBox.Show($"HTML file created: {htmlFilePath}");
			//System.Diagnostics.Process.Start(htmlFilePath); // Use the system's default browser to navigate to the HTML file
															// We must use the webview2
			

			// With the following corrected line:
			webView.Source = new Uri(htmlFilePath);
		}

		private void showClefPanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ClefPanel.Visible)
			{
				ClefPanel.Visible = false;
				showClefPanelToolStripMenuItem.Checked = false;
			}
			else
			{
				ClefPanel.Visible = true;
				ClefPanel.BringToFront();
				showClefPanelToolStripMenuItem.Checked = true;
			}
		}
		private void UserColorButton_Click(object sender, EventArgs e)
		{
			Button clickedButton = sender as Button;
			if (clickedButton != null)
			{
				int buttonIndex = (int)clickedButton.Tag;
				int[] customColors = new int[userPalett.Length];
				for (int i = 0; i < userPalett.Length; i++)
				{
					customColors[i] = ColorTranslator.ToOle(userPalett[i]);
				}
				ColorDialog.CustomColors = customColors;
				if (RemoveColorFromPalettCheckBox.Checked)
				{
					string htmlColor = "transparent";
					userPalett[buttonIndex] = ColorTranslator.FromHtml(htmlColor);
					clickedButton.BackColor = ColorTranslator.FromHtml(htmlColor);
					WriteIniFile();
					PopulatePalettPanel();

				}
				
				
			}
			}
		

		private void CurrentColorPanel_Paint(object sender, PaintEventArgs e)
		{

		}

		private void RemoveColorFromPalettCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void ColorPickerButton_Click(object sender, EventArgs e)
		{
			int[] customColors = new int[userPalett.Length];
			for (int i = 0; i < userPalett.Length; i++)
			{
				customColors[i] = ColorTranslator.ToOle(userPalett[i]);
			}
			ColorDialog.CustomColors = customColors;
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				 
				CurrentColorPanel.BackColor = ColorDialog.Color;
			

			}
		}

		private void NewWorkFlowButton_Click(object sender, EventArgs e)
		{
			// Prompt the user with a choice to start a new workflow
			DialogResult result = MessageBox.Show("Do you want to start a new workflow?", "New Workflow", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				// User chose to start a new workflow
				// we need current directory
				string currentDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			
				
				string indexFilePath = System.IO.Path.Combine(currentDirectory, "index.html");
				if (System.IO.File.Exists(indexFilePath))
				{
					string indexContent = System.IO.File.ReadAllText(indexFilePath);
					//PreviewWebBrowser.DocumentText = indexContent;
					webView.Source = new Uri(indexFilePath);
				}
				else
				{
					MessageBox.Show("index.html file not found.");
				}

				FilesCreatedRichTextBox.Clear();
				FilesSelectedRichTextBox.Clear();
				SubmitProgressBar.Value = 0;
				MakeHtmlButton.Enabled = false;
				ApplyMultpleFilesButton.Enabled = false;
				// here we need to create a method that makes all the colorbuttons color tranparent
				ColorButton1.BackColor = Color.FromName("transparent");
				ColorButton2.BackColor = Color.FromName("transparent");
				ColorButton3.BackColor = Color.FromName("transparent");
				ColorButton4.BackColor = Color.FromName("transparent");
				ColorButton5.BackColor = Color.FromName("transparent");
				ColorButton1b.BackColor = Color.FromName("transparent");
				ColorButton2b.BackColor = Color.FromName("transparent");
				ColorButton3b.BackColor = Color.FromName("transparent");
				ColorButton4b.BackColor = Color.FromName("transparent");
				ColorButton5b.BackColor = Color.FromName("transparent");
				colors[0] = "transparent";
				colors[1] = "transparent";
				colors[2] = "transparent";
				colors[3] = "transparent";
				colors[4] = "transparent";
				additionalColors[0] = "transparent";
				additionalColors[1] = "transparent";
				additionalColors[2] = "transparent";
				additionalColors[3] = "transparent";
				additionalColors[4] = "transparent";
				// Clear the RichTextBoxes
				HeadRichTextBox.Clear();
				ColorsSpaceRichTextBox.Clear();
				SheetRichTextBox.Clear();
				ClefsRichTextBox.Clear();
				// Clear the label
				label1.Text = string.Empty;
				// Clear the ClefPanel
				label2.Text = string.Empty;
				ClefPanel.Visible = false;
				showClefPanelToolStripMenuItem.Checked = false;
				// Clear the DebugPanel
				DebugPanel.Visible = false;
				showDebugPanelToolStripMenuItem.Checked = false;
				// Clear the ClefBrowsing
				clefBrowsing = false;
				clefString = string.Empty;
				// Clear the fileName
				fileName = string.Empty;
				


			}
			else
			{
				// User chose not to start a new workflow
				MessageBox.Show("Workflow continuation selected.");
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save(HeadRichTextBox.Text, ColorsSpaceRichTextBox.Text, SheetRichTextBox.Text);
		}

		private void SvgOpenFileDialog_FileOk(object sender, CancelEventArgs e)
		{

		}

		private ToolTip toolTip = new ToolTip();

		private void CurrentColorPanel_MouseHover(object sender, EventArgs e)
		{
			// Set up the tooltip text for the CurrentColorPanel
			toolTip.SetToolTip(CurrentColorPanel, "Click to add this color to My Colors.");
		}

		private void InitializeComponentTooltip()
		{
			// Other initialization code...

			// Initialize the tooltip
			toolTip.AutoPopDelay = 2000;
			toolTip.InitialDelay = 1000;
			toolTip.ReshowDelay = 500;
			toolTip.ShowAlways = true;

			// Attach the MouseHover event handler to the CurrentColorPanel
			this.CurrentColorPanel.MouseHover += new System.EventHandler(this.CurrentColorPanel_MouseHover);
			// can I add this to the other panels?
			this.FilesSelectedRichTextBox.MouseHover += new System.EventHandler(this.FilesSelectedRichTextBox_MouseHover);
			this.FilesCreatedRichTextBox.MouseHover += new System.EventHandler(this.FilesCreatedRichTextBox_MouseHover);
		}
		private void FilesSelectedRichTextBox_MouseHover(object sender, EventArgs e)
		{
			// Set up the tooltip text for the FilesSelectedRichTextBox
			toolTip.SetToolTip(FilesSelectedRichTextBox, "Selected files will be processed.");
		}
		private void FilesCreatedRichTextBox_MouseHover(object sender, EventArgs e)
		{
			// Set up the tooltip text for the FilesCreatedRichTextBox
			toolTip.SetToolTip(FilesCreatedRichTextBox, "Created files will be listed here.");
		}

		private void RemoveColorsFromSheetButton_Click(object sender, EventArgs e)
		{
			// Define a regular expression to match the <g> tag and its content
			string pattern = @"<g\b[^>]*>(.*?)<\/g>";

			// Use Regex to replace the matched content with an empty string
			SheetRichTextBox.Text = Regex.Replace(SheetRichTextBox.Text, pattern, string.Empty, RegexOptions.Singleline);
			// Optionally, you can also clear the ColorsSpaceRichTextBox if needed
			ColorsSpaceRichTextBox.Clear();
			Apply();
		}

		private void SaveTheRemovedColorsToTheFileButton_Click(object sender, EventArgs e)
		{
			// Define a regular expression to match the <g> tag and its content
			string pattern = @"<g\b[^>]*>(.*?)<\/g>";
			// Use Regex to replace the matched content with an empty string
			string updatedContent = Regex.Replace(SheetRichTextBox.Text, pattern, string.Empty, RegexOptions.Singleline);
			// Update the SheetRichTextBox with the updated content
			SheetRichTextBox.Text = updatedContent;
			// Save the updated content 
			fileName = fileName.Replace(".svg", "_removed_colors.svg");
			//Here i want to replace the text in the selected line in the fileselected rtb with fileName
			int selectedLineIndex = FilesSelectedRichTextBox.GetLineFromCharIndex(FilesSelectedRichTextBox.SelectionStart);
			string[] lines = FilesSelectedRichTextBox.Lines;
			if (selectedLineIndex >= 0 && selectedLineIndex < lines.Length)
			{
				lines[selectedLineIndex] = fileName;
				FilesSelectedRichTextBox.Lines = lines;
			}


			ColorsSpaceRichTextBox.Clear();
			Apply();
		}
	}
	public enum OLECMDID
	{
		OLECMDID_OPTICAL_ZOOM = 63
	}

	public enum OLECMDEXECOPT
	{
		OLECMDEXECOPT_DODEFAULT = 0,
		OLECMDEXECOPT_PROMPTUSER = 1,
		OLECMDEXECOPT_DONTPROMPTUSER = 2,
		OLECMDEXECOPT_SHOWHELP = 3
	}
}
