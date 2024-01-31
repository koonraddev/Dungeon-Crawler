using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentItem", menuName = "Scriptable Objects/Items/Equipment Item")]
public class EquipmentItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private StatisticsSO itemStatistics;
    [SerializeField] private EquipmentSlotType itemSlot;
    [SerializeField] [HideInInspector] private AttackType attackType;

    public int ItemID { get => itemID; }
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }
    public EquipmentSlotType ItemSlot { get => itemSlot; }
    public AttackType AttackType { get => attackType; }

    [CustomEditor(typeof(EquipmentItemSO))]
    public class EquipmentItemSOEditor : Editor
    {
        private EquipmentItemSO eqItemSO;
        private SerializedProperty serItemSlot;

        private void OnEnable()
        {
            serItemSlot = serializedObject.FindProperty("itemSlot");
            eqItemSO = (EquipmentItemSO)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (serItemSlot.enumValueIndex == (int)EquipmentSlotType.RIGHT_ARM)
            {
                eqItemSO.attackType = (AttackType)EditorGUILayout.EnumPopup("Attack type", eqItemSO.attackType);
            }
            serializedObject.Update();
        }    
    }
}


