using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Paratext.PluginInterfaces;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Drawing.Imaging;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Speech.Recognition;
using System.Threading.Tasks;
using System.Globalization;


namespace PtxBuddy
{
    public partial class FrmChatBoxDwn : Form
    {
        #region Member variables
        private IVerseRef m_reference;
        private IProject m_project;
        private Regex m_regexWordExtractor;
        private Thread m_updateThread;
        private IWindowPluginHost m_host;
        private string m_selectedText;

        private SpeechRecognitionEngine recognizer;
        #endregion

        #region Constructor
        private Panel typingIndicatorPanel;
        private Label lblTyping;
        private System.Windows.Forms.Timer typingTimer;
        private int dotCount = 0;

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("PtxbuddyIsEncryptionKeyZyxwvutsr");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("PtxbuddyIsIVDcab");

        int IsCounter = 0;
        string MYGUID;

        private MarkdownChatRenderer _markdownRenderer;

        private string currentImageBase64 = null;

        private static readonly Dictionary<string, string> BookNames = new Dictionary<string, string>
        {
            { "GEN", "Genesis" },
            { "EXO", "Exodus" },
            { "LEV", "Leviticus" },
            { "NUM", "Numbers" },
            { "DEU", "Deuteronomy" },
            { "JOS", "Joshua" },
            { "JDG", "Judges" },
            { "RUT", "Ruth" },
            { "1SA", "1 Samuel" },
            { "2SA", "2 Samuel" },
            { "1KI", "1 Kings" },
            { "2KI", "2 Kings" },
            { "1CH", "1 Chronicles" },
            { "2CH", "2 Chronicles" },
            { "EZR", "Ezra" },
            { "NEH", "Nehemiah" },
            { "EST", "Esther" },
            { "JOB", "Job" },
            { "PSA", "Psalms" },
            { "PRO", "Proverbs" },
            { "ECC", "Ecclesiastes" },
            { "SNG", "Song of Solomon" },
            { "ISA", "Isaiah" },
            { "JER", "Jeremiah" },
            { "LAM", "Lamentations" },
            { "EZK", "Ezekiel" },
            { "DAN", "Daniel" },
            { "HOS", "Hosea" },
            { "JOL", "Joel" },
            { "AMO", "Amos" },
            { "OBA", "Obadiah" },
            { "JON", "Jonah" },
            { "MIC", "Micah" },
            { "NAM", "Nahum" },
            { "HAB", "Habakkuk" },
            { "ZEP", "Zephaniah" },
            { "HAG", "Haggai" },
            { "ZEC", "Zechariah" },
            { "MAL", "Malachi" },
            { "MAT", "Matthew" },
            { "MRK", "Mark" },
            { "LUK", "Luke" },
            { "JHN", "John" },
            { "ACT", "Acts" },
            { "ROM", "Romans" },
            { "1CO", "1 Corinthians" },
            { "2CO", "2 Corinthians" },
            { "GAL", "Galatians" },
            { "EPH", "Ephesians" },
            { "PHP", "Philippians" },
            { "COL", "Colossians" },
            { "1TH", "1 Thessalonians" },
            { "2TH", "2 Thessalonians" },
            { "1TI", "1 Timothy" },
            { "2TI", "2 Timothy" },
            { "TIT", "Titus" },
            { "PHM", "Philemon" },
            { "HEB", "Hebrews" },
            { "JAS", "James" },
            { "1PE", "1 Peter" },
            { "2PE", "2 Peter" },
            { "1JN", "1 John" },
            { "2JN", "2 John" },
            { "3JN", "3 John" },
            { "JUD", "Jude" },
            { "REV", "Revelation" }
        };
        public string UserAction = "";
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // Define RECT structure for GetWindowRect
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region Implementation of EmbeddedPluginControl
        //public override void OnAddedToParent(IPluginChildWindow parent, IWindowPluginHost host, string state)
        //{
        //    m_host = host;
        //    parent.SetTitle(Ptxbuddy.pluginName);

        //    //parent.VerseRefChanged += Parent_VerseRefChanged;
        //    //parent.ProjectChanged += Parent_ProjectChanged;

        //    SetProject(parent.CurrentState.Project);
        //    m_reference = parent.CurrentState.VerseRef;
        //    m_selectedText = state;
        //    //selectedTextToolStripMenuItem.Checked = state != null;
        //}

        private void SetProject(IProject project)
        {
            if (m_project != null)
                m_project.ProjectDeleted -= HandleProjectDeleted;

            m_project = project;
            project.ProjectDeleted += HandleProjectDeleted;
            m_regexWordExtractor = new Regex(m_project.Language.WordMatchRegex, RegexOptions.Compiled);
        }

        private void HandleProjectDeleted()
        {
            m_project = null;
        }

        //public override string GetState()
        //{
        //    return m_selectedText;
        //}

        //public override void DoLoad(IProgressInfo progress)
        //{
        //    //UpdateWordle(new ProgressInfoWrapper(progress));
        //}

        #endregion

        #region Overridden Form methods
        protected override void OnHandleDestroyed(EventArgs e)
        {
            //m_host.ActiveWindowSelectionChanged -= ActiveWindowSelectionChanged;
            base.OnHandleDestroyed(e);
        }
        #endregion




        public FrmChatBoxDwn()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint, true);

            chatArea.FlowDirection = FlowDirection.TopDown;
            chatArea.WrapContents = false;
            chatArea.AutoScroll = true;
            chatArea.AutoSize = false;
            chatArea.Dock = DockStyle.Fill;

            listBox1.Visible = false;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += listBox1_DrawItem;
            btnagent.Text = "Select an Agent";
            _markdownRenderer = new MarkdownChatRenderer(textBoxResult);

            txtPrompt.Text = "Type here...";
            txtPrompt.ForeColor = Color.Gray;

            // Wire up events
            txtPrompt.Enter += RemovePlaceholder;
            txtPrompt.Leave += SetPlaceholder;

            Load_Projects();
            UserAction = "";
            txtPrompt.Focus();
            InitializeTypingIndicator();
            InitializeTimer();
            Resize_Controls();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
            {
                MYGUID = key?.GetValue("MachineGuid")?.ToString();
            }

        }
        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtPrompt.Text == "Type here...")
            {
                txtPrompt.Text = "";
                txtPrompt.ForeColor = Color.Black;
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPrompt.Text))
            {
                txtPrompt.Text = "Type here...";
                txtPrompt.ForeColor = Color.Gray;
            }
        }
        private void Resize_Controls()
        {
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chatArea.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chatArea.SizeChanged += ChatArea_SizeChanged;
            txtPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAiCommunicator.Dock = DockStyle.None;
            btnAiCommunicator.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            //btnAiCommunicator.MaximumSize = new Size(100, 50);
            //btnAiCommunicator.MinimumSize = new Size(55, 45);
            btnAiCommunicator.Location = new Point(
            panel1.Width - btnAiCommunicator.Width - 5,
                (panel1.Height - btnAiCommunicator.Height) / 2
            );

            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button1.MaximumSize = new Size(100, 50);
            button1.MinimumSize = new Size(55, 45);
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button2.MaximumSize = new Size(100, 50);
            button2.MinimumSize = new Size(55, 45);

            btnagent.Dock = DockStyle.None;
            btnagent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnagent.Left = btnagent.Left;

            listBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            listBox1.Left = btnagent.Left;
            listBox1.BringToFront();

        }

        private void InitializeTypingIndicator()
        {
            typingIndicatorPanel = new RoundedPanel();
            //typingIndicatorPanel.CornerRadius = 15;
            typingIndicatorPanel.BackColor = Color.LightGray;
            typingIndicatorPanel.Size = new Size(120, 30);
            typingIndicatorPanel.Visible = false;

            typingIndicatorPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;


            lblTyping = new Label();
            lblTyping.Text = "AI is typing";
            lblTyping.Location = new Point(10, 5);
            lblTyping.AutoSize = true;
            typingIndicatorPanel.Controls.Add(lblTyping);


            chatArea.Controls.Add(typingIndicatorPanel);

            typingIndicatorPanel.Location = new Point(0, chatArea.Height - typingIndicatorPanel.Height);
        }



        private void InitializeTimer()
        {
            typingTimer = new System.Windows.Forms.Timer
            {
                Interval = 500,
                Enabled = false
            };
            typingTimer.Tick += TypingTimer_Tick;
        }
        private void TypingTimer_Tick(object sender, EventArgs e)
        {
            dotCount = (dotCount + 1) % 4;
            string dots = new string('.', dotCount);
            lblTyping.Text = $"AI is typing{dots}";


            typingIndicatorPanel.Location = new Point(
                10,
                chatArea.VerticalScroll.Value + chatArea.Height - typingIndicatorPanel.Height - 10
            );
        }


        private void AddChatBubble(string sender, string message, Color bubbleColor)
        {
            if (chatArea.InvokeRequired)
            {
                chatArea.Invoke(new Action(() => AddChatBubble(sender, message, bubbleColor)));
                return;
            }
            FlowLayoutPanel wrapperPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Top,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            Panel bubble = new Panel
            {
                BackColor = bubbleColor,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(5),
                Padding = new Padding(10),
                MaximumSize = new Size(chatArea.Width - 40, 0)
            };

            RichTextBox messageBox = new RichTextBox
            {
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = bubbleColor,
                Font = new Font("Segoe UI", 10),
                ScrollBars = RichTextBoxScrollBars.None,
                WordWrap = true,
                MaximumSize = new Size(bubble.MaximumSize.Width - 20, 0),
                Width = bubble.MaximumSize.Width - 20,
            };

            messageBox.Text = message;
            ApplyMarkdownFormatting(messageBox, message);

            // Measure height
            messageBox.Height = TextRenderer.MeasureText(messageBox.Text, messageBox.Font,
                new Size(messageBox.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 10;


            Button copyButton = new Button
            {
                Text = "📋",
                Font = new Font("Segoe UI Emoji", 8),
                Size = new Size(30, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = bubbleColor,
                ForeColor = Color.Black,
                Cursor = Cursors.Hand,
                TabStop = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.Click += (s, e) => Clipboard.SetText(messageBox.Text);
            ToolTip tip = new ToolTip();
            tip.SetToolTip(copyButton, "Copy");

            bubble.Resize += (s, e) =>
            {
                copyButton.Location = new Point(
                    bubble.Width - copyButton.Width - 10,
                    5
                );

                messageBox.Width = bubble.Width - 50;
            };


            TableLayoutPanel layout = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.Controls.Add(messageBox, 0, 0);
            layout.Controls.Add(copyButton, 1, 0);

            bubble.Controls.Add(layout);
            wrapperPanel.Controls.Add(bubble);

            chatArea.Controls.Add(wrapperPanel);
            chatArea.ScrollControlIntoView(wrapperPanel);



        }


        private void ChatArea_Resize(object sender, EventArgs e)
        {
            if (chatArea.Controls.Count == 0) return;

            chatArea.SuspendLayout();
            int yPos = 0;

            foreach (Control c in chatArea.Controls)
            {
                if (c is RoundedPanel bubble)
                {
                    bubble.Width = chatArea.ClientSize.Width - 25;

                    if (bubble.Controls.Count > 0 && bubble.Controls[0] is RichTextBox rtb)
                    {
                        using (Graphics g = CreateGraphics())
                        {
                            SizeF size = g.MeasureString(rtb.Text, rtb.Font, bubble.Width - 20);
                            rtb.Height = (int)Math.Ceiling(size.Height) + 10;
                        }
                        bubble.Height = rtb.Height + bubble.Padding.Vertical;
                    }

                    c.Location = new Point(10, yPos);
                    yPos = c.Bottom + 5;
                }
            }

            chatArea.ResumeLayout(true);
            chatArea.PerformLayout();
        }

        private void ApplyMarkdownFormatting(RichTextBox rtb, string markdownText)
        {
            int startPos = 0;

            while ((startPos = rtb.Text.IndexOf("**", startPos)) >= 0)
            {
                int endPos = rtb.Text.IndexOf("**", startPos + 2);
                if (endPos > startPos)
                {
                    // Remove the ** markers
                    rtb.Select(startPos, endPos - startPos + 2);
                    rtb.SelectedText = rtb.Text.Substring(startPos + 2, endPos - startPos - 2);

                    // Apply bold formatting
                    rtb.Select(startPos, endPos - startPos - 2);
                    rtb.SelectionFont = new Font(rtb.Font, FontStyle.Bold);

                    startPos = endPos - 2;
                }
                startPos++;
            }

            rtb.DeselectAll();
            rtb.Refresh();
        }

        private void ChatArea_SizeChanged(object sender, EventArgs e)
        {

            int yPosition = 10;
            foreach (Control control in chatArea.Controls)
            {
                if (control is RoundedPanel chatBubble)
                {
                    chatBubble.Width = chatArea.ClientSize.Width - 5;

                    chatBubble.Location = new Point(0, yPosition);
                    yPosition = chatBubble.Bottom + 10;
                }
            }
        }

        private void Load_Projects()
        {
            string paratextProjectsPath = @"C:\My Paratext 9 Projects";

            if (Directory.Exists(paratextProjectsPath))
            {
                Console.WriteLine("Valid Paratext Projects:");
                string[] projectFolders = Directory.GetDirectories(paratextProjectsPath);
                cmbProject.Items.Clear();
                foreach (string folder in projectFolders)
                {

                    bool hasUsfmFile = Directory.GetFiles(folder, "*.SFM").Length > 0;
                    bool hasSettingsFile = File.Exists(Path.Combine(folder, "Settings.xml"));

                    if (hasUsfmFile && hasSettingsFile)
                    {
                        string projectName = Path.GetFileName(folder);
                        cmbProject.Items.Add(projectName);
                    }
                }
            }
            else
            {
                Console.WriteLine("Paratext projects directory not found.");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                textBoxResult.Text = string.Empty;
                string bookCode = cmbBook.SelectedValue?.ToString();
                string chapter = cmbChapter.SelectedItem?.ToString();
                string ProjectName = cmbProject.SelectedItem?.ToString();

                if (bookCode == null || chapter == null || ProjectName == null)
                {
                    MessageBox.Show("Please give inputs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string projectPath = @"C:\My Paratext 9 Projects\" + ProjectName + "\\";

                string filePath = projectPath + bookCode + ".SFM";
                string sfmContent = File.ReadAllText(filePath);

                // string input = "copy MAT 2:2-5"; 
                int VerseFrom = int.Parse(cmbVerseFrm.SelectedItem.ToString());
                int VerseTo = int.Parse(cmbVerseTo.SelectedItem.ToString());
                string extractedText = ExtractVerses(sfmContent, chapter, VerseFrom, VerseTo);
                //  textBoxResult.Text = extractedText;
                // messageLabel.Text = extractedText;
                AddChatBubble("Me", extractedText, Color.LightBlue);
            }
            catch { }

        }

        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadBooks();
            }
            catch
            {

            }
        }
        static string ExtractVerses(string text, string startChapter, int startVerse, int endVerse)
        {
            //  Chapter 
            string selectedChapter = startChapter.ToString();
            string chapterPattern = @"\\c " + selectedChapter + "\\b";
            Match chapterMatch = Regex.Match(text, chapterPattern);

            if (!chapterMatch.Success) return "Chapter not found.";

            int chapterIndex = chapterMatch.Index;


            int nextChapterIndex = Regex.Match(text.Substring(chapterIndex + 3), @"\\c \d+").Success
                ? text.IndexOf("\\c", chapterIndex + 3)
                : text.Length;

            string chapterText = text.Substring(chapterIndex, nextChapterIndex - chapterIndex);


            string versePattern = @"\\v (\d+) (.*?)((?=\\v \d+)|(?=\\c \d+)|$)";
            MatchCollection verses = Regex.Matches(chapterText, versePattern, RegexOptions.Singleline);

            string result = "Chapter " + selectedChapter + @":\n";
            bool foundVerses = false;

            foreach (Match verse in verses)
            {
                int verseNumber = int.Parse(verse.Groups[1].Value);
                if (verseNumber >= startVerse && verseNumber <= endVerse)
                {
                    result += $"\\v {verse.Groups[1].Value} {verse.Groups[2].Value.Trim()}\n";
                    foundVerses = true;
                }
            }

            return foundVerses ? result.Trim() : "No verses found in this Chapter .";
        }


        private void LoadBooks()
        {
            try
            {
                string ProjectName = cmbProject.SelectedItem.ToString();
                string projectPath = @"C:\My Paratext 9 Projects\" + ProjectName + "\\";
                string bookName = string.Empty;
                string output = string.Empty;
                cmbBook.DataSource = null;
                if (Directory.Exists(projectPath))
                {

                    List<Book> bookData = new List<Book>();

                    var usfmFiles = Directory.GetFiles(projectPath, "*.SFM");

                    var bookCodes = usfmFiles
                        .Select(file => Path.GetFileNameWithoutExtension(file))
                        .OrderBy(book => book);

                    foreach (string bookCode in bookCodes)
                    {

                        output = bookCode.Substring(2, 3);
                        if (BookNames.TryGetValue(output, out bookName))
                        {
                            bookData.Add(new Book { DisplayName = bookName, Code = bookCode });
                        }
                        else
                        {
                            bookData.Add(new Book { DisplayName = bookCode, Code = bookCode });
                        }
                    }
                    cmbBook.DataSource = bookData;
                    cmbBook.DisplayMember = "DisplayName";
                    cmbBook.ValueMember = "Code";

                    if (cmbBook.Items.Count > 0)
                    {
                        cmbBook.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Project directory not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

            }
        }
        private void LoadChapters()
        {

            try
            {
                string ProjectName = cmbProject.SelectedItem.ToString();
                string projectPath = @"C:\My Paratext 9 Projects\" + ProjectName + "\\";

                string bookCode = cmbBook.SelectedValue?.ToString();

                //string code = GetBookCode(BookNames, bookCode);
                if (bookCode == "ChatWithAIPlugin.Book" || bookCode == null)
                {
                    return;
                }

                string usfmFilePath = Path.Combine(projectPath, bookCode + ".SFM");

                if (File.Exists(usfmFilePath))
                {
                    // Read the USFM file
                    string[] lines = File.ReadAllLines(usfmFilePath);

                    // Extract chapter numbers
                    var chapters = lines
                        .Where(line => line.StartsWith("\\c ")) // Find lines with chapter markers
                        .Select(line => int.Parse(line.Substring(3).Trim())) // Extract chapter numbers
                        .Distinct() // Remove duplicates
                        .OrderBy(chapter => chapter); // Sort chapters
                    cmbChapter.Items.Clear();
                    // Add chapters to the ComboBox
                    foreach (int chapter in chapters)
                    {
                        cmbChapter.Items.Add(chapter);
                    }

                    // Select the first chapter by default
                    if (cmbChapter.Items.Count > 0)
                    {
                        cmbChapter.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("USFM file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

            }
        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChapters();
        }

        private void cmbChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVerses();
        }
        private int CountVersesInChapter(string text, int chapterNumber)
        {
            // Locate the chapter (\c X)
            string chapterPattern = $@"\\c {chapterNumber}\b";
            Match chapterMatch = Regex.Match(text, chapterPattern);

            if (!chapterMatch.Success) return 0; // Chapter not found

            int chapterIndex = chapterMatch.Index;

            // Find where the next chapter starts
            int nextChapterIndex = Regex.Match(text.Substring(chapterIndex + 3), @"\\c \d+").Success
                ? text.IndexOf("\\c", chapterIndex + 3)
                : text.Length;

            // Extract the text of the chapter
            string chapterText = text.Substring(chapterIndex, nextChapterIndex - chapterIndex);

            // Count verse markers (\v X)
            MatchCollection verseMatches = Regex.Matches(chapterText, @"\\v (\d+)");

            return verseMatches.Count; // Return the number of verses
        }
        private void LoadVerses()
        {
            string ProjectName = cmbProject.SelectedItem.ToString();
            string projectPath = @"C:\My Paratext 9 Projects\" + ProjectName + "\\";
            string bookCode = cmbBook.SelectedValue?.ToString();
            string filePath = projectPath + bookCode + ".SFM";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("SFM file not found.");
                return;
            }

            if (!int.TryParse(cmbChapter.SelectedItem.ToString(), out int chapterNumber))
            {
                MessageBox.Show("Please enter a valid chapter number.");
                return;
            }

            string sfmContent = File.ReadAllText(filePath);
            int verseCount = CountVersesInChapter(sfmContent, chapterNumber);

            if (verseCount == 0)
            {
                MessageBox.Show($"No verses found in Chapter {chapterNumber}.");
                return;
            }


            cmbVerseFrm.Items.Clear();
            cmbVerseTo.Items.Clear();
            for (int i = 1; i <= verseCount; i++)
            {
                cmbVerseFrm.Items.Add(i);
                cmbVerseTo.Items.Add(i);
                //cmbVerseTo.Items.Add($"Verse {i}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxResult.Text != null)
                {
                    string bookCode = cmbBook.SelectedValue?.ToString();
                    string chapter = cmbChapter.SelectedItem?.ToString();
                    string ProjectName = cmbProject.SelectedItem?.ToString();

                    if (bookCode == null || chapter == null || ProjectName == null)
                    {
                        MessageBox.Show("Please give inputs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string projectPath = @"C:\My Paratext 9 Projects\" + ProjectName + "\\";

                    string filePath = projectPath + bookCode + ".SFM";
                    string sfmContent = File.ReadAllText(filePath);

                    int VerseFrom = int.Parse(cmbVerseFrm.SelectedItem.ToString());
                    int VerseTo = int.Parse(cmbVerseTo.SelectedItem.ToString());

                    // Extract verses 
                    string extractedVerses = textBoxResult.Text;

                    if (extractedVerses.StartsWith("Chapter"))
                    {

                        sfmContent = ReplaceVerses(sfmContent, chapter, VerseFrom, VerseTo, extractedVerses);

                        File.WriteAllText(filePath, sfmContent);

                        MessageBox.Show("Verses replaced successfully..", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Console.WriteLine("Verses replaced successfully.");
                    }
                    else
                    {
                        Console.WriteLine(extractedVerses);
                    }
                }
            }
            catch
            { }


        }
        static string ReplaceVerses(string text, string chapter, int startVerse, int endVerse, string newVerses)
        {

            string chapterPattern = @"\\c " + chapter + "\\b";
            Match chapterMatch = Regex.Match(text, chapterPattern);

            if (!chapterMatch.Success) return text; //

            int chapterIndex = chapterMatch.Index;


            int nextChapterIndex = Regex.Match(text.Substring(chapterIndex + 3), @"\\c \d+").Success
                ? text.IndexOf("\\c", chapterIndex + 3)
                : text.Length;

            string chapterText = text.Substring(chapterIndex, nextChapterIndex - chapterIndex);


            string versePattern = @"\\v (\d+) (.*?)((?=\\v \d+)|(?=\\c \d+)|$)";
            MatchCollection verses = Regex.Matches(chapterText, versePattern, RegexOptions.Singleline);

            int startIndex = -1;
            int endIndex = -1;

            foreach (Match verse in verses)
            {
                int verseNumber = int.Parse(verse.Groups[1].Value);
                if (verseNumber == startVerse && startIndex == -1)
                {
                    startIndex = verse.Index + chapterIndex;
                }
                if (verseNumber == endVerse)
                {
                    endIndex = verse.Index + verse.Length + chapterIndex;
                }
            }

            if (startIndex == -1 || endIndex == -1) return text; // If verses not found, return original text


            string updatedText = text.Substring(0, startIndex) + newVerses + text.Substring(endIndex);

            return updatedText;
        }


        private static readonly HttpClient client = new HttpClient();


        private void AppendUserMessage(string message)
        {
            if (textBoxResult.InvokeRequired)
            {
                textBoxResult.Invoke((Action)(() => AppendUserMessage(message)));
            }
            else
            {

                textBoxResult.AppendText("\n\nMe: ");

                int start = textBoxResult.TextLength;

                textBoxResult.AppendText(message);

                int end = textBoxResult.TextLength;

                textBoxResult.Select(start, end - start);
                textBoxResult.SelectionBackColor = Color.LightBlue;
                textBoxResult.SelectionColor = Color.Black;

                textBoxResult.Select(end, 0);

                textBoxResult.ScrollToCaret();
            }
        }

        private void AppendBotMessage(string message)
        {
            if (textBoxResult.InvokeRequired)
            {
                textBoxResult.Invoke((Action)(() => AppendBotMessage(message)));
            }
            else
            {
                textBoxResult.AppendText("\n\nAI: ");

                int start = textBoxResult.TextLength;

                textBoxResult.AppendText(message);

                int end = textBoxResult.TextLength;

                textBoxResult.Select(start, end - start);
                textBoxResult.SelectionBackColor = Color.LightGray;
                textBoxResult.SelectionColor = Color.Black;

                textBoxResult.Select(end, 0);

                textBoxResult.ScrollToCaret();
            }
        }

        private async void btnAiCommunicator_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            PtxbuddySecurity ptxbuddySecurity = new PtxbuddySecurity();
            string originalGuid = MYGUID;
            string userMessage = txtPrompt.Text;
            int ISlimit = 0;

            string appDirectory = Application.StartupPath;
            string filePathLimit = Path.Combine(appDirectory, "Limit.txt");
            if (File.Exists(filePathLimit))
            {
                ISlimit = 1;
            }
            else
            {
                ISlimit = 0;
            }

            if (ISlimit == 0)
            {
                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
                string filePath = Path.Combine(appDataPath, "encryptedData.txt");
                if (File.Exists(filePath))
                {
                    string ModifiledGuid = PtxbuddySecurity.Decrypt(File.ReadAllText(filePath));

                    ptxbuddySecurity.IsLimitterExists(ModifiledGuid, IsCounter);
                }
                else
                {
                    ptxbuddySecurity.IsLimitterExists(originalGuid, IsCounter);
                }

                IsCounter++;

                if (IsCounter > 3)
                {
                    MessageBox.Show($"Your free limit has been reached");
                    return;
                }
            }
            else if (ISlimit == 1)
            {


                if (!string.IsNullOrEmpty(userMessage))
                {
                    //AppendUserMessage(userMessage);
                    AddChatBubble("Me", userMessage, Color.LightBlue);

                    typingIndicatorPanel.Visible = true;
                    typingTimer.Start();
                    UpdateTypingIndicatorPosition();

                    string webhookUrl = "https://ptxbuddy.app.n8n.cloud/webhook-test/ptxbuddy";
                    // string webhookUrl = "https://jac777thomas.app.n8n.cloud/webhook/3b692d4e-2c9e-4240-b1dd-3baf7e6f5272";

                    // string webhookUrl = "https://jacob777thomas.app.n8n.cloud/webhook/Mywebhook";


                    string apiKey = "pbuddy4P9.4";
                    string headerName = "Ptxbuddy";

                    string queryText = txtPrompt.Text;
                    string sys_prompt = messageLabel.Text;
                    if (UserAction == "")
                    {
                        UserAction = "General Traslation";
                    }
                    var requestData = new
                    {
                        action = UserAction,
                        query = queryText,
                        txt = queryText,
                        //in_language = "Malayalam",
                        //out_language = "English",
                        //Sys_Id = "abcd"

                    };

                    string jsonContent = JsonConvert.SerializeObject(requestData);

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add(headerName, apiKey);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(webhookUrl, content);
                            string result = await response.Content.ReadAsStringAsync();

                            // Parse the JSON response
                            var jsonResponse = JObject.Parse(result);
                            string message = jsonResponse["output"]?.ToString() ?? "No response from AI";

                            // Clean up the response
                            message = message.Replace("\\n", "\n")
                                           .Replace("\\\"", "\"")
                                           .Replace("\\r", "\r")
                                           .Trim('"');

                            // Add to chat area with Markdown formatting
                            AddChatBubble("AI", message, Color.LightGray);
                        }
                    }
                    catch (Exception ex)
                    {
                        AddChatBubble("System", $"Error: {ex.Message}", Color.LightPink);
                    }
                    finally
                    {
                        typingIndicatorPanel.Visible = false;
                        typingTimer.Stop();
                    }
                }
                lblTyping.Text = "AI is typing";
            }

        }
        private void chatArea_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateTypingIndicatorPosition();
        }
        private void UpdateTypingIndicatorPosition()
        {
            if (!typingIndicatorPanel.Visible) return;

            // Position just above the bottom, considering scroll offset
            int y = chatArea.DisplayRectangle.Height > chatArea.ClientSize.Height
                ? chatArea.DisplayRectangle.Bottom - typingIndicatorPanel.Height - 10
                : chatArea.ClientSize.Height - typingIndicatorPanel.Height - 10;

            typingIndicatorPanel.Location = new Point(10, y);
            typingIndicatorPanel.BringToFront();
        }
        private void FrmTestChat_Load(object sender, EventArgs e)
        {
            txtPrompt.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.LightSlateGray, e.Bounds);
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnagent.Text = "Select an Agent";
            if (listBox1.SelectedItem != null)
            {
                string selectedItem = listBox1.SelectedItem.ToString();
                UserAction = selectedItem;
                btnagent.Text = selectedItem;
                listBox1.Visible = false;
                // MessageBox.Show("You selected: " + selectedItem);
            }
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            chatArea.Controls.Clear();
        }

        private void btnscrnsht_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;

                        const int MAX_SIZE_MB = 5;
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length > MAX_SIZE_MB * 1024 * 1024)
                        {
                            MessageBox.Show($"Maximum file size is {MAX_SIZE_MB}MB");
                            return;
                        }

                        using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(filePath))
                        {

                            Bitmap imageCopy = new Bitmap(originalImage);

                            currentImageBase64 = ImageToBase64(imageCopy);

                            AddImageBubble(imageCopy);
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        MessageBox.Show("Invalid image file format");
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("File not found");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                    }
                }
            }
        }
        private string ImageToBase64(System.Drawing.Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Image conversion failed: {ex.Message}");
                return null;
            }
        }
        private void AddImageBubble(Image image)
        {
            try
            {
                // Calculate max dimensions
                int maxWidth = 300;
                int maxHeight = 300;

                // Scale image proportionally
                double ratio = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);
                int scaledWidth = (int)(image.Width * ratio);
                int scaledHeight = (int)(image.Height * ratio);

                // Create picture box with scaled size
                PictureBox pb = new PictureBox
                {
                    Image = new Bitmap(image),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(scaledWidth, scaledHeight),
                    Cursor = Cursors.Hand
                };

                // Wrap in a rounded panel
                RoundedPanel imageBubble = new RoundedPanel
                {
                    BackColor = Color.LightBlue,
                    CornerRadius = 20,
                    Padding = new Padding(5),
                    AutoSize = true
                };
                imageBubble.Controls.Add(pb);

                // Add context menu to remove image
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Remove", null, (s, e) =>
                {
                    chatArea.Controls.Remove(imageBubble);
                    currentImageBase64 = null;
                    pb.Image?.Dispose();
                    imageBubble.Dispose();
                });
                pb.ContextMenuStrip = menu;

                // Position below the last control
                int yPosition = chatArea.Controls.Count > 0
                    ? chatArea.Controls[chatArea.Controls.Count - 1].Bottom + 10
                    : 10;

                imageBubble.Location = new Point(10, yPosition);
                chatArea.Controls.Add(imageBubble);
                chatArea.ScrollControlIntoView(imageBubble);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating image bubble: {ex.Message}");
            }
        }



        private void btnMic_Click(object sender, EventArgs e)
        {
            //    Label listeningLabel = new Label
            //    {
            //        Text = "🎙️ Listening...",
            //        AutoSize = true,
            //        Font = new Font("Segoe UI", 10, FontStyle.Italic),
            //        ForeColor = Color.DarkGray,
            //        BackColor = Color.Transparent,
            //        Location = new Point(btnMic.Left, btnMic.Top - 25), // adjust as needed
            //        Visible = false
            //    };
            //    this.Controls.Add(listeningLabel);
            //    if (recognizer == null)
            //    {
            //        listeningLabel.Visible = true;
            //        recognizer = new SpeechRecognitionEngine();
            //        recognizer.SetInputToDefaultAudioDevice();
            //        recognizer.LoadGrammar(new DictationGrammar());

            //        recognizer.SpeechRecognized += (s, args) =>
            //        {
            //            string spokenText = args.Result.Text;
            //            Invoke(new Action(() =>
            //            {
            //                txtPrompt.Text = spokenText; // or auto-send
            //                txtPrompt.Focus();
            //            }));
            //        };

            //        recognizer.RecognizeCompleted += (s, args) =>
            //        {
            //            recognizer.Dispose();
            //            recognizer = null;
            //        };
            //    }

            //    try
            //    {
            //        recognizer.RecognizeAsync(RecognizeMode.Single);
            //    }
            //    catch (InvalidOperationException)
            //    {
            //        // Already listening
            //    }
            //  //  await Task.Delay(3000); // or after SpeechRecognized event
            //    listeningLabel.Visible = false;
            //}
        }

        public class RoundedPanel : FlowLayoutPanel
        {
            public int CornerRadius { get; set; } = 20;

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                Invalidate(); // Force redraw when resized
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                    path.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                    path.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                    path.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(new SolidBrush(BackColor), path);
                }
            }
        }

    }
}

