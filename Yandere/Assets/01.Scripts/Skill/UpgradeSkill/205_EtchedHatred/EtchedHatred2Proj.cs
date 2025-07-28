using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtchedHatred2Proj : BaseProjectile
{
    [SerializeField] private GameObject _debuffPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    
    private EtchedHatred2Wrapper _data;
    private Transform _target;
    private bool _isExploding;

    private HashSet<Transform> _attachedEnemies;
    
    public override void Initialize() { }
    public void Initialize(EtchedHatred2Wrapper data, LayerMask enemyLayer, Transform target, HashSet<Transform>  attachedEnemies)
    {
        _debuffPrefab.SetActive(true);
        _explosionPrefab.SetActive(false);
        
        _data = data;
        this.enemyLayer = enemyLayer;
        _target = target;
        _attachedEnemies = attachedEnemies;
        
        StartCoroutine(UpdatePosition());
    }

    private IEnumerator  UpdatePosition()
    {
        _isExploding = false;
        
        while (true)
        {
            if (_target)
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

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explosionRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);

                // 감염 전파(중복 방지 + 확률)
                if (!_attachedEnemies.Contains(e.transform))
                {
                    if (Random.Range(0, 100) < _data.infectChance)
                    {
                        Vector3 spawnPos = e.transform.position;

                        GameObject go = Instantiate(gameObject, spawnPos, Quaternion.identity);
                        EtchedHatred2Proj projectile = go.GetComponent<EtchedHatred2Proj>();
                        projectile.Initialize(_data, enemyLayer, e.transform, _attachedEnemies);

                        _attachedEnemies.Add(e.transform); // 중복 방지
                    }
                }
            }
        }

        Invoke(nameof(ReturnToPool), 0.5f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.EtchedHatred2Proj, gameObject);
    }
    
    private void OnDrawGizmos()
    {
        // 폭발 범위를 빨간색으로 표시
        Gizmos.color = Color.red;

        // 실제 폭발 위치 기준, 반지름을 기준으로 그려짐
        Gizmos.DrawWireSphere(transform.position, _data != null ? _data.explosionRadius : 1f);
    }
}
