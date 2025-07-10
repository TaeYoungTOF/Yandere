using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    private float _debugRadius;
    private Vector3 _debugPosition;
    private bool _initialized;

    public void Initialize(float damage, float radius, LayerMask enemyLayer)
    {
        _debugRadius = radius;
        _debugPosition = transform.position;
        _initialized = true;

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

    private void OnDrawGizmos()
    {
        if (!_initialized) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawWireSphere(_debugPosition, _debugRadius);
    }
}
