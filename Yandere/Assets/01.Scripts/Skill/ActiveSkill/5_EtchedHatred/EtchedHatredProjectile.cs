using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtchedHatredProjectile : BaseProjectile
{
    [SerializeField] private GameObject _debuffPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    
    private EtchedHatredDataWrapper _data;
    private Transform _target;
    private bool _isExploding;
    
    public override void Initialize() { }
    public void Initialize(EtchedHatredDataWrapper data, LayerMask enemyLayer, Transform target)
    {
        _debuffPrefab.SetActive(true);
        _explosionPrefab.SetActive(false);
        
        _data = data;
        this.enemyLayer = enemyLayer;
        _target = target;
        
        StartCoroutine(UpdatePosition());
    }

    private IEnumerator  UpdatePosition()
    {
        _isExploding = false;
        
        while (true)
        {
            if (_target.gameObject.activeSelf)
            {
                transform.position = _target.position;
            }
            else if (!_isExploding)
            {
                _isExploding = true;
                yield return new WaitForSeconds(0.5f);
                Explode();
                yield break;
            }

            yield return null;
        }
    }

    private void Explode()
    {
        transform.localScale = Vector3.one * _data.explosionRadius;
        
        _debuffPrefab.SetActive(false);
        _explosionPrefab.SetActive(true);

        if (_explosionPrefab)
        {
            SoundManager.Instance.PlayRandomSFX(SoundCategory.EtchedHatred);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explosionRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }

        Invoke(nameof(ReturnToPool), 0.5f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.EtchedHatredProj, gameObject);
    }
    
    private void OnDrawGizmos()
    {
        // 폭발 범위를 빨간색으로 표시
        Gizmos.color = Color.red;

        // 실제 폭발 위치 기준, 반지름을 기준으로 그려짐
        Gizmos.DrawWireSphere(transform.position, _data != null ? _data.explosionRadius : 1f);
    }
}
