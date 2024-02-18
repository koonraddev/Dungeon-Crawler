using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatisticalUtility
{
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

    public static bool CheckIfTargetInRange(GameObject requestingObject, GameObject targetObject, float rangeToCheck, out Vector3 closestPoint, bool colliderIsTarget = false)
    {
        closestPoint = Vector3.zero;
        if (requestingObject == null || targetObject == null) return false;

        Vector3 target = targetObject.transform.position;

        if (colliderIsTarget)
        {
            Collider col = targetObject.GetComponent<Collider>();
            if (col != null) target = col.ClosestPoint(targetObject.transform.position);
        }

        float distToTarget = Vector3.Distance(requestingObject.transform.position, target);
        Vector3 dirToTarget = (targetObject.transform.position - requestingObject.transform.position).normalized;
        float distToMove = Mathf.Ceil(distToTarget - rangeToCheck);
        closestPoint = requestingObject.transform.position + dirToTarget * distToMove;

        return distToTarget <= rangeToCheck;
    }

    public static bool CheckIfTargetInRange(GameObject requestingObject, GameObject targetObject, float rangeToCheck, bool colliderIsTarget = false)
    {
        if (requestingObject == null || targetObject == null) return false;

        Vector3 target = targetObject.transform.position;

        if (colliderIsTarget)
        {
            Collider col = targetObject.GetComponent<Collider>();
            if (col != null) target = col.ClosestPoint(targetObject.transform.position);
        }

        float distToTarget = Vector3.Distance(requestingObject.transform.position, target);
        return distToTarget <= rangeToCheck;
    }
}