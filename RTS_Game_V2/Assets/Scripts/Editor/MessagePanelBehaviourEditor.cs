using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MessagePanelBehaviour))]
public class MessagePanelBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        MessagePanelBehaviour messagePB = (MessagePanelBehaviour)target;
        EditorGUILayout.HelpBox("This script effects work only on panel(this) and all chilld objects with TMP_Text Component", UnityEditor.MessageType.Info);

        GUIStyle style = GUIStyle.none;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 14;

        EditorGUILayout.PrefixLabel("List of effects", GUIStyle.none, style);

        EditorGUILayout.BeginHorizontal(style);
        messagePB.fadeEffect = EditorGUILayout.ToggleLeft("Fade Effect", messagePB.fadeEffect);
        if (messagePB.fadeEffect == true)
        {
            messagePB.fadingTime = EditorGUILayout.FloatField("Fade Time", messagePB.fadingTime);
        }
        EditorGUILayout.EndHorizontal();
        if (messagePB.fadingTime < 0)
        {
            EditorGUILayout.HelpBox("Fading Time should be greater than 0", UnityEditor.MessageType.Warning);
        }
    }
}
