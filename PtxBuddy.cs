using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Paratext.PluginInterfaces;

namespace PtxBuddy
{
    [PublicAPI]
    public class PtxBuddy : IParatextWindowPlugin
    {
        public const string pluginName = "Ptxbuddy";

        public string Name => pluginName;

        public Version Version => new Version(2, 0);

        public string VersionString => Version.ToString();

        public string Publisher => "SIL/UBS";

        public IEnumerable<WindowPluginMenuEntry> PluginMenuEntries
        {
            get
            {
                yield return new WindowPluginMenuEntry("&" + pluginName + "...", Run, PluginMenuLocation.ScrTextTools);
            }
        }

        public IDataFileMerger GetMerger(IPluginHost host, string dataIdentifier) => throw new NotImplementedException();

        public string GetDescription(string locale)
        {
            return "Ptxbuddy";
        }

        /// <summary>
        /// Called by Paratext when the menu item created for this plugin was clicked.
        /// </summary>
        private static void Run(IWindowPluginHost host, IParatextChildState windowState)
        {
            host.ShowEmbeddedUi(new FrmPtxbuddy(), windowState.Project);
        }
    }
}