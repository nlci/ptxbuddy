using Markdig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PtxBuddy
{

    using Markdig;
    using Markdig.Syntax;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MarkdownChatRenderer
    {
        private readonly RichTextBox _chatBox;
        private readonly Font _defaultFont;
        private readonly Font _boldFont;
        private readonly Font _italicFont;
        private readonly Color _codeBackground = Color.LightGray;

        public MarkdownChatRenderer(RichTextBox chatBox)
        {
            _chatBox = chatBox;
            _defaultFont = new Font("Segoe UI", 10);
            _boldFont = new Font(_defaultFont, FontStyle.Bold);
            _italicFont = new Font(_defaultFont, FontStyle.Italic);
        }

        public void AppendMarkdownMessage(string markdownText, Color bubbleColor)
        {
            // Save current position
            int startPos = _chatBox.TextLength;

            // Append plain text first (without formatting)
            _chatBox.AppendText(markdownText);

            // Parse and apply Markdown formatting
            ApplyMarkdownFormatting(startPos, markdownText.Length, markdownText);


            // Add bubble styling
            _chatBox.Select(startPos, _chatBox.TextLength - startPos);
            _chatBox.SelectionBackColor = bubbleColor;
            _chatBox.DeselectAll();
        }
        private void ApplyUnderline(RichTextBox rtb, int start, int length)
        {
            rtb.Select(start, length);
            string rtf = rtb.SelectedRtf;

            // Modify RTF to include underline
            if (!rtf.Contains(@"\ul"))
            {
                rtf = rtf.Insert(rtf.IndexOf(@"\f0"), @"\ul ");
                if (!rtf.Contains(@"\ulnone"))
                {
                    rtf += @"\ulnone";
                }
            }

            rtb.SelectedRtf = rtf;
            rtb.DeselectAll();
        }

        private void ApplyMarkdownFormatting(int startPos, int length, string markdownText)
        {
            try
            {
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var document = Markdig.Markdown.Parse(markdownText, pipeline);

                // Process both block and inline elements
                foreach (var node in document.Descendants())
                {
                    switch (node)
                    {
                        case Markdig.Syntax.Inlines.LinkInline link:
                            HandleLinkInline(startPos, link);
                            break;

                        case Markdig.Syntax.Inlines.EmphasisInline emphasis:
                            HandleEmphasisInline(startPos, emphasis);
                            break;

                        case Markdig.Syntax.LeafBlock leafBlock when leafBlock.Inline != null:
                            // Process inline content within leaf blocks (paragraphs, headings)
                            foreach (var inline in leafBlock.Inline.Descendants())
                            {
                                if (inline is Markdig.Syntax.Inlines.LinkInline childLink)
                                    HandleLinkInline(startPos, childLink);
                                else if (inline is Markdig.Syntax.Inlines.EmphasisInline childEmphasis)
                                    HandleEmphasisInline(startPos, childEmphasis);
                            }
                            break;

                            // Add other cases as needed
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error applying Markdown formatting: {ex.Message}");
            }
            finally
            {
                _chatBox.DeselectAll();
            }
        }

        private void HandleLinkInline(int startPos, Markdig.Syntax.Inlines.LinkInline link)
        {
            int start = startPos + link.Span.Start;
            int end = startPos + link.Span.End;

            _chatBox.Select(start, end - start);
            _chatBox.SelectionColor = Color.Blue;
            _chatBox.SelectionFont = _boldFont;

            // Apply underline using RTF
            string rtf = _chatBox.SelectedRtf;
            if (!rtf.Contains(@"\ul"))
            {
                rtf = rtf.Insert(rtf.IndexOf(@"\f0"), @"\ul ");
                if (!rtf.Contains(@"\ulnone"))
                {
                    rtf += @"\ulnone";
                }
            }
            _chatBox.SelectedRtf = rtf;
        }

        private void HandleEmphasisInline(int startPos, Markdig.Syntax.Inlines.EmphasisInline emphasis)
        {
            int start = startPos + emphasis.Span.Start;
            int end = startPos + emphasis.Span.End;

            _chatBox.Select(start, end - start);

            if (emphasis.DelimiterChar == '*' || emphasis.DelimiterChar == '_')
            {
                if (emphasis.DelimiterCount == 2)
                {
                    _chatBox.SelectionFont = _boldFont;
                }
                else if (emphasis.DelimiterCount == 1)
                {
                    _chatBox.SelectionFont = _italicFont;
                }
            }
        }
        // Required WinAPI declarations
        private const int WM_USER = 0x0400;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int SCF_SELECTION = 0x0001;
        private const uint CFM_UNDERLINE = 0x00000008;
        private const uint CFE_UNDERLINE = 0x00000008;

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2
        {
            public uint cbSize;
            public uint dwMask;
            public uint dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szFaceName;
            public ushort wWeight;
            public ushort sSpacing;
            public int crBackColor;
            public int lcid;
            public uint dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref CHARFORMAT2 lParam);
    }
}
