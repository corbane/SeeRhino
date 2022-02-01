


using System.Runtime.InteropServices;

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("A1887341-0BC6-44A9-BC29-5A4F6CC48A3C")] // This will also be the Guid of the Rhino plug-in



namespace SeeRhino;

using System;
using System.Linq;

using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.PlugIns;
using Rhino.UI;

using EF = Eto.Forms;


public class See : PlugIn
{
    // some Eto.Control do not have the same height by default.
    internal const int C_HEIGHT = 22;

    internal static RhinoObject[] GetSelectedObjects ()
    {
        var selected = (RhinoDoc.ActiveDoc.Objects.GetSelectedObjects (includeLights: false, includeGrips: false)).ToArray ();
        if (selected.Length == 0)
        {
            SeePanel.Info ("You must preselect one or more objects");
            return selected;
        }
        return selected;
    }
}

public class SeeCommand : Rhino.Commands.Command
{
    public override string EnglishName => "SeeRhino";
    public SeeCommand ()
    {
        Panels.RegisterPanel (PlugIn, typeof (SeePanel), LOC.STR ("See"), null);
    }
    protected override Result RunCommand (RhinoDoc doc, RunMode mode)
    {
        Panels.OpenPanel (typeof (SeePanel).GUID);
        return Result.Success;
    }
}


[System.Runtime.InteropServices.Guid ("7CA00409-EC59-46A7-B8E4-328A8783B4CE")]
public class SeePanel : EF.Panel
{
    static EF.TextArea? s_text;
    // static EF.RichTextArea? s_text;

    public SeePanel ()
    {
        SeePanel.Write ("Panel Constructor");

        if (s_text != null)
            throw new Exception ("Cannot have multiple `See_Panel` instance");

        var c_clear = new EF.Button { Text = "Clear" };
        c_clear.Click += (_, _) => Clear ();

        Content = new EF.StackLayout
        {
            Orientation = EF.Orientation.Vertical,
            HorizontalContentAlignment = EF.HorizontalAlignment.Stretch,
            Items =
            {
                new EF.StackLayoutItem (c_clear),
                // new EF.StackLayoutItem (s_text = new EF.RichTextArea (), expand: true),
                new EF.StackLayoutItem (s_text = new EF.TextArea (), expand: true),
                new EF.StackLayout
                {
                    Orientation = EF.Orientation.Horizontal,
                    Items =
                    {
                        new EF.StackLayoutItem (See_App.CreateControl (), expand: true),
                        new EF.StackLayoutItem (See_Doc.CreateControl (), expand: true)
                    }
                },
                new EF.StackLayoutItem (See_CustomObject.CreateControl ()),
                new EF.StackLayoutItem (See_CustomUserData.CreateControl ()),
            }
        };
    }
    ~SeePanel ()
    {
        SeePanel.Write ("Panel ~Destructor");
        s_text = null;
    }
    protected override void Dispose (bool disposing)
    {
        SeePanel.Write ("Panel Dispose");

        base.Dispose (disposing);
        s_text = null;
    }

    public static void Clear ()
    {
        if (s_text == null)
            return;

        // s_text.Buffer.Clear ();
        s_text.Text = "";
    }
    public static void Info (string? text)
    {
        if (text == null) return;
        text = "-- " + String.Join ("-- ", text.Split ('\n'));
        _Write (text, color: true);
    }
    public static void Write (string? text = null, string? tail = null)
    {
        _Write (text, tail, bold: true);
    }

    // works badly.
    // I think `RhinoApp.WriteLine` focuses on Rhino window and RhinoApp.KeyboardEvent is not fetched.
    //
    // static void _Write (string? text, string? tail = null, bool bold = false, bool color = false)
    // {
    //     if (s_text == null)
    //         return;
    //     
    //     if (tail == null)
    //     {
    //         RhinoApp.WriteLine ((text ?? "") + "\n");
    //     }
    //     else if (text == null)
    //     {
    //         RhinoApp.WriteLine (tail + "\n");
    //     }
    //     else
    //     {
    //         RhinoApp.WriteLine (text + " " + tail + "\n");
    //     }
    // }

    static void _Write (string? text, string? tail = null, bool bold = false, bool color = false)
    {
        if (s_text == null)
            return;
    
        if (tail == null)
        {
            s_text.Append ((text ?? "") + "\n");
        }
        else if (text == null)
        {
            s_text.Append (tail + "\n");
        }
        else
        {
            s_text.Append (text + " " + tail + "\n");
        }
    }

    // static bool s_idle_separator = true;
    // static void _OnIdle (object sender, EventArgs e)
    // {
    //     RhinoApp.Idle -= _OnIdle;
    //     s_idle_separator = true;
    // }
    // static void _Write (string? text, string? tail = null, bool bold = false, bool color = false)
    // {
    //     // !!!
    //     // sometimes there is a thread exception on `var end = s_text.Text.Length`.
    //     // I think it's when the focus changes between a control event and this function.
    //     // !!!
    //     RhinoApp.InvokeOnUiThread (() => {
    //
    //         if (s_text == null)
    //             return;
    //
    //         var end = s_text.Text.Length;
    //
    //         if (s_idle_separator) {
    //             s_idle_separator = false;
    //             s_text.Buffer.Insert (end++, "\n");
    //             RhinoApp.Idle += _OnIdle;
    //         }
    //
    //         if (tail == null)
    //         {
    //             s_text.Buffer.Insert (end, (text ?? "") + "\n");
    //         }
    //         else if (text == null)
    //         {
    //             s_text.Buffer.Insert (end, tail + "\n");
    //         }
    //         else
    //         {
    //             s_text.Buffer.Insert (end, text + " " + tail + "\n");
    //
    //             var range = new EF.Range <int> (end-1, end+text.Length-1);
    //             if (bold)
    //                 s_text.Buffer.SetBold (range, true);
    //             if (color)
    //                 s_text.Buffer.SetForeground (range, ED.Colors.DarkOrange);
    //         }
    //         s_text.CaretIndex = end;
    //
    //     }); /* UiThread */
    // }
}
