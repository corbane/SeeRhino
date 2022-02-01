namespace SeeRhino;

using Rhino;
using Rhino.DocObjects;
using Eto.Forms;


public static class See_Doc
{
    public static Control CreateControl ()
    {
        var c_track = new ToggleButton { Text = "Track RhinoDoc.*" };
        c_track.CheckedChanged += (_, _) => Track (c_track.Checked);
        c_track.MouseUp        += (_, e) => { if (e.Buttons == MouseButtons.Alternate) CreateContextMenu ().Show (); };
        return c_track;
    }
    public static ContextMenu CreateContextMenu ()
    {
        var c_menu = new ContextMenu ();
        var c_documents  = new CheckMenuItem { Text = "Documents" };
        var c_object     = new CheckMenuItem { Text = "Object" };
        var c_selections = new CheckMenuItem { Text = "Selections" };
        var c_tables     = new CheckMenuItem { Text = "Tables" };

        c_documents.CheckedChanged  += (_, _) => TrackDocuments = c_documents.Checked;
        c_object.CheckedChanged     += (_, _) => TrackObjects = c_object.Checked;
        c_selections.CheckedChanged += (_, _) => TrackSelections = c_selections.Checked;
        c_tables.CheckedChanged     += (_, _) => TrackTables = c_tables.Checked;

        return new ContextMenu (c_documents, c_object, c_selections, c_tables);
    }

    public static bool IsEnabled { get; private set; } = false;
    public static bool TrackDocuments { get; set; }
    public static bool TrackObjects { get; set; }
    public static bool TrackSelections { get; set; }
    public static bool TrackTables { get; set; }
    public static void Track (bool enabled)
    {
        if (enabled == IsEnabled) return;
        IsEnabled = enabled;
        if (enabled)
        {
            // Documents
            RhinoDoc.CloseDocument                    += RhinoDoc_CloseDocument;
            RhinoDoc.NewDocument                      += RhinoDoc_NewDocument;
            RhinoDoc.ActiveDocumentChanged            += RhinoDoc_ActiveDocumentChanged;
            RhinoDoc.DocumentPropertiesChanged        += RhinoDoc_DocumentPropertiesChanged;
            RhinoDoc.BeginOpenDocument                += RhinoDoc_BeginOpenDocument;
            RhinoDoc.EndOpenDocument                  += RhinoDoc_EndOpenDocument;
            RhinoDoc.EndOpenDocumentInitialViewUpdate += RhinoDoc_EndOpenDocumentInitialViewUpdate;
            RhinoDoc.BeginSaveDocument                += RhinoDoc_BeginSaveDocument;
            RhinoDoc.EndSaveDocument                  += RhinoDoc_EndSaveDocument;

            // Objects
            RhinoDoc.UserStringChanged                += RhinoDoc_UserStringChanged;
            RhinoDoc.AddRhinoObject                   += RhinoDoc_AddRhinoObject;
            RhinoDoc.DeleteRhinoObject                += RhinoDoc_DeleteRhinoObject;
            RhinoDoc.ReplaceRhinoObject               += RhinoDoc_ReplaceRhinoObject;
            RhinoDoc.UndeleteRhinoObject              += RhinoDoc_UndeleteRhinoObject;
            RhinoDoc.PurgeRhinoObject                 += RhinoDoc_PurgeRhinoObject;
            RhinoDoc.ModifyObjectAttributes           += RhinoDoc_ModifyObjectAttributes;
            RhinoDoc.BeforeTransformObjects           += RhinoDoc_BeforeTransformObjects;

            // Selections
            RhinoDoc.SelectObjects                    += RhinoDoc_SelectObjects;
            RhinoDoc.DeselectObjects                  += RhinoDoc_DeselectObjects;
            RhinoDoc.DeselectAllObjects               += RhinoDoc_DeselectAllObjects;

            // Tables
            RhinoDoc.LayerTableEvent                  += RhinoDoc_LayerTableEvent;
            RhinoDoc.DimensionStyleTableEvent         += RhinoDoc_DimensionStyleTableEvent;
            RhinoDoc.InstanceDefinitionTableEvent     += RhinoDoc_InstanceDefinitionTableEvent;
            RhinoDoc.LightTableEvent                  += RhinoDoc_LightTableEvent;
            RhinoDoc.MaterialTableEvent               += RhinoDoc_MaterialTableEvent;
            RhinoDoc.GroupTableEvent                  += RhinoDoc_GroupTableEvent;
            RhinoDoc.RenderMaterialsTableEvent        += RhinoDoc_RenderMaterialsTableEvent;
            RhinoDoc.RenderEnvironmentTableEvent      += RhinoDoc_RenderEnvironmentTableEvent;
            RhinoDoc.RenderTextureTableEvent          += RhinoDoc_RenderTextureTableEvent;
            // ??? TrackObjects or TrackTables ??
            RhinoDoc.TextureMappingEvent              += RhinoDoc_TextureMappingEvent;
        }
        else
        {
            RhinoDoc.CloseDocument                    -= RhinoDoc_CloseDocument;
            RhinoDoc.NewDocument                      -= RhinoDoc_NewDocument;
            RhinoDoc.ActiveDocumentChanged            -= RhinoDoc_ActiveDocumentChanged;
            RhinoDoc.DocumentPropertiesChanged        -= RhinoDoc_DocumentPropertiesChanged;
            RhinoDoc.BeginOpenDocument                -= RhinoDoc_BeginOpenDocument;
            RhinoDoc.EndOpenDocument                  -= RhinoDoc_EndOpenDocument;
            RhinoDoc.EndOpenDocumentInitialViewUpdate -= RhinoDoc_EndOpenDocumentInitialViewUpdate;
            RhinoDoc.BeginSaveDocument                -= RhinoDoc_BeginSaveDocument;
            RhinoDoc.EndSaveDocument                  -= RhinoDoc_EndSaveDocument;

            RhinoDoc.UserStringChanged                -= RhinoDoc_UserStringChanged;

            RhinoDoc.AddRhinoObject                   -= RhinoDoc_AddRhinoObject;
            RhinoDoc.DeleteRhinoObject                -= RhinoDoc_DeleteRhinoObject;
            RhinoDoc.ReplaceRhinoObject               -= RhinoDoc_ReplaceRhinoObject;
            RhinoDoc.UndeleteRhinoObject              -= RhinoDoc_UndeleteRhinoObject;
            RhinoDoc.PurgeRhinoObject                 -= RhinoDoc_PurgeRhinoObject;
            RhinoDoc.ModifyObjectAttributes           -= RhinoDoc_ModifyObjectAttributes;
            RhinoDoc.BeforeTransformObjects           -= RhinoDoc_BeforeTransformObjects;

            RhinoDoc.SelectObjects                    -= RhinoDoc_SelectObjects;
            RhinoDoc.DeselectObjects                  -= RhinoDoc_DeselectObjects;
            RhinoDoc.DeselectAllObjects               -= RhinoDoc_DeselectAllObjects;
        
            RhinoDoc.LayerTableEvent                  -= RhinoDoc_LayerTableEvent;
            RhinoDoc.DimensionStyleTableEvent         -= RhinoDoc_DimensionStyleTableEvent;
            RhinoDoc.InstanceDefinitionTableEvent     -= RhinoDoc_InstanceDefinitionTableEvent;
            RhinoDoc.LightTableEvent                  -= RhinoDoc_LightTableEvent;
            RhinoDoc.MaterialTableEvent               -= RhinoDoc_MaterialTableEvent;
            RhinoDoc.GroupTableEvent                  -= RhinoDoc_GroupTableEvent;
            RhinoDoc.RenderMaterialsTableEvent        -= RhinoDoc_RenderMaterialsTableEvent;
            RhinoDoc.RenderEnvironmentTableEvent      -= RhinoDoc_RenderEnvironmentTableEvent;
            RhinoDoc.RenderTextureTableEvent          -= RhinoDoc_RenderTextureTableEvent;

            RhinoDoc.TextureMappingEvent              -= RhinoDoc_TextureMappingEvent;
        }
    }


    #region Documents
    private static void RhinoDoc_CloseDocument (object sender, DocumentEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "CloseDocument");
    }
    private static void RhinoDoc_NewDocument (object sender, DocumentEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "NewDocument");
    }
    private static void RhinoDoc_ActiveDocumentChanged (object sender, DocumentEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "ActiveDocumentChanged");
    }
    private static void RhinoDoc_DocumentPropertiesChanged (object sender, DocumentEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "DocumentPropertiesChanged");
    }
    private static void RhinoDoc_BeginOpenDocument (object sender, DocumentOpenEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "BeginOpenDocument");
    }
    private static void RhinoDoc_EndOpenDocument (object sender, DocumentOpenEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "EndOpenDocument");
    }
    private static void RhinoDoc_EndOpenDocumentInitialViewUpdate (object sender, DocumentOpenEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "EndOpenDocumentInitialViewUpdate");
    }
    private static void RhinoDoc_BeginSaveDocument (object sender, DocumentSaveEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "BeginSaveDocument");
    }
    private static void RhinoDoc_EndSaveDocument (object sender, DocumentSaveEventArgs e)
    {
        if (IsEnabled && TrackDocuments)
            SeePanel.Write ("Doc", "EndSaveDocument");
    }
    #endregion


    #region Objects
    private static void RhinoDoc_UserStringChanged (object sender, RhinoDoc.UserStringChangedArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "UserStringChanged");
    }
    private static void RhinoDoc_AddRhinoObject (object sender, RhinoObjectEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "AddRhinoObject");
    }
    private static void RhinoDoc_DeleteRhinoObject (object sender, RhinoObjectEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "DeleteRhinoObject");
    }
    private static void RhinoDoc_ReplaceRhinoObject (object sender, RhinoReplaceObjectEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "ReplaceRhinoObject");
    }
    private static void RhinoDoc_UndeleteRhinoObject (object sender, RhinoObjectEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "UndeleteRhinoObject");
    }
    private static void RhinoDoc_PurgeRhinoObject (object sender, RhinoObjectEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "PurgeRhinoObject");
    }
    private static void RhinoDoc_ModifyObjectAttributes (object sender, RhinoModifyObjectAttributesEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "ModifyObjectAttributes");
    }
    private static void RhinoDoc_BeforeTransformObjects (object sender, RhinoTransformObjectsEventArgs e)
    {
        if (IsEnabled && TrackObjects)
            SeePanel.Write ("Doc", "BeforeTransformObjects");
    }
    #endregion


    #region Selections
    private static void RhinoDoc_SelectObjects (object sender, RhinoObjectSelectionEventArgs e)
    {
        if (IsEnabled && TrackSelections)
            SeePanel.Write ("Doc", "SelectObjects");
    }
    private static void RhinoDoc_DeselectObjects (object sender, RhinoObjectSelectionEventArgs e)
    {
        if (IsEnabled && TrackSelections)
            SeePanel.Write ("Doc", "DeselectObjects");
    }
    private static void RhinoDoc_DeselectAllObjects (object sender, RhinoDeselectAllObjectsEventArgs e)
    {
        if (IsEnabled && TrackSelections)
            SeePanel.Write ("Doc", "DeselectAllObjects");
    }
    #endregion


    #region Tables
    private static void RhinoDoc_LayerTableEvent (object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "LayerTableEvent");
    }
    private static void RhinoDoc_DimensionStyleTableEvent (object sender, Rhino.DocObjects.Tables.DimStyleTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "DimensionStyleTableEvent");
    }
    private static void RhinoDoc_InstanceDefinitionTableEvent (object sender, Rhino.DocObjects.Tables.InstanceDefinitionTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "InstanceDefinitionTableEvent");
    }
    private static void RhinoDoc_LightTableEvent (object sender, Rhino.DocObjects.Tables.LightTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "LightTableEvent");
    }
    private static void RhinoDoc_MaterialTableEvent (object sender, Rhino.DocObjects.Tables.MaterialTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "MaterialTableEvent");
    }
    private static void RhinoDoc_GroupTableEvent (object sender, Rhino.DocObjects.Tables.GroupTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "GroupTableEvent");
    }
    private static void RhinoDoc_RenderMaterialsTableEvent (object sender, RhinoDoc.RenderContentTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "RenderMaterialsTableEvent");
    }
    private static void RhinoDoc_RenderEnvironmentTableEvent (object sender, RhinoDoc.RenderContentTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "RenderEnvironmentTableEvent");
    }
    private static void RhinoDoc_RenderTextureTableEvent (object sender, RhinoDoc.RenderContentTableEventArgs e)
    {
        if (IsEnabled && TrackTables)
            SeePanel.Write ("Doc", "RenderTextureTableEvent");
    }
    #endregion

    private static void RhinoDoc_TextureMappingEvent (object sender, RhinoDoc.TextureMappingEventArgs e)
    {
        SeePanel.Write ("Doc", "TextureMappingEvent");
    }
}
