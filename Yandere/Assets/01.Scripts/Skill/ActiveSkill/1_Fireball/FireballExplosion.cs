using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    public void Initialize(float damage, float radius, LayerMask enemyLayer)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject, 2f);
    }
}
