using DG.Tweening;
using UnityEngine;

public class BurstingGazeProjectile : BaseProjectile
{
    private float _speed;
    private float _distance;
    private float _damage;
    //private LayerMask enemyLayer;

    private Vector2 _direction;

    public override void Initialize() { }

    public void Initialize(Vector2 direction, float projectileSpeed, float projectileDistance, float skillDamage)
    {
        _speed = projectileSpeed;
        _distance = projectileDistance;
        _damage = skillDamage;
        enemyLayer = LayerMask.GetMask("Enemy");

        _direction = direction.normalized;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _distance);
        transform.DOMove(targetPos, _distance / _speed)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            if (other.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
