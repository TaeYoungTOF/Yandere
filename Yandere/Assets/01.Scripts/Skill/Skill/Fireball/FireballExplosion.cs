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

        // 파티클 등 시각효과 종료 후 제거 (여기선 1초 후 제거)
        Destroy(gameObject, 1f);
    }
}
