using DG.Tweening;
using UnityEngine;

public class BurstingGazeProjectile : BaseProjectile
{
    private Vector2 _direction;
    private BurstingGazeDataWrapper _data;
    private Tween _moveTween;

    public override void Initialize() { }

    public void Initialize(Vector2 direction, BurstingGazeDataWrapper data, LayerMask enemyLayer)
    {
        _direction = direction.normalized;
        _data = data;
        this.enemyLayer = enemyLayer;

        transform.localScale = Vector3.one * data.projectileSize;

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
                //SoundManager.Instance.PlayRandomSFX(SoundCategory.BurstingGaze);
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        _moveTween?.Kill();
        ObjectPoolManager.Instance.ReturnToPool(PoolType.BurstingGazeProj, gameObject);
    }
}
