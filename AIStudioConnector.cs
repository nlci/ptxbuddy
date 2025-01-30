using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace AIStudioConnector
{
    internal static class AIStudioConnector
    {

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const string MODEL = "gemini-2.0-flash-exp";
        private const string TRANSCRIPTION_MODEL = "gemini-1.5-flash-8b";

        private const int HOTKEY_ID = 1;
        private const uint MOD_CONTROL = 0x2;
        private const uint MOD_ALT = 0x1;
        private const uint VK_G = 0x47;

        private static readonly string apiKey = "AIzaSyBjm79V2oHLvWnO7ElBIvLMb0m5Ne4Tuhk";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmController());
        }
        private class HiddenForm : Form
        {
            public HiddenForm()
            {
                RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_ALT, VK_G);
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_HOTKEY = 0x0312;
                if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
                {
                    Task.Run(() => OpenGeminiScreenShareWithAPIKey());
                }
                base.WndProc(ref m);
            }

            private async Task OpenGeminiScreenShareWithAPIKey()
            {
                string apiUrl = "https://aistudio.google.com/live";
                string urlWithKey = $"{apiUrl}?apiKey={apiKey}";

                // Send an API request if needed (optional)
                string response = await CallGeminiAPI(urlWithKey);

                if (response == "Success")
                {

                    // Open Gemini AI Studio "Share Your Screen"
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlWithKey,
                        UseShellExecute = true
                    });

                    MessageBox.Show("Opening Gemini AI Studio: Share Your Screen", "Gemini AI Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Error: {response}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private async Task<string> CallGeminiAPI(string url)
            {
                try
                {
                     HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    // Example request, adjust as needed for Gemini's API
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return "Success";
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                UnregisterHotKey(this.Handle, HOTKEY_ID);
                base.OnFormClosing(e);
            }
        }
    }
}
