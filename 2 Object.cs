namespace SeeRhino;


using System.Collections.Generic;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using Eto.Forms;


public class See_CustomObject : Rhino.DocObjects.Custom.CustomMeshObject
{
    public static void Append ()
    {
        RhinoDoc.ActiveDoc.Objects.AddRhinoObject (new See_CustomObject ());
    }

    public static Control CreateControl ()
    {
        var c_append = new Button { Text = "Append custom object" };
        var c_track  = new ToggleButton { Text = "Track" };

        c_append.Click         += (_, _) => See_CustomObject.Append ();
        c_track.CheckedChanged += (_, _) => Tracking = c_track.Checked;
        c_track.MouseUp        += (_, e) => { if (e.Buttons == MouseButtons.Alternate) CreateTrackMenu ().Show (); };

        return new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                new StackLayoutItem (c_append, expand: true),
                new StackLayoutItem (c_track),
            }
        };
    }
    public static ContextMenu CreateTrackMenu ()
    {
        var c_menu = new ContextMenu ();
        var c_livecycle = new CheckMenuItem { Text = "LiveCycle", Checked = TrackLiveCycle };
        var c_selection = new CheckMenuItem { Text = "Selection", Checked = TrackSelection };
        var c_transform = new CheckMenuItem { Text = "Transform", Checked = TrackTransform };
        var c_drawing   = new CheckMenuItem { Text = "Drawing",   Checked = TrackDrawing };

        c_livecycle.CheckedChanged += (_, _) => TrackLiveCycle = c_livecycle.Checked;
        c_selection.CheckedChanged += (_, _) => TrackSelection = c_selection.Checked;
        c_transform.CheckedChanged += (_, _) => TrackTransform = c_transform.Checked;
        c_drawing.CheckedChanged   += (_, _) => TrackDrawing   = c_drawing.Checked;

        c_menu.Items.Add (c_livecycle);
        c_menu.Items.Add (c_selection);
        c_menu.Items.Add (c_transform);
        c_menu.Items.Add (c_drawing);

        return c_menu;
    }

    public static bool Tracking { get; private set; } = false;
    public static bool TrackLiveCycle { get; set; }= true;
    public static bool TrackSelection { get;  set; }= true;
    public static bool TrackTransform { get; set; }= true;
    public static bool TrackDrawing { get; set; } = false;


    #region Live Cycle
    public See_CustomObject () : base (Mesh.CreateIcoSphere (new Sphere (Point3d.Origin, 10), 1))
    {
        if (Tracking && TrackLiveCycle)
            SeeRhino.SeePanel.Write ("Custom object", "Constructor");
    }
    public See_CustomObject (Mesh mesh) : base (mesh)
    {
        if (Tracking && TrackLiveCycle)
            SeeRhino.SeePanel.Write ("Custom object", "Contructor");
    }
    protected override void OnAddToDocument (RhinoDoc doc)
    {
        if (Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom object", "OnAddToDocument");
        base.OnAddToDocument (doc);
    }
    protected override void OnDeleteFromDocument (RhinoDoc doc)
    {
        if (Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom object", "OnDeleteFromDocument");
        base.OnDeleteFromDocument (doc);
    }
    protected override void OnDuplicate (RhinoObject source)
    {
        if (Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom object", "OnDuplicate");
        base.OnDuplicate (source);
    }
    protected override void OnSwitchToNonConst ()
    {
        if (Tracking && TrackLiveCycle) // ???
            SeePanel.Write ("Custom object", "OnSwitchToNonConst");
        base.OnSwitchToNonConst ();
    }
    protected override void Dispose (bool disposing)
    {
        if (Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom object", "Dispose");
        base.Dispose (disposing);
    }
    ~See_CustomObject ()
    {
        if (Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom object", "~Destructor");
    }
    #endregion


    #region Selection
    protected override IEnumerable<ObjRef> OnPick (PickContext context)
    {
        if (Tracking && TrackSelection)
            SeePanel.Write ("Custom object", "OnPick");
        return base.OnPick (context);
    }
    protected override void OnPicked (PickContext context, IEnumerable<ObjRef> pickedItems)
    {
        if (Tracking && TrackSelection)
            SeePanel.Write ("Custom object", "OnPicked");
        base.OnPicked (context, pickedItems);
    }
    protected override void OnSelectionChanged ()
    {
        if (Tracking && TrackSelection)
            SeePanel.Write ("Custom object", "OnSelectionChanged");
        base.OnSelectionChanged ();
    }
    #endregion


    #region Transform
    protected override void OnSpaceMorph (SpaceMorph morph)
    {
        if (Tracking && TrackTransform)
            SeePanel.Write ("Custom object", "OnSpaceMorph");
        base.OnSpaceMorph (morph);
    }
    protected override void OnTransform (Transform transform)
    {
        if (Tracking && TrackTransform)
            SeePanel.Write ("Custom object", "OnTransform");
        base.OnTransform (transform);
    }
    #endregion
    

    #region Draw
    protected override void OnDraw (DrawEventArgs e)
    {
        if (Tracking && TrackDrawing)
            SeePanel.Write ("Custom object", "OnDraw");
        base.OnDraw (e);
    }
    #endregion
}

