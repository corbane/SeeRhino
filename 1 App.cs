namespace SeeRhino;

using System;
using Rhino;
using Eto.Forms;


public static class See_App
{
    public static Control CreateControl ()
    {
        var c_track = new ToggleButton { Text = "Track RhinoApp.*"};
        c_track.CheckedChanged += (_, _) => Track (c_track.Checked);
        return c_track;
    }

    public static void Track (bool enabled)
    {
        if (enabled)
        {
            SeePanel.Write ("TRACK APP");
            // RhinoApp.AppSettingsChanged += RhinoApp_AppSettingsChanged;
            // RhinoApp.Closing            += RhinoApp_Closing;
            RhinoApp.KeyboardEvent      += RhinoApp_KeyboardEvent;
            // RhinoApp.EscapeKeyPressed   += RhinoApp_EscapeKeyPressed;
            // RhinoApp.RendererChanged    += RhinoApp_RendererChanged;

            // RhinoApp.Initialized
            // RhinoApp.Idle
            // RhinoApp.MainLoop
        }
        else
        {
            SeePanel.Write ("UNTRACK APP");
            RhinoApp.AppSettingsChanged -= RhinoApp_AppSettingsChanged;
            RhinoApp.Closing            -= RhinoApp_Closing;
            RhinoApp.KeyboardEvent      -= RhinoApp_KeyboardEvent;
            RhinoApp.EscapeKeyPressed   -= RhinoApp_EscapeKeyPressed;
            RhinoApp.RendererChanged    -= RhinoApp_RendererChanged;

            // RhinoApp.Initialized
            // RhinoApp.Idle
            // RhinoApp.MainLoop
        }
    }

    static void RhinoApp_AppSettingsChanged (object sender, EventArgs e)
    {
        SeePanel.Write ("App", "RhinoApp_AppSettingsChanged");
    }
    static void RhinoApp_Closing (object sender, EventArgs e)
    {
        SeePanel.Write ("App", "RhinoApp_Closing");
    }
    static void RhinoApp_Initialized (object sender, EventArgs e)
    {
        SeePanel.Write ("App", "RhinoApp_Initialized");
    }
    static void RhinoApp_KeyboardEvent (int key)
    {
        SeePanel.Write ("App", "RhinoApp_KeyboardEvent (" + key + ")");
    }
    static void RhinoApp_EscapeKeyPressed (object sender, EventArgs e)
    {
        SeePanel.Write ("App", "RhinoApp_EscapeKeyPressed");
    }
    static void RhinoApp_RendererChanged (object sender, EventArgs e)
    {
        SeePanel.Write ("App", "RhinoApp_RendererChanged");
    }
}
