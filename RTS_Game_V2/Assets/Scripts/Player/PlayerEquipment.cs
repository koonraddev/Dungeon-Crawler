using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform leftHandWeaponPlace, rightHandWeaponPlace, noCombatWeaponPlace;
    private GameObject instantiatedWeapon;
    private EquipmentSlotType weaponSlotType;
    private Enemy enemyInCombat;
    Tween seq1;
    private void OnEnable()
    {
        GameEvents.instance.OnEquipmentUpdate += UpdatePlayerWeaponPrefab;
        GameEvents.instance.OnEnemyClick += ActiveWeapon;
    }

    private void ActiveWeapon(Enemy enemy)
    {
        enemyInCombat = enemy;
        if(enemy != null)
        {
            switch (weaponSlotType)
            {
                case EquipmentSlotType.LEFT_ARM:
                    SwitchPosition(leftHandWeaponPlace, instantiatedWeapon);
                    break;
                case EquipmentSlotType.RIGHT_ARM:
                    SwitchPosition(rightHandWeaponPlace, instantiatedWeapon);
                    break;
                default:
                    break;
            }
        }
        else
        {
            SwitchPosition(noCombatWeaponPlace, instantiatedWeapon, 3f);
        }
    }

    private void SwitchPosition(Transform targetTransform, GameObject weaponObject, float waitSeconds = 0f)
    {
        if(weaponObject == null)
        {
            return;
        }

        Renderer rend = weaponObject.GetComponent<Renderer>();

        if (seq1 != null)
        {
            seq1.Kill();
        }

        seq1 = DOTween.Sequence()
                .AppendInterval(waitSeconds)
                .Append(rend.material.DOFade(0f, 0.2f))
                .AppendCallback(() => weaponObject.transform.SetParent(targetTransform))
                .Append(weaponObject.transform.DOLocalMove(Vector3.zero, 0f))
                .Join(weaponObject.transform.DOLocalRotate(Vector3.zero, 0f))
                .Append(rend.material.DOFade(1f, 0.2f));

        seq1.Play();
    }

    private void UpdatePlayerWeaponPrefab()
    {
        EquipmentSlot eqLeftHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.LEFT_ARM);
        EquipmentSlot eqRightHandSlot = EquipmentManager.instance.GetEquipmentSlot(EquipmentSlotType.RIGHT_ARM);
        Transform spawnPlace = null;

        if (!eqLeftHandSlot.Empty && eqLeftHandSlot.Item.IsWeapon)
        {
            spawnPlace = leftHandWeaponPlace;
            weaponSlotType = eqLeftHandSlot.SlotType;
        }

        if (!eqRightHandSlot.Empty && eqRightHandSlot.Item.IsWeapon)
        {
            spawnPlace = rightHandWeaponPlace;
            weaponSlotType = eqRightHandSlot.SlotType;
        }

        if(spawnPlace != null)
        {
            if (instantiatedWeapon != null)
            {
                Destroy(instantiatedWeapon);
            }

            instantiatedWeapon = Instantiate(eqRightHandSlot.Item.WeaponPrefab, spawnPlace);
            ActiveWeapon(enemyInCombat);
            return;
        }

        if (instantiatedWeapon != null)
        {
            Destroy(instantiatedWeapon);
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnEquipmentUpdate -= UpdatePlayerWeaponPrefab;
    }
}
