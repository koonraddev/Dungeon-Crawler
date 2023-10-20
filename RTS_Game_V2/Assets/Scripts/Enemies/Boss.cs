using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    protected sealed override void Die()
    {
        GameEvents.instance.EnemyClick(null);
        LootManager.instance.CreateLoot(gameObject.transform.position, lootSO.GetContainer(Name), lootSO.LootTimeExisting);
        GameEvents.instance.ActivateTeleport();
        gameObject.SetActive(false);
    }
}
