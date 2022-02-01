namespace SeeRhino;

using Rhino.DocObjects.Custom;
using Rhino.FileIO;
using Rhino.Geometry;

using Eto.Forms;


[System.Runtime.InteropServices.Guid ("045DCF80-C3D3-49AC-843D-BD11D16F6C71")]
public class See_CustomUserData : Rhino.DocObjects.Custom.UserData
{
    public enum AttachedTo { RhinoObject, Geometry, Attributes }
    public static void AppendTo (AttachedTo to)
    {
        switch (to)
        {
        case AttachedTo.RhinoObject:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null)
                    obj.UserData.Add (new See_CustomUserData ("toRhinoObject"));
            break;

        case AttachedTo.Geometry:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null)
                    obj.Geometry.UserData.Add (new See_CustomUserData ("ToGeometry"));
            break;

        case AttachedTo.Attributes:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null)
                    obj.Attributes.UserData.Add (new See_CustomUserData ("ToAttributes"));
            break;
        }
    }
    public static void RemoveTo (AttachedTo to)
    {
        var t = typeof (See_CustomUserData);
        switch (to)
        {
        case AttachedTo.RhinoObject:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null && obj.UserData.Find (t) is See_CustomUserData udata)
                    obj.UserData.Remove (udata);
            break;

        case AttachedTo.Geometry:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null && obj.Geometry.UserData.Find (t) is See_CustomUserData udata)
                    obj.Geometry.UserData.Remove (udata);
            break;

        case AttachedTo.Attributes:

            foreach (var obj in See.GetSelectedObjects ())
                if (obj != null && obj.Attributes.UserData.Find (t) is See_CustomUserData udata)
                    obj.Attributes.UserData.Remove (udata);
            break;
        }
    }

    public static Control CreateControl ()
    {
        var c_udata__append = new Button { Text = "Append" };
        var c_udata__remove = new Button { Text = "Remove" };
        var c_udata__to     = new EnumDropDown <See_CustomUserData.AttachedTo> { SelectedValue = See_CustomUserData.AttachedTo.Attributes };
        var c_udata__track  = new ToggleButton { Text = "Track" };

        c_udata__append.Click += (_, _) => AppendTo (c_udata__to.SelectedValue);
        c_udata__remove.Click += (_, _) => RemoveTo (c_udata__to.SelectedValue);
        c_udata__track.CheckedChanged += (_, _) => Tracking = c_udata__track.Checked;
        c_udata__track.MouseUp += (_, e) => { if (e.Buttons == MouseButtons.Alternate) CreateTrackMenu ().Show (); };

        c_udata__to.Height = c_udata__append.Height = c_udata__remove.Height = c_udata__track.Height = See.C_HEIGHT;

        return new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                new StackLayoutItem (c_udata__append, expand: true),
                new StackLayoutItem (c_udata__remove, expand: true),
                new StackLayoutItem (new Label { Text = " UserData to "}),
                new StackLayoutItem (c_udata__to, expand: true),
                new StackLayoutItem (c_udata__track),
            }
        };

    }
    public static ContextMenu CreateTrackMenu ()
    {
        var c_menu = new ContextMenu ();
        var c_livecycle = new CheckMenuItem { Text = "LiveCycle", Checked = See_CustomUserData.TrackLiveCycle };
        var c_transform = new CheckMenuItem { Text = "Transorm",  Checked = See_CustomUserData.TrackTransform };

        c_livecycle.Click += (_, e) => TrackLiveCycle = c_livecycle.Checked;
        c_transform.Click += (_, e) => TrackTransform = c_transform.Checked;

        c_menu.Items.Add (c_livecycle);
        c_menu.Items.Add (c_transform);

        return c_menu;
    }

    public static bool Tracking { get; private set; } = false;
    public static bool TrackLiveCycle { get; set; } = true;
    public static bool TrackTransform { get; set; } = true;


    #region
    public string To;
    public override string Description
    {
        get
        {
            SeePanel.Write ("Custom UserData", "Description");
            return nameof (See_CustomUserData);
        }
    }
    #endregion


    #region Live Cycle
    public See_CustomUserData ()
    {
        if(Tracking && TrackLiveCycle)
            SeeRhino.SeePanel.Write ("Custom UserData", "Required constructor");
        To = "IN REQUIRED CONSTRUCTOR";
    }
    public See_CustomUserData (string parent)
    {
        if(Tracking && TrackLiveCycle)
            SeeRhino.SeePanel.Write ("Custom UserData", "Constructor (" + parent + ")");
        To = parent;
    }
    protected override void Dispose (bool disposing)
    {
        if(Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom UserData", "Dispose");
        base.Dispose (disposing);
    }
    protected override void OnDuplicate (UserData source)
    {
        if(Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom UserData", "OnDuplicate");
        base.OnDuplicate (source);
    }
    public override bool ShouldWrite
    {
        get
        {
            if(Tracking && TrackLiveCycle)
                SeePanel.Write ("Custom UserData", "ShouldWrite");
            return true;
        }
    }
    protected override bool Read (BinaryArchiveReader archive)
    {
        if(Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom UserData", "Read");
        To = archive.ReadUtf8String ();
        return true;
    }
    protected override bool Write (BinaryArchiveWriter archive)
    {
        if(Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom UserData", "Write");
        archive.WriteUtf8String (To);
        return true;
    }
    ~See_CustomUserData ()
    {
        if(Tracking && TrackLiveCycle)
            SeePanel.Write ("Custom UserData", "~Destructor");
    }
    #endregion


    #region Transform
    protected override void OnTransform (Transform transform)
    {
        if(Tracking && TrackTransform)
            SeeRhino.SeePanel.Write ("Custom UserData", "OnTransform");
        base.OnTransform (transform);
    }
    #endregion
}
