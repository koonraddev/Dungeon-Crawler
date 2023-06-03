using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    private SerializedProperty serializedMinFov;
    private SerializedProperty serializedBaseFov;
    private SerializedProperty serializedMaxFov;
    private SerializedProperty serializedCanZoom;
    private SerializedProperty serializedScrollSpeed;
    private SerializedProperty serializedZoomSensitivity;
    private SerializedProperty serializedCamDistanceMin;
    private SerializedProperty serializedCamDistanceMax;
    private SerializedProperty serializedlocalOffset;
    private SerializedProperty serializedZoomType;

    private void OnEnable()
    {
        serializedMinFov = serializedObject.FindProperty("minFov");
        serializedBaseFov = serializedObject.FindProperty("baseFov");
        serializedMaxFov = serializedObject.FindProperty("maxFov");
        serializedCanZoom = serializedObject.FindProperty("canZoom");
        serializedScrollSpeed = serializedObject.FindProperty("scrollSpeed");
        serializedlocalOffset = serializedObject.FindProperty("localOffset");
        serializedCamDistanceMin = serializedObject.FindProperty("camDistanceMin");
        serializedCamDistanceMax = serializedObject.FindProperty("camDistanceMax");
        serializedZoomSensitivity = serializedObject.FindProperty("zoomSensitivity");
        serializedZoomType = serializedObject.FindProperty("zoomType");
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.LabelField("Essentials", EditorStyles.boldLabel);
        serializedlocalOffset.vector3Value = EditorGUILayout.Vector3Field("Local Camera offset", serializedlocalOffset.vector3Value);
        EditorGUILayout.LabelField("Distance beetwen TargetObject(parent) and camera (this)");
        serializedBaseFov.floatValue = EditorGUILayout.Slider("Base FoV", serializedBaseFov.floatValue, serializedMinFov.floatValue, serializedMaxFov.floatValue);

        EditorGUILayout.Space(20);
        serializedCanZoom.boolValue = EditorGUILayout.Toggle("Camera can zoom", serializedCanZoom.boolValue);
        
        if (serializedCanZoom.boolValue)
        {
            var sds = Enum.GetValues(typeof(CameraFollow.ZoomType));
            string[] ddd = Array.ConvertAll((CameraFollow.ZoomType[])sds, x => x.ToString());
            serializedZoomType.intValue = EditorGUILayout.Popup(serializedZoomType.intValue, ddd);


            serializedScrollSpeed.floatValue = EditorGUILayout.FloatField("Zoom scroll speed", serializedScrollSpeed.floatValue);
            serializedZoomSensitivity.floatValue = EditorGUILayout.Slider("Zoom Sensitivity speed", serializedZoomSensitivity.floatValue, 0, 1);
            switch (serializedZoomType.intValue)
            {
                case (int)CameraFollow.ZoomType.POSITION:
                    EditorGUILayout.LabelField("Position Zoom Settings", EditorStyles.boldLabel);
                    serializedCamDistanceMin.floatValue = EditorGUILayout.FloatField("Minimum Camera Distance", serializedCamDistanceMin.floatValue);
                    serializedCamDistanceMax.floatValue = EditorGUILayout.FloatField("Maximum Camera Distance", serializedCamDistanceMax.floatValue);

                    if (serializedCamDistanceMin.floatValue >= serializedCamDistanceMax.floatValue)
                    {
                        EditorGUILayout.HelpBox("Minimum camera distance cannot be greater than maximum camera distance", UnityEditor.MessageType.Error);
                    }

                    break;
                case (int)CameraFollow.ZoomType.FOV:
                    EditorGUILayout.LabelField("Field of View Zoom Settings", EditorStyles.boldLabel);

                    serializedMinFov.floatValue = EditorGUILayout.Slider("Minimum FoV", serializedMinFov.floatValue, 0, 180);
                    serializedMaxFov.floatValue = EditorGUILayout.Slider("Maximum FoV", serializedMaxFov.floatValue, 0, 180);

                    if (serializedMinFov.floatValue >= serializedMaxFov.floatValue)
                    {
                        EditorGUILayout.HelpBox("Minimum Field of View cannot be greater than Maximum Field of View", UnityEditor.MessageType.Error);
                    }


                    break;
                default:
                    break;
            }

            

            

        }

        serializedObject.ApplyModifiedProperties ();
    }
}