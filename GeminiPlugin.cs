using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using AIStudioParatext;
using Microsoft.Extensions.Hosting;
using Paratext.PluginInterfaces;

public class GeminiPlugin : IPlugin
{
    public string Name => "Gemini AIStudio Integration";

    public string Version => "1.0";

    public IEnumerable<PluginAnnotationMenuEntry> PluginAnnotationMenuEntries => throw new NotImplementedException();

    public string VersionString => throw new NotImplementedException();

    public string Publisher => throw new NotImplementedException();

   // Version Paratext.PluginInterfaces.ParatextInternal.IParatextPlugin.Version => throw new NotImplementedException();

    public string GetDescription(string locale)
    {
        throw new NotImplementedException();
    }

    public IDataFileMerger GetMerger(IPluginHost host, string dataIdentifier)
    {
        throw new NotImplementedException();
    }

    public void Start(IHost host)
    {
        // Add a menu item for accessing Gemini AIStudio
        //var menu = host.GetPluginMenu();
        //menu.AddMenuItem("Gemini AIStudio API", OnMenuClicked);
    }

    private void OnMenuClicked(IPluginHost host)
    {
        // Show a dialog to retrieve and use the API key
        GeminiApiForm apiForm = new GeminiApiForm();
        apiForm.ShowDialog();
    }
}

public class PluginAnnotationMenuEntry
{
    public string Name { get; }
    public string Description { get; }
    private object PluginInstance { get; }
    private MethodInfo ActionMethod { get; }

    public PluginAnnotationMenuEntry(string name, string description, object pluginInstance, MethodInfo actionMethod)
    {
        Name = name;
        Description = description;
        PluginInstance = pluginInstance;
        ActionMethod = actionMethod;
    }

    public void Execute()
    {
        if (PluginInstance != null && ActionMethod != null)
        {
            try
            {
                ActionMethod.Invoke(PluginInstance, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing menu action: {ex.Message}");
            }
        }
    }

    public override string ToString()
    {
        return $"MenuEntry: {Name} - {Description}";
    }
}