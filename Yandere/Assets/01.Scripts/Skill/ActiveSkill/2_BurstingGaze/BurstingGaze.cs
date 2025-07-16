using System.Collections;
using UnityEngine;

[System.Serializable]
public class BurstingGazeDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float angle;

    [Header("UnLeveling Data")]
    public float projectileSize;
    public float projectileDistance;
    
    [Header("Const Data")]
    public readonly float projectileSpeed = 25f;
    public readonly float shootDelay = 0.1f;
}

public class BurstingGaze : ActiveSkill<BurstingGazeDataWrapper>
{
    private LevelupData_BurstingGaze CurrentData => ActiveData as LevelupData_BurstingGaze;
    [SerializeField] private float _projectileSize = 0.5f;
    [SerializeField] private float _projectileDistance = 25f;
    
    [Header("References")]
    [SerializeField] private GameObject _burstingGazeProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.angle = CurrentData.angle;
        
        // UnLeveling Data
        data.projectileSize = _projectileSize * player.stat.FinalSkillRange;
        data.projectileDistance = _projectileDistance * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        StartCoroutine(ShootProjectiles());
    }

    private IEnumerator ShootProjectiles()
    {
        Vector2 origin;
        Vector2 dir;

        for (int i = 0; i < data.projectileCount; i++)
        {
            origin = transform.position;
            dir = player.GetLastMoveDirection();

            float randomAngle = GetRandomAngle(-data.angle / 2f, data.angle / 2f, 0);

            Vector2 finalDir = Quaternion.Euler(0f, 0f, randomAngle) * dir;

            GameObject projGO = Instantiate(_burstingGazeProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<BurstingGazeProjectile>();

            proj.transform.localScale = Vector3.one * data.projectileSize;

            proj.Initialize(finalDir, data.projectileSpeed, data.projectileDistance, data.skillDamage);

            yield return new WaitForSeconds(data.shootDelay);
        }
    }

    private float GetRandomAngle(float min, float max, float mode)
    {
        float u = Random.value;
        float d = max - min;
        float f = (mode - min) / d;

        return u < f
            ? min + Mathf.Sqrt(u * d * (mode - min))
            : max - Mathf.Sqrt((1 - u) * d * (max - mode));
    }
}
