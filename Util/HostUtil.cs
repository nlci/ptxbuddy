
using AddInSideViews;
using AIStudioConnector;
using System;
using System.Windows.Forms;


namespace PpmPlugin.Util
{
    public class HostUtil
    {
        private static readonly HostUtil _instance = new HostUtil();
        private AiStudioCls _ChatWithAIPlugin;
        private IHost _host;

        public static HostUtil Instance => HostUtil._instance;

        public AiStudioCls ChatWithAIPlugin
        {
            set => this._ChatWithAIPlugin = value;
        }

        public IHost Host
        {
            set => this._host = value;
        }

        public string ParatextVersion => this._host.ApplicationVersion;

        public void ReportError(string prefixText, Exception ex)
        {
            if (prefixText == null && ex == null)
                throw new ArgumentNullException("prefixText (or) ex must be non-null");
            string str1 = prefixText ?? "Error: Please contact support.";
            string str2;
            if (ex != null)
                str2 = Environment.NewLine + Environment.NewLine + "Details: " + ex?.ToString() + Environment.NewLine;
            else
                str2 = string.Empty;
            string text = str1 + str2;
            int num = (int)MessageBox.Show(text, "Notice...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.LogLine("Error: " + text, true);
        }

        public void LogLine(string inputText, bool isError)
        {
            (isError ? Console.Error : Console.Out).WriteLine(inputText);
            this._host?.WriteLineToLog((IParatextAddIn)this._ChatWithAIPlugin, inputText);
        }
    }
}
