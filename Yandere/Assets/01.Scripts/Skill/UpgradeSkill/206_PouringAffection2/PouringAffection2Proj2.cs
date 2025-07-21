using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PouringAffection2Proj2 : BaseProjectile
{
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    private PouringAffection2Wrapper _data;
    
    public override void Initialize() { }
    public void Initialize(PouringAffection2Wrapper data, LayerMask enemyLayer, Vector3 targetPosition)
    {
        _starPrefab.SetActive(true);
        _explosionPrefab.SetActive(false);
        
        _data = data;
        this.enemyLayer = enemyLayer;
        
        Vector3 start = GetRandomStartPosition(targetPosition, 8f); // 8f는 시작 거리, 필요에 따라 조정
        
        transform.DOMove(targetPosition, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => Explode());
    }
    
    private Vector3 GetRandomStartPosition(Vector3 target, float distance)
    {
        bool isLeft = Random.value < 0.5f;
        float angle = isLeft ? 135f : 45f; // 90(정상 위쪽) 기준으로 +-45도
        float rad = angle * Mathf.Deg2Rad;

        // direction 구해서 distance만큼 이동
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        return target + dir * distance;
    }

    private void Explode()
    {
        _explosionPrefab.transform.localScale = Vector3.one * _data.secondExplodeRadius;
        
        _starPrefab.SetActive(false);
        _explosionPrefab.SetActive(true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.secondExplodeRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }
        
        Destroy(gameObject, 1f);
    }
}
