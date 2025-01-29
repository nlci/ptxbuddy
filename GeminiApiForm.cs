using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace AIStudioParatext
{
    public partial class GeminiApiForm : Form
    {

        private TextBox apiKeyTextBox;
        private Button saveButton;
        private const string Placeholder= "Enter your API key here";
        public GeminiApiForm()
        {
            InitializeComponent();
            this.Text = "Gemini AIStudio API Key";

            // TextBox for API Key
            apiKeyTextBox = new TextBox
            {
                Text = Placeholder,
                ForeColor = System.Drawing.Color.Gray,
                Width = 300,
                Top = 20,
                Left = 20
            };

            // Save Button
            saveButton = new Button
            {
                Text = "Save",
                Top = 60,
                Left = 20
            };
            saveButton.Click += SaveButton_Click;

            // Add controls to the form
            this.Controls.Add(apiKeyTextBox);
            this.Controls.Add(saveButton);

            // Load the saved API key, if it exists
            LoadApiKey();
        }

         private void SaveButton_Click(object sender, EventArgs e)
    {
        string apiKey = apiKeyTextBox.Text;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            MessageBox.Show("Please enter a valid API key.");
            return;
        }

        // Save the API key securely (e.g., encrypted storage or file)
        SaveApiKey(apiKey);

        MessageBox.Show("API Key saved successfully.");
        this.Close();
    }

    private void SaveApiKey(string apiKey)
    {
        // Save API key securely (use encryption in production)
        File.WriteAllText("api_key.txt", apiKey);
    }

    private void LoadApiKey()
    {
        // Load API key from file if it exists
        if (File.Exists("api_key.txt"))
        {
            apiKeyTextBox.Text = File.ReadAllText("api_key.txt");
        }
    }

        private void GeminiApiForm_Load(object sender, EventArgs e)
        {

        }
    }
}
