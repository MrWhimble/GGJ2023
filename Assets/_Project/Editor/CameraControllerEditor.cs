using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraController cc = target as CameraController;
        if (cc == null)
        {
            base.OnInspectorGUI();
            return;
        }
        
        if (GUILayout.Button("Zoom In"))
        {
            cc.ZoomIn();
        }

        if (GUILayout.Button("Zoom Out"))
        {
            cc.ZoomOut();
        }

        if (GUILayout.Button("Instantly Zoom In"))
        {
            cc.InstantZoomIn();
        }

        if (GUILayout.Button("Instantly Zoom Out"))
        {
            cc.InstantZoomOut();
        }
        
        base.OnInspectorGUI();
    }
}