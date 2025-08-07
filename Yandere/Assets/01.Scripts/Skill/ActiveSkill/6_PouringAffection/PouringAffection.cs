using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PouringAffectionDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float explosionRadius;
    
    //[Header("UnLeveling Data")]

    [Header("Const Data")]
    public float minDistance = 5f;      // 별똥별 끼리는 최소 value의 거리를 갖음
    public float xDistance = 5f;        // 별똥별은 -value부터 value 사이의 x값에 떨어짐
    public float yDistance = 5f;        // 별똥별은 -value부터 value 사이의 y값에 떨어짐
}

public class PouringAffection : ActiveSkill<PouringAffectionDataWrapper>
{
    private LevelupData_PouringAffection CurrentData => ActiveData as LevelupData_PouringAffection;

    [Header("References")]
    //[SerializeField] private GameObject _pouringAffectionProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.explosionRadius = CurrentData.explosionRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        Vector2 center = transform.position;
        List<Vector2> spawnPositions = GenerateRandomPoints(data.projectileCount, data.xDistance, data.yDistance);
        //_lastSpawnedPoints = spawnPositions;

        foreach (Vector2 offset in spawnPositions)
        {
            Vector2 spawnPosition = center + offset;
            Quaternion spawnRotation = Quaternion.Euler(0f, offset.x > 0 ? 180 : 0, -45);
            
            //GameObject go = Instantiate(_pouringAffectionProjectilePrefab, spawnPosition, spawnRotation);
            GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.PouringAffectionProj, spawnPosition, spawnRotation);
            PouringAffectionProjectile projectile = go.GetComponent<PouringAffectionProjectile>();
            SoundManager.Instance.PlayRandomSFX(SoundCategory.PouringAffectionProjectile);
            projectile.Initialize(data, _enemyLayer, offset.x > 0);
        }
    }

    private List<Vector2> GenerateRandomPoints(int count, float x, float y)
    {
        List<Vector2> points = new(count);

        while (points.Count < count)
        {
            Vector2 newPoint = new(Random.Range(-x, x), Random.Range(-y, y));
            bool isValid = true;

            foreach (var existing in points)
            {
                if (Vector2.Distance(newPoint, existing) < data.minDistance)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
                points.Add(newPoint);
        }

        return points;
    }

#if UNITY_EDITOR
    private List<Vector2> _lastSpawnedPoints = new();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var point in _lastSpawnedPoints)
        {
            Gizmos.DrawSphere((Vector2)transform.position + point, 0.3f);
        }
    }
#endif
}
