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
    [SerializeField] [HideInInspector] private bool isWeapon;
    [SerializeField] [HideInInspector] private AttackType attackType;
    [SerializeField] [HideInInspector] private GameObject weaponPrefab;

    public int ItemID { get => itemID; }
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }
    public EquipmentSlotType ItemSlot { get => itemSlot; }
    public bool IsWeapon { get => isWeapon; }
    public AttackType AttackType { get => attackType; }
    public GameObject WeaponPrefab { get => weaponPrefab; }

    [CustomEditor(typeof(EquipmentItemSO))]
    public class EquipmentItemSOEditor : Editor
    {
        private EquipmentItemSO eqItemSO;
        private SerializedProperty isWeaponSer, itemSlotSer;

        private void OnEnable()
        {
            isWeaponSer = serializedObject.FindProperty("isWeapon");
            itemSlotSer = serializedObject.FindProperty("itemSlot");
            eqItemSO = (EquipmentItemSO)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(itemSlotSer.enumValueIndex == (int)EquipmentSlotType.RIGHT_ARM || itemSlotSer.enumValueIndex == (int)EquipmentSlotType.LEFT_ARM)
            {
                eqItemSO.isWeapon = EditorGUILayout.Toggle("Is Weapon", isWeaponSer.boolValue); //PropertyField(isWeaponSer);
                if (isWeaponSer.boolValue)
                {
                    eqItemSO.attackType = (AttackType)EditorGUILayout.EnumPopup("Attack type", eqItemSO.attackType);
                    eqItemSO.weaponPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon Prefab", eqItemSO.weaponPrefab, typeof(GameObject), true);
                }
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }    
    }
}


