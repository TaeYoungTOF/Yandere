using UnityEngine;
using DG.Tweening;

public class FireballProjectile : BaseProjectile
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;
    
    private Vector2 _direction;
    private FireballDataWrapper _data;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, FireballDataWrapper data, LayerMask enemyLayer)
    {
        fireballPrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
        _direction = direction;
        _data = data;
        this.enemyLayer = enemyLayer;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.projectileDistance);
        transform.DOMove(targetPos, _data.projectileDistance / _data.projectileSpeed)
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

        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explosionRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }

        Destroy(gameObject, 2f);
    }
}
