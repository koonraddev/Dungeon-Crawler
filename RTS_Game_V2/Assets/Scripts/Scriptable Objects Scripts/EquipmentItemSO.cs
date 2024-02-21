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
    [SerializeField] [HideInInspector] private GameObject weaponPrefab, projectilePrefab;

    public int ItemID { get => itemID; }
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public StatisticsSO ItemStatistics { get => itemStatistics; }
    public EquipmentSlotType ItemSlot { get => itemSlot; }
    public bool IsWeapon { get => isWeapon; }
    public bool ProjectileAttack
    {
        get
        {
            return attackType switch
            {
                AttackType.FISTS or AttackType.SWORD => false,
                AttackType.WAND or AttackType.BOW or AttackType.SPELL => true,
                _ => false,
            };
        }
    }
    public AttackType AttackType { get => attackType; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }

    [CustomEditor(typeof(EquipmentItemSO))]
    private class EquipmentItemSOEditor : Editor
    {
        private EquipmentItemSO eqItemSO;

        private void OnEnable()
        {
            eqItemSO = (EquipmentItemSO)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (eqItemSO.itemSlot == EquipmentSlotType.RIGHT_ARM || eqItemSO.itemSlot == EquipmentSlotType.LEFT_ARM)
            {
                eqItemSO.isWeapon = EditorGUILayout.Toggle("Is Weapon", eqItemSO.isWeapon);
                if (eqItemSO.isWeapon)
                {
                    eqItemSO.attackType = (AttackType)EditorGUILayout.EnumPopup("Attack type", eqItemSO.attackType);
                    eqItemSO.weaponPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon Prefab", eqItemSO.weaponPrefab, typeof(GameObject), true);

                    if (eqItemSO.attackType == AttackType.WAND || eqItemSO.attackType == AttackType.BOW || eqItemSO.attackType == AttackType.SPELL)
                    {
                        eqItemSO.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", eqItemSO.projectilePrefab, typeof(GameObject), true);
                    }
                }
            }
            else
            {
                eqItemSO.isWeapon = false;
                eqItemSO.weaponPrefab = null;
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }    
    }
}


