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
}

public class BurstingGaze : ActiveSkill
{
    private LevelupData_BurstingGaze CurrentData => ActiveData as LevelupData_BurstingGaze;

    [SerializeField] private BurstingGazeDataWrapper _data;
    [SerializeField] private GameObject _burstingGazeProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("UnLeveling Data")]
    [SerializeField] private float _projectileSize = 0.5f;
    [SerializeField] private float _projectileDistance = 25f;
    
    [Header("Const Data")]
    [SerializeField] private float _projectileSpeed = 25f;
    [SerializeField] private float _shootDelay = 0.1f;

    public override void UpdateCooldown()
    {
        if (coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;
        }
    }
    
    public override void TryActivate()
    {
        if (coolDownTimer <= 0f)
        {
            UpdateActiveData();
            Activate();
            coolDownTimer = _data.coolTime;
        }
    }

    public override void UpdateActiveData()
    {
        // Active Skill Data
        _data.projectileCount = CurrentData.projectileCount + player.stat.ProjectileCount;
        _data.skillDamage = CurrentData.skillDamage;
        _data.coolTime = CurrentData.coolTime * (1 - player.stat.CoolDown / 100f);

        // Leveling Data
        _data.angle = CurrentData.angle;
        
        // UnLeveling Data
        _data.projectileSize = _projectileSize * player.stat.FinalSkillRange;
        _data.projectileDistance = _projectileDistance * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        StartCoroutine(ShootProjectiles());
    }

    private IEnumerator ShootProjectiles()
    {
        Vector2 origin;
        Vector2 dir;

        for (int i = 0; i < _data.projectileCount; i++)
        {
            origin = transform.position;
            dir = player.GetLastMoveDirection();

            float randomAngle = GetRandomAngle(-_data.angle / 2f, _data.angle / 2f, 0);

            Vector2 finalDir = Quaternion.Euler(0f, 0f, randomAngle) * dir;

            GameObject projGO = Instantiate(_burstingGazeProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<BurstingGazeProjectile>();

            proj.transform.localScale = Vector3.one * _projectileSize;

            proj.Initialize(finalDir, _projectileSpeed, _projectileDistance, CalculateDamage(_data.skillDamage));

            yield return new WaitForSeconds(_shootDelay);
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
