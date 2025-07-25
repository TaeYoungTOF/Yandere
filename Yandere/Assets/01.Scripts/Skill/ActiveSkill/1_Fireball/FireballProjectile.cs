using UnityEngine;
using DG.Tweening;

public class FireballProjectile : BaseProjectile
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;
    
    private Vector2 _direction;
    private FireballDataWrapper _data;
    private Tween _moveTween;
    private bool _hasExploded = false;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, FireballDataWrapper data, LayerMask enemyLayer)
    {
        fireballPrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
        _direction = direction;
        _data = data;
        this.enemyLayer = enemyLayer;

        // 투사체 크기 조절
        transform.localScale = Vector3.one * _data.projectileSize;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.projectileDistance);
        _moveTween = transform.DOMove(targetPos, _data.projectileDistance / _data.projectileSpeed)
                              .SetEase(Ease.Linear)
                              .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;
        
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            _moveTween?.Kill();
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        
        fireballPrefab.SetActive(false);
        explosionPrefab.SetActive(true);
        
        transform.localScale = Vector3.one * _data.explosionRadius;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explosionRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }

        Invoke(nameof(ReturnToPool), 1f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.FireballProj, gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_data == null) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f); // 주황색 반투명
        Gizmos.DrawSphere(transform.position, _data.explosionRadius);
    }
#endif
}
