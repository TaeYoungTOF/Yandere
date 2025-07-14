using System;
using UnityEngine;

public class Item_Bomb : Item
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _damage = 50f;
    [SerializeField] private LayerMask _layerMask;
    

    public override void Use(Player player)
    {
        Debug.Log("[Bomb] Bomb!");

        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, _explosionRadius, _layerMask);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(_damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
