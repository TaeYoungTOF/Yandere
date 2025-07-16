using UnityEngine;
using DG.Tweening;

public class FireballProjectile : BaseProjectile
{
    private float _speed;
    private float _distance;
    private float _damage;
    private float _explosionRadius;

    private Vector2 _direction;

    [SerializeField] private GameObject explosionPrefab;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, float projectileSpeed, float projectileDistance, float skillDamage, float explosionRadius)
    {
        _speed = projectileSpeed;
        _distance = projectileDistance;
        _damage = skillDamage;
        _explosionRadius = explosionRadius;
        enemyLayer = LayerMask.GetMask("Enemy");

        _direction = direction;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _distance);
        transform.DOMove(targetPos, _distance / _speed)
                 .SetEase(Ease.Linear)
                 .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log($"[Fireball Projectile] {other.gameObject.name} Collision!");
            Explode();
        }
    }

    private void Explode()
    {
        if (!explosionPrefab)
        {
            Debug.Log("[Fireball Projectile] Fireball Explosion Prefab is null!");
            return;
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity)
            .GetComponent<FireballExplosion>()
            .Initialize(_damage, _explosionRadius, enemyLayer);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(_direction.normalized * _distance);

        Gizmos.DrawLine(start, end);

        Gizmos.DrawWireSphere(end, 0.3f);
    }
}
