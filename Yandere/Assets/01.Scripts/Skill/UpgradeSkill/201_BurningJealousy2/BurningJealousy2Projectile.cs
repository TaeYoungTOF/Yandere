using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BurningJealousy2Projectile : BaseProjectile
{
    private BurningJealousy2Wrapper _data;

    private Vector2 _direction;
    
    [SerializeField] private GameObject _fireballProjectilePrefab;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, BurningJealousy2Wrapper data, LayerMask enemyLayer)
    {
        _direction = direction;
        _data = data;
        this.enemyLayer = enemyLayer;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.pjtDistance);
        transform.DOMove(targetPos, _data.pjtDistance / _data.pjtSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explodeRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }
        
        for (int i = 0; i < _data.secondPjtCount; i++)
        {
            float angle = (360f / _data.secondPjtCount) * i;
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            
            GameObject projGO = Instantiate(_fireballProjectilePrefab, transform.position, Quaternion.identity);
            var proj = projGO.GetComponent<BurnignJealousy2Proj2>();
            proj.Initialize(dir, _data, enemyLayer);

            proj.transform.localScale = Vector3.one * _data.secondPjtSize;
        }

        Destroy(gameObject);
    }
}
