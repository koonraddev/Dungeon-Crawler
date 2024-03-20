using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatisticalUtility
{
    public static Dictionary<int, RaycastHit[]> raycastDict = new();
    public static float DamageMultiplier(float defenceValue)
    {
        if (defenceValue >= 0)
        {
            return 100 / (100 + defenceValue);
        }
        else
        {
            return 2 - (100 / (100 - defenceValue));
        }
    }

    public static float AttackCooldown(float attackSpeed) { return 60 / attackSpeed; }

    public static float HealthRegeneration(float maxHP, float points, float percentage)
    {
        return points + (maxHP * (percentage / 100));
    }

    public static float ProjectileSpeed(float physicalDamage, float magicDamage, float attackSpeed, float attackRange)
    {
        return 10 + physicalDamage * 0.02f + magicDamage * 0.02f + attackSpeed * 0.1f + attackRange * 0.2f;
    }

    public static bool CheckIfTargetInRange(GameObject requestingObject, GameObject targetObject, float rangeToCheck, out Vector3 closestPoint, bool colliderIsTarget = false)
    {
        closestPoint = Vector3.zero;
        if (requestingObject == null || targetObject == null) return false;

        Vector3 startPoint = requestingObject.transform.position;
        Vector3 targetPoint = targetObject.transform.position;

        if (colliderIsTarget)
        {
            if (requestingObject.TryGetComponent(out Collider requestColl))
            {
                startPoint = requestColl.ClosestPoint(targetObject.transform.position);
            }

            if (targetObject.TryGetComponent(out Collider targetColl))
            {
                targetPoint = targetColl.ClosestPoint(requestingObject.transform.position);
            }
        }

        float distToTarget = Vector3.Distance(startPoint, targetPoint);
        Vector3 dirToTarget = (targetObject.transform.position - requestingObject.transform.position).normalized;
        float distToMove = distToTarget - rangeToCheck + 0.05f;
        closestPoint = requestingObject.transform.position + dirToTarget * distToMove;

        return distToTarget <= rangeToCheck;
    }

    public static bool CheckIfTargetInRange(GameObject requestingObject, GameObject targetObject, float rangeToCheck, bool colliderIsTarget = false)
    {
        if (requestingObject == null || targetObject == null) return false;

        Vector3 startPoint = requestingObject.transform.position;
        Vector3 targetPoint = targetObject.transform.position;

        if (colliderIsTarget)
        {
            if (requestingObject.TryGetComponent(out Collider requestColl))
            {
                startPoint = requestColl.ClosestPoint(targetObject.transform.position);
            }

            if (targetObject.TryGetComponent(out Collider targetColl))
            {
                targetPoint = targetColl.ClosestPoint(requestingObject.transform.position);
            }
        }

        float distToTarget = Vector3.Distance(startPoint, targetPoint);
        return distToTarget <= rangeToCheck;
    }

    public static bool CheckIfTargetIsBlocked(GameObject requestingObject, GameObject targetObject)
    {
        float distance = Vector3.Distance(requestingObject.transform.position, targetObject.transform.position);
        Vector3 dir = targetObject.transform.position - requestingObject.transform.position;
        Ray enemyRay = new(requestingObject.transform.position, dir);

        if (!raycastDict.ContainsKey(requestingObject.GetInstanceID()))
        {
            raycastDict.Add(requestingObject.GetInstanceID(), new RaycastHit[5]);
        }

        int numHits = Physics.RaycastNonAlloc(enemyRay, raycastDict[requestingObject.GetInstanceID()], distance);
        if (numHits > 0)
        {
            for (int i = 0; i < numHits; i++)
            {
                if (raycastDict[requestingObject.GetInstanceID()][i].collider.gameObject.CompareTag("Wall"))
                {
                    return true;
                }
            }
        }
        return false;
    }

}