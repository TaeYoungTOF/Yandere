using System.Collections;
using UnityEngine;

[System.Serializable]
public class BurstingGazeDataWrapper : AcviteDataWapper
{
    public float angle;
}

public class BurstingGaze : ActiveSkill
{
    private LevelupData_BurstingGaze _currentData => ActiveData as LevelupData_BurstingGaze;

    [SerializeField] private BurstingGazeDataWrapper _data;
    [SerializeField] private GameObject _burstingGazeProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Unupgradable Data")]
    [SerializeField] private float _projectileSize = 0.5f;
    [SerializeField] private float _projectileSpeed = 25f;
    [SerializeField] private float _projectileDistance = 25f;
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
        _data.projectileCount = _currentData.projectileCount + player.stat.ProjectileCount;
        _data.skillDamage = _currentData.skillDamage;
        _data.coolTime = _currentData.coolTime * (1 - player.stat.CoolDown / 100f);

        _data.angle = _currentData.angle;
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
            dir = player.MoveVec;
            if (dir == Vector2.zero)
                dir = Vector2.right;

            GameObject projGO = Instantiate(_burstingGazeProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<BurstingGazeProjectile>();

            proj.transform.localScale = Vector3.one * _projectileSize;

            proj.Initialize(dir, _projectileSpeed, _projectileDistance, CalculateDamage(_data.skillDamage));

            yield return new WaitForSeconds(_shootDelay);
        }
    }
}
