using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballZone : MonoBehaviour
{
    public float duration = 0.2f;
    public float tickDamage = 10f;
    public LayerMask enemyLayer;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(tickDamage * Time.deltaTime);
            }
        }
    }
}
