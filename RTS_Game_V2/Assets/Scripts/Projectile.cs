using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DamageDealer();
public class Projectile : MonoBehaviour
{
    GameObject targetObject;
    float movementSpeed;
    Vector3 targetPosition;
    DamageDealer dealer;

    void Update()
    {
        targetPosition = targetObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        if (transform.position == targetObject.transform.position)
        {
            DestroyProjectile();
        }
    }

    public void SetProjectile(GameObject targetObject, float movementSpeed, DamageDealer dealer)
    {
        this.targetObject = targetObject;
        targetPosition = targetObject.transform.position;
        this.movementSpeed = movementSpeed;
        this.dealer = dealer;
        float distance = Vector3.Distance(transform.position, targetPosition);

        float time = distance / movementSpeed;
        Invoke(nameof(DestroyProjectile), time);
    }

    private void DestroyProjectile()
    {
        dealer?.Invoke();
        Destroy(gameObject);
    }
}
