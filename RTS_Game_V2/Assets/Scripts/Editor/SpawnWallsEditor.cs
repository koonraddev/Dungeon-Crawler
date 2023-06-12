using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnWalls))]
public class SpawnWallsEditor : Editor
{
    private SerializedProperty serializedGenerateEndingDoor;
    private SerializedProperty serializedGenerateManyDoors;
    private SerializedProperty serializedGenerateDoorRandomly;
    private SerializedProperty serializedWallA;
    private SerializedProperty serializedWallB;
    private SerializedProperty serializedWallC;
    private SerializedProperty serializedWallD;


    private void OnEnable()
    {
        serializedGenerateEndingDoor = serializedObject.FindProperty("generateEndingDoor");
        serializedGenerateManyDoors = serializedObject.FindProperty("generateManyDoors");
        serializedGenerateDoorRandomly = serializedObject.FindProperty("generateDoorRandomly");

        serializedWallA = serializedObject.FindProperty("wallA");
        serializedWallB = serializedObject.FindProperty("wallB");
        serializedWallC = serializedObject.FindProperty("wallC");
        serializedWallD = serializedObject.FindProperty("wallD");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.LabelField("Doors Spawn Section", EditorStyles.boldLabel);
        serializedGenerateEndingDoor.boolValue = EditorGUILayout.Toggle("Ending Door", serializedGenerateEndingDoor.boolValue);
        serializedGenerateDoorRandomly.boolValue = EditorGUILayout.Toggle("Doors Position Randomly", serializedGenerateDoorRandomly.boolValue);

        serializedGenerateManyDoors.boolValue = EditorGUILayout.Toggle("Generate Many Doors", serializedGenerateManyDoors.boolValue);

        if (serializedGenerateManyDoors.boolValue)
        {
            EditorGUILayout.HelpBox("Specify number of doors on each wall. If you want generate one door on random wall, uncheck this field", UnityEditor.MessageType.Info);
            serializedWallA.intValue = EditorGUILayout.IntField("Wall A", serializedWallA.intValue);
            serializedWallB.intValue = EditorGUILayout.IntField("Wall B", serializedWallB.intValue);
            serializedWallC.intValue = EditorGUILayout.IntField("Wall C", serializedWallC.intValue);
            serializedWallD.intValue = EditorGUILayout.IntField("Wall D", serializedWallD.intValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
