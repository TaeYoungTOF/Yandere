using DG.Tweening;
using UnityEngine;

public class BurnignJealousy2Proj2 : BaseProjectile
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;
    
    private Vector2 _direction;
    private BurningJealousy2Wrapper _data;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, BurningJealousy2Wrapper data, LayerMask enemyLayer)
    {
        fireballPrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
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
        fireballPrefab.SetActive(false);
        explosionPrefab.SetActive(true);
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.secondExplodeRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.secondDmg);
            }
        }

        Destroy(gameObject, 2f);
    }
}
