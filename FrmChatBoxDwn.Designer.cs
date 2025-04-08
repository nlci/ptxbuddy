

using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PtxBuddy
{
    partial class FrmChatBoxDwn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChatBoxDwn));
            this.button1 = new System.Windows.Forms.Button();
            this.cmbProject = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbChapter = new System.Windows.Forms.ComboBox();
            this.cmbVerseFrm = new System.Windows.Forms.ComboBox();
            this.cmbVerseTo = new System.Windows.Forms.ComboBox();
            this.cmbBook = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnagent = new System.Windows.Forms.Button();
            this.btnscrnsht = new System.Windows.Forms.Button();
            this.btnMic = new System.Windows.Forms.Button();
            this.btnAiCommunicator = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.chatArea = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.RichTextBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.timestamp = new System.Windows.Forms.Label();
            this.panelBooks = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.chatArea.SuspendLayout();
            this.panelBooks.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Location = new System.Drawing.Point(514, 40);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "G0->";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbProject
            // 
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(94, 12);
            this.cmbProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(132, 24);
            this.cmbProject.TabIndex = 1;
            this.cmbProject.SelectedIndexChanged += new System.EventHandler(this.cmbProject_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Project Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chapter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Verse From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(387, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Verse To";
            // 
            // cmbChapter
            // 
            this.cmbChapter.FormattingEnabled = true;
            this.cmbChapter.Location = new System.Drawing.Point(94, 39);
            this.cmbChapter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbChapter.Name = "cmbChapter";
            this.cmbChapter.Size = new System.Drawing.Size(132, 24);
            this.cmbChapter.TabIndex = 1;
            this.cmbChapter.SelectedIndexChanged += new System.EventHandler(this.cmbChapter_SelectedIndexChanged);
            // 
            // cmbVerseFrm
            // 
            this.cmbVerseFrm.FormattingEnabled = true;
            this.cmbVerseFrm.Location = new System.Drawing.Point(325, 40);
            this.cmbVerseFrm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbVerseFrm.Name = "cmbVerseFrm";
            this.cmbVerseFrm.Size = new System.Drawing.Size(49, 24);
            this.cmbVerseFrm.TabIndex = 1;
            // 
            // cmbVerseTo
            // 
            this.cmbVerseTo.FormattingEnabled = true;
            this.cmbVerseTo.Location = new System.Drawing.Point(456, 40);
            this.cmbVerseTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbVerseTo.Name = "cmbVerseTo";
            this.cmbVerseTo.Size = new System.Drawing.Size(55, 24);
            this.cmbVerseTo.TabIndex = 1;
            // 
            // cmbBook
            // 
            this.cmbBook.FormattingEnabled = true;
            this.cmbBook.Location = new System.Drawing.Point(326, 12);
            this.cmbBook.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbBook.Name = "cmbBook";
            this.cmbBook.Size = new System.Drawing.Size(229, 24);
            this.cmbBook.TabIndex = 1;
            this.cmbBook.SelectedIndexChanged += new System.EventHandler(this.cmbBook_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Book Name";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 26);
            this.button2.TabIndex = 0;
            this.button2.Text = "Replace With Updated  Text";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPrompt
            // 
            this.txtPrompt.BackColor = System.Drawing.Color.White;
            this.txtPrompt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.txtPrompt.ForeColor = System.Drawing.Color.Black;
            this.txtPrompt.Location = new System.Drawing.Point(88, 19);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(401, 49);
            this.txtPrompt.TabIndex = 5;
            this.txtPrompt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrompt_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnagent);
            this.panel1.Controls.Add(this.btnscrnsht);
            this.panel1.Controls.Add(this.btnMic);
            this.panel1.Controls.Add(this.btnAiCommunicator);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.txtPrompt);
            this.panel1.Location = new System.Drawing.Point(11, 748);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(551, 99);
            this.panel1.TabIndex = 7;
            // 
            // btnagent
            // 
            this.btnagent.BackColor = System.Drawing.Color.SlateGray;
            this.btnagent.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.btnagent.ForeColor = System.Drawing.Color.White;
            this.btnagent.Location = new System.Drawing.Point(352, 69);
            this.btnagent.Name = "btnagent";
            this.btnagent.Size = new System.Drawing.Size(137, 23);
            this.btnagent.TabIndex = 9;
            this.btnagent.Text = "Select an Agent";
            this.btnagent.UseVisualStyleBackColor = false;
            this.btnagent.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnscrnsht
            // 
            this.btnscrnsht.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnscrnsht.BackgroundImage")));
            this.btnscrnsht.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnscrnsht.FlatAppearance.BorderSize = 0;
            this.btnscrnsht.Location = new System.Drawing.Point(2, 19);
            this.btnscrnsht.Name = "btnscrnsht";
            this.btnscrnsht.Size = new System.Drawing.Size(35, 35);
            this.btnscrnsht.TabIndex = 8;
            this.btnscrnsht.UseVisualStyleBackColor = true;
            this.btnscrnsht.Click += new System.EventHandler(this.btnscrnsht_Click);
            // 
            // btnMic
            // 
            this.btnMic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMic.BackgroundImage")));
            this.btnMic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMic.FlatAppearance.BorderSize = 0;
            this.btnMic.Location = new System.Drawing.Point(47, 19);
            this.btnMic.Name = "btnMic";
            this.btnMic.Size = new System.Drawing.Size(35, 35);
            this.btnMic.TabIndex = 7;
            this.btnMic.UseVisualStyleBackColor = true;
            this.btnMic.Click += new System.EventHandler(this.btnMic_Click);
            // 
            // btnAiCommunicator
            // 
            this.btnAiCommunicator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAiCommunicator.BackgroundImage")));
            this.btnAiCommunicator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAiCommunicator.FlatAppearance.BorderSize = 0;
            this.btnAiCommunicator.Location = new System.Drawing.Point(489, 23);
            this.btnAiCommunicator.Name = "btnAiCommunicator";
            this.btnAiCommunicator.Size = new System.Drawing.Size(55, 45);
            this.btnAiCommunicator.TabIndex = 6;
            this.btnAiCommunicator.UseVisualStyleBackColor = true;
            this.btnAiCommunicator.Click += new System.EventHandler(this.btnAiCommunicator_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.SlateGray;
            this.listBox1.Font = new System.Drawing.Font("MV Boli", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.listBox1.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Items.AddRange(new object[] {
            "Back Translation",
            "General Translation"});
            this.listBox1.Location = new System.Drawing.Point(355, 21);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(137, 55);
            this.listBox1.TabIndex = 10;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // chatArea
            // 
            this.chatArea.AutoScroll = true;
            this.chatArea.BackColor = System.Drawing.Color.White;
            this.chatArea.Controls.Add(this.btnRefresh);
            this.chatArea.Location = new System.Drawing.Point(9, 6);
            this.chatArea.Name = "chatArea";
            this.chatArea.Size = new System.Drawing.Size(553, 732);
            this.chatArea.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(37, 29);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(237, 6);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(83, 24);
            this.textBoxResult.TabIndex = 1;
            this.textBoxResult.Text = "";
            this.textBoxResult.Visible = false;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(24, 11);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(44, 16);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "label6";
            // 
            // timestamp
            // 
            this.timestamp.AutoSize = true;
            this.timestamp.Location = new System.Drawing.Point(153, 68);
            this.timestamp.Name = "timestamp";
            this.timestamp.Size = new System.Drawing.Size(44, 16);
            this.timestamp.TabIndex = 2;
            this.timestamp.Text = "label6";
            // 
            // panelBooks
            // 
            this.panelBooks.Controls.Add(this.button1);
            this.panelBooks.Controls.Add(this.textBoxResult);
            this.panelBooks.Controls.Add(this.button2);
            this.panelBooks.Controls.Add(this.cmbProject);
            this.panelBooks.Controls.Add(this.cmbBook);
            this.panelBooks.Controls.Add(this.label4);
            this.panelBooks.Controls.Add(this.cmbChapter);
            this.panelBooks.Controls.Add(this.label3);
            this.panelBooks.Controls.Add(this.cmbVerseFrm);
            this.panelBooks.Controls.Add(this.label2);
            this.panelBooks.Controls.Add(this.cmbVerseTo);
            this.panelBooks.Controls.Add(this.label5);
            this.panelBooks.Controls.Add(this.label1);
            this.panelBooks.Location = new System.Drawing.Point(6, 3);
            this.panelBooks.Name = "panelBooks";
            this.panelBooks.Size = new System.Drawing.Size(566, 76);
            this.panelBooks.TabIndex = 8;
            this.panelBooks.Visible = false;
            // 
            // FrmChatBoxDwn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 850);
            this.Controls.Add(this.chatArea);
            this.Controls.Add(this.panelBooks);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmChatBoxDwn";
            this.Load += new System.EventHandler(this.FrmTestChat_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.chatArea.ResumeLayout(false);
            this.panelBooks.ResumeLayout(false);
            this.panelBooks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbProject;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private System.Windows.Forms.ComboBox cmbChapter;
        private System.Windows.Forms.ComboBox cmbVerseFrm;
        private System.Windows.Forms.ComboBox cmbVerseTo;
        private System.Windows.Forms.ComboBox cmbBook;
        private Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Button btnAiCommunicator;
        private Panel panel1;
        private System.Windows.Forms.Button btnMic;
        private System.Windows.Forms.Button btnscrnsht;
        private ListBox listBox1;
        private System.Windows.Forms.Button btnagent;
        private FlowLayoutPanel chatArea;
        //    private ChatWithAIPlugin.RoundedPanel roundedPanel1;
        private RichTextBox textBoxResult;
        private Label messageLabel;
        private Label timestamp;
        private System.Windows.Forms.Button btnRefresh;
        private Panel panelBooks;
        //    private ChatWithAIPlugin.RoundedPanel chatBubble;
    }
}
