using UnityEditor;

[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    private SerializedProperty serializedMinFov;
    private SerializedProperty serializedBaseFov;
    private SerializedProperty serializedMaxFov;
    private SerializedProperty serializedCanZoom;
    private SerializedProperty serializedScrollSpeed;
    private SerializedProperty serializedlocalOffset;

    private void OnEnable()
    {
        serializedMinFov = serializedObject.FindProperty("minFov");
        serializedBaseFov = serializedObject.FindProperty("baseFov");
        serializedMaxFov = serializedObject.FindProperty("maxFov");
        serializedCanZoom = serializedObject.FindProperty("canZoom");
        serializedScrollSpeed = serializedObject.FindProperty("scrollSpeed");
        serializedlocalOffset = serializedObject.FindProperty("localOffset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedlocalOffset.vector3Value = EditorGUILayout.Vector3Field("Local Camera offset", serializedlocalOffset.vector3Value);
        EditorGUILayout.LabelField("Distance beetwen TargetObject(parent) and camera (this)");
        
        EditorGUILayout.Space(10);
        serializedCanZoom.boolValue = EditorGUILayout.Toggle("Camera can zoom", serializedCanZoom.boolValue);

        if (serializedCanZoom.boolValue)
        {
            serializedScrollSpeed.floatValue = EditorGUILayout.FloatField("Zoom scroll speed", serializedScrollSpeed.floatValue);
            serializedMinFov.floatValue = EditorGUILayout.Slider("Minimum FoV", serializedMinFov.floatValue, 0, 180);
            serializedBaseFov.floatValue = EditorGUILayout.Slider("Base FoV", serializedBaseFov.floatValue, serializedMinFov.floatValue, serializedMaxFov.floatValue);
            serializedMaxFov.floatValue = EditorGUILayout.Slider("Maximum FoV", serializedMaxFov.floatValue, 0, 180);

            if (serializedMinFov.floatValue >= serializedMaxFov.floatValue)
            {
                EditorGUILayout.HelpBox("Minimum Field of View cannot be greater than Maximum Field of View", UnityEditor.MessageType.Error);
            }
        }

        serializedObject.ApplyModifiedProperties ();
    }
}