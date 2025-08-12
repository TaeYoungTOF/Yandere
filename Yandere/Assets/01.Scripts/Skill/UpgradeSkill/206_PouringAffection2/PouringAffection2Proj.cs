using System.Collections;
using UnityEngine;

public class PouringAffection2Proj : BaseProjectile
{
    [SerializeField] private GameObject _auraPrefab;
    [SerializeField] private GameObject _proj2Prefab;
    
    private PouringAffection2Wrapper _data;
    private Vector3 _targetPosition;
    
    public override void Initialize() { }
    public void Initialize(PouringAffection2Wrapper data, LayerMask enemyLayer)
    {
        _auraPrefab.SetActive(true);
        
        _data = data;
        this.enemyLayer = enemyLayer;

        transform.localScale = Vector3.one * data.explodeRadius;

        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(1.5f);
        float timer = 0f;

        while (timer < _data.spawnDuration)
        {
            // N개 위치 선정
            for (int i = 0; i < _data.projectileCount; i++)
            {
                Vector3 spawnPos = GetRandomPointInCircle(transform.position, _data.explodeRadius);
                Starfall(spawnPos);
            }

            yield return new WaitForSeconds(_data.spawnInterval);
            timer += _data.spawnInterval;
        }

        _auraPrefab.SetActive(false);
        ObjectPoolManager.Instance.ReturnToPool(PoolType.PouringAffection2Proj, gameObject);
    }

    private Vector3 GetRandomPointInCircle(Vector3 center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return center + new Vector3(randomCircle.x, randomCircle.y, 0f);
    }

    private void Starfall(Vector3 spawnPosition)
    {
        GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.PouringAffection2Proj2, spawnPosition, Quaternion.identity);
        PouringAffection2Proj2 proj2 = go.GetComponent<PouringAffection2Proj2>();
        proj2.Initialize(_data, enemyLayer, spawnPosition);
    }
}
