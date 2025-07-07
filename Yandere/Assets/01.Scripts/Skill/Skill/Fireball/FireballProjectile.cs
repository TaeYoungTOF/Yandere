using UnityEngine;
using DG.Tweening;

public class FireballProjectile : MonoBehaviour
{
    private float _speed;
    private float _distance;
    private float _damage;
    private float _explosionRadius;
    private LayerMask _enemyLayer;

    private Vector2 _direction;

    [SerializeField] private GameObject explosionPrefab;

    public void Initialize(LevelupData_Fireball data, Vector2 direction)
    {
        _speed = data.projectileSpeed;
        _distance = data.projectileDistance;
        _damage = data.skillDamage;
        _explosionRadius = data.explosionRadius;
        _enemyLayer = LayerMask.GetMask("Enemy");

        _direction = direction;

        // Collider 설정
        var collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = 0.2f;
        }
        else
        {
            Debug.LogWarning("[FireballProjectile] CircleCollider is null");
        }

        // DOTween으로 이동
        Vector3 targetPos = transform.position + (Vector3)(_direction * _distance);
        transform.DOMove(targetPos, _distance / _speed)
                 .SetEase(Ease.Linear)
                 .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _enemyLayer) == 0) return;

        // 충돌 시 즉시 폭발
        Explode();
    }

    private void Explode()
    {
        // 폭발 오브젝트 생성
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity)
                .GetComponent<FireballExplosion>()
                .Initialize(_damage, _explosionRadius, _enemyLayer);
        }

        Destroy(gameObject);
    }
}
