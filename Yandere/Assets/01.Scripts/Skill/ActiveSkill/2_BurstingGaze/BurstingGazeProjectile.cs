using DG.Tweening;
using UnityEngine;

public class BurstingGazeProjectile : BaseProjectile
{
    private BurstingGazeDataWrapper _data;
    private Tween _moveTween;

    //Gizmo
    private Vector3 _startPos;
    private Vector3 _targetPos;

    public override void Initialize() { }

    public void Initialize(Vector3 targetPos, BurstingGazeDataWrapper data, LayerMask enemyLayer)
    {
        _startPos = transform.position;
        _targetPos = targetPos;
        
        _data = data;
        this.enemyLayer = enemyLayer;

        transform.localScale = Vector3.one * data.projectileSize;

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
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_targetPos == Vector3.zero) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_startPos, _targetPos);
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }
#endif
}
