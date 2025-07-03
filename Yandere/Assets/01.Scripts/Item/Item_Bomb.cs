using UnityEngine;

public class Item_Bomb : Item
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _damage = 50f;

    public override void Use(Player player)
    {
        Debug.Log("[Bomb] Bomb!");

        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, _explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(_damage);
            }
        }

        Destroy(gameObject);
    }
}
