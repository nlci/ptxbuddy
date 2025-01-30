using AddInSideViews;
using Microsoft.Win32;
using PpmPlugin;
using PpmPlugin.Util;
using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AIStudioConnector
{
    [System.AddIn.AddIn("Chat with AI", Description = "Plugin for integrating AI Studio with Paratext, enabling seamless interaction and AI-powered assistance within the application.", Version = "1.3.1.0", Publisher = "NLCI, Inc.")]
    [QualificationData("MenuText", "Chat with AI")]
    [QualificationData("InsertAfterMenuName", "Tools|")]
    [QualificationData("EnableWhen", "Always")]
    [QualificationData("MultipleInstances", "Always")]
    public class AiStudioCls : IParatextAddIn2, IParatextAddIn
    {
        private const string PpmRegistryRoot = "HKEY_CURRENT_USER";
        private const string PpmRegistryBaseKey = "HKEY_CURRENT_USER\\SOFTWARE\\NLCI\\ChatWithAI";
        private const string PpmAppInstallDirKey = "CWAIAppInstallDir";
        private const string PpmAppNotInstalledMessage = "Paratext Plugin Manager is not installed.";

        public Dictionary<string, IPluginDataFileMergeInfo> DataFileKeySpecifications
        {
            get => (Dictionary<string, IPluginDataFileMergeInfo>)null;
        }

        public void Activate(string activeProjectName)
        {
        }

        public void RequestShutdown() => Environment.Exit(0);

        public virtual DialogResult ShowMessageBox(
          string messageText,
          MessageBoxButtons messageButtons,
          MessageBoxIcon messageIcon)
        {
            return MessageBox.Show(messageText, "Notice...", messageButtons, messageIcon);
        }
        public void Run(IHost host, string activeProjectName)
        {
            lock (this)
            {
                HostUtil.Instance.Host = host;
                HostUtil.Instance.ChatWithAIPlugin = this;
                try
                {
                    Application.EnableVisualStyles();
                    Thread thread = new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            object obj = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\NLCI\\ChatWithAI", "CWAIAppInstallDir", (object)null);
                            if (obj == null)
                                throw new Exception("AIStudio Plugin is not installed.");
                            HostUtil.Instance.LogLine("Paratext running... Application name: '" + host.ApplicationName + "', version: '" + host.ApplicationVersion + "'.", false);
                            Process.Start(string.Format("{0}\\AIStudioConnector.exe", obj));
                        }
                        catch (Exception ex)
                        {
                            AiStudioCls.ReportErrorWithDetails(ex.Message, (IDictionary<string, string>)new Dictionary<string, string>());
                        }
                        finally
                        {
                            Environment.Exit(0);
                        }
                    }))
                    {
                        IsBackground = false
                    };
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                catch (Exception ex)
                {
                    HostUtil.Instance.ReportError((string)null, ex);
                    throw;
                }
            }
        }

        public static void ReportErrorWithDetails(
          string message,
          IDictionary<string, string> details = null,
          bool printException = false,
          Exception ex = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (printException && ex == null)
                throw new ArgumentNullException(nameof(ex));
            StringBuilder stringBuilder = new StringBuilder(message + "\r\n");
            if (details != null)
            {
                foreach (KeyValuePair<string, string> detail in (IEnumerable<KeyValuePair<string, string>>)details)
                    stringBuilder.AppendLine("    " + detail.Key + ": " + detail.Value);
            }
            if (printException)
                HostUtil.Instance.ReportError(stringBuilder.ToString(), ex);
            else
                HostUtil.Instance.ReportError(stringBuilder.ToString(), (Exception)null);
        }
    }
}
