using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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
    public readonly float projectileSpeed = 15f;
    public readonly float shootDelay = 0.3f;
}

public class BurstingGaze : ActiveSkill<BurstingGazeDataWrapper>
{
    private LevelupData_BurstingGaze CurrentData => ActiveData as LevelupData_BurstingGaze;
    [SerializeField] private float _projectileSize = 0.5f;
    [SerializeField] private float _projectileDistance = 25f;
    
    [Header("References")]
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
        for (int i = 0; i < data.projectileCount; i++)
        {
            float randomAngle = GetRandomAngle(-data.angle / 2f, data.angle / 2f, 0);
            Vector2 direction = Quaternion.Euler(0f, 0f, randomAngle) * player.GetLastMoveDirection();
            direction.Normalize();

            Vector3 endPosition = transform.position + (Vector3)(direction * data.projectileDistance);

            GameObject projGo = ObjectPoolManager.Instance.GetFromPool(PoolType.BurstingGazeProj, transform.position, Quaternion.identity);
            var proj = projGo.GetComponent<BurstingGazeProjectile>();
            SoundManager.Instance.Play("InGame_PlayerSkill_2_BurstingGaze01");
            proj.Initialize(endPosition, data, _enemyLayer);
            
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
