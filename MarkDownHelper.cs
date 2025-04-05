using Markdig;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PtxBuddy
{
   
    public static class MarkdownHelper
    {
        private static readonly MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        public static void AppendMarkdownFormattedText(RichTextBox richTextBox, string markdownText)
        {
            // Convert Markdown to HTML
            string html = Markdig.Markdown.ToHtml(markdownText, pipeline);

            // Convert HTML to RTF (simplified approach)
            string rtf = HtmlToRtf(html);

            // Append to RichTextBox
            richTextBox.SelectedRtf = rtf;
        }

        private static string HtmlToRtf(string html)
        {
            // This is a simplified conversion - you may want to enhance it
            html = html.Replace("<strong>", @"\b ")
                      .Replace("</strong>", @"\b0 ")
                      .Replace("<em>", @"\i ")
                      .Replace("</em>", @"\i0 ")
                      .Replace("<code>", @"\cf1 ")
                      .Replace("</code>", @"\cf0 ")
                      .Replace("<h1>", @"\b\fs32 ")
                      .Replace("</h1>", @"\b0\fs20 ")
                      .Replace("<h2>", @"\b\fs28 ")
                      .Replace("</h2>", @"\b0\fs20 ")
                      .Replace("<ul>", "")
                      .Replace("</ul>", "")
                      .Replace("<li>", "• ")
                      .Replace("</li>", @"\line ")
                      .Replace("<p>", "")
                      .Replace("</p>", @"\line ");

            // Remove any remaining HTML tags
            html = Regex.Replace(html, "<.*?>", string.Empty);

            // Basic RTF header
            string rtfHeader = @"{\rtf1\ansi\deff0{\colortbl;\red0\green0\blue0;\red0\green128\blue0;}";
            string rtfFooter = "}";

            return rtfHeader + html + rtfFooter;
        }
    }
}
