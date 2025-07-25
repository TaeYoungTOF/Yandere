using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PouringAffectionProjectile : BaseProjectile
{
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    
    private PouringAffectionDataWrapper _data;
    private Vector3 _targetPosition;
    public override void Initialize()
    {
    }

    public void Initialize(PouringAffectionDataWrapper data, LayerMask enemyLayer, bool isRight)
    {
        _starPrefab.SetActive(true);
        _explosionPrefab.SetActive(false);
        
        _data = data;
        this.enemyLayer = enemyLayer;
        
        _targetPosition = transform.position;
        float startX = _targetPosition.x + (isRight ? 6f : -6f);
        float startY = _targetPosition.y + 6f;
        
        transform.position = new Vector3(startX, startY);

        StartCoroutine(Starfall());
    }

    private IEnumerator Starfall()
    {
        transform.DOMove(_targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => Explode());

        yield return null;
    }

    private void Explode()
    {
        _explosionPrefab.transform.localScale = Vector3.one * _data.explosionRadius;
        
        _starPrefab.SetActive(false);
        _explosionPrefab.SetActive(true);

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
        ObjectPoolManager.Instance.ReturnToPool(PoolType.PouringAffectionProj, gameObject);
    }
}
