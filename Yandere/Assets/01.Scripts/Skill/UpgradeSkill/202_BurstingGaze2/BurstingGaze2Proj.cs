using DG.Tweening;
using UnityEngine;

public class BurstingGaze2Proj : BaseProjectile
{
    private Vector2 _direction;
    private BurstingGaze2Wrapper _data;
    private Tween _moveTween;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, BurstingGaze2Wrapper data, LayerMask enemyLayer)
    {
        _data = data;
        this.enemyLayer = enemyLayer;

        _direction = direction.normalized;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.projectileDistance);
        _moveTween = transform.DOMove(targetPos,  _data.projectileDistance / _data.projectileSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(ReturnToPool);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            if (other.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        _moveTween?.Kill();
        ObjectPoolManager.Instance.ReturnToPool(PoolType.BurningJealousy2Proj, gameObject);
    }
}