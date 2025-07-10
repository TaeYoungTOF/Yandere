using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class FireballProjectile : MonoBehaviour
{
    private float _speed;
    private float _distance;
    private float _damage;
    private float _explosionRadius;
    private LayerMask _enemyLayer;

    private Vector2 _direction;

    [SerializeField] private GameObject explosionPrefab;

    public void Initialize(FireballDataWrapper data, Vector2 direction)
    {
        _speed = data.projectileSpeed;
        _distance = data.projectileDistance;
        _damage = data.skillDamage;
        _explosionRadius = data.explosionRadius;
        _enemyLayer = LayerMask.GetMask("Enemy");

        _direction = direction;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _distance);
        transform.DOMove(targetPos, _distance / _speed)
                 .SetEase(Ease.Linear)
                 .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _enemyLayer) != 0)
        {
            Debug.Log($"[Fireball Projectile] {other.gameObject.name} Collision!");
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionPrefab == null)
        {
            Debug.Log("[Fireball Projectile] Fireball Explosion Prefab is null!");
            return;

        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity)
            .GetComponent<FireballExplosion>()
            .Initialize(_damage, _explosionRadius, _enemyLayer);

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
