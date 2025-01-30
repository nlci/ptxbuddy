using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace AIStudioConnector
{
    public partial class frmController : Form
    {

        private static readonly string apiKey = "AIzaSyBjm79V2oHLvWnO7ElBIvLMb0m5Ne4Tuhk"; // Replace with your Gemini API Key
        private static readonly string apiUrl = "https://aistudio.google.com/live??apiKey=" + apiKey;

        public frmController()
        {
            InitializeComponent();
        }

       
        private async Task<string> CallGeminiAPI(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    prompt = new { text = prompt }
                };

                string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        private async void getButton_Click(object sender, EventArgs e)
        {
            string userInput = "Tell me a joke"; // Example prompt

            //string response = await CallGeminiAPI(userInput);
            //MessageBox.Show(response, "Gemini AI Response");
            string url = apiUrl;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Gemini AI Studio: " + ex.Message);
            }
        }
    }
}
