using DG.Tweening;
using UnityEngine;

public class BurningJealousy2Proj2 : BaseProjectile
{
    private Vector2 _direction;
    private BurningJealousy2Wrapper _data;
    private Tween _moveTween;
    private bool _hasExploded;
    
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, BurningJealousy2Wrapper data, LayerMask enemyLayer)
    {
        fireballPrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
        _direction = direction;
        _data = data;
        this.enemyLayer = enemyLayer;
        _hasExploded = false;

        transform.localScale = Vector3.one * _data.secondPjtSize;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.pjtDistance);
        _moveTween = transform.DOMove(targetPos, _data.pjtDistance / _data.pjtSpeed)
                              .SetEase(Ease.Linear)
                              .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        
        _moveTween?.Kill();

        fireballPrefab.SetActive(false);
        explosionPrefab.SetActive(true);
        
        transform.localScale = Vector3.one * _data.secondExplodeRadius;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.secondExplodeRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.secondDmg);
            }
        }

        Invoke(nameof(ReturnToPool), 2f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.BurningJealousy2Proj2, gameObject);
    }
}
