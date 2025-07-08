using System.Linq;
using UnityEngine;

[System.Serializable]
public class FireballDataWrapper
{
    public int projectileCount;
    public float skillDamage;
    public float skillDuration;
    public float coolDown;
    public float skillRange;
    public float crit;

    public float projectileSize;
    public float explosionRadius;

    public float projectileSpeed;
    public float projectileDistance;
}

public class Fireball : ActiveSkill
{
    private LevelupData_Fireball _currentData => ActiveData as LevelupData_Fireball;

    [SerializeField] private FireballDataWrapper _data;
    [SerializeField] private GameObject _fireballProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;
    private float _coolDownTimer;

    private void Start()
    {
        _data = new FireballDataWrapper();
    }

    public override void UpdateCooldown()
    {
        if (_coolDownTimer > 0f)
        {
            _coolDownTimer -= Time.deltaTime;
        }
    }
    
    public override void TryActivate()
    {
        if (_coolDownTimer <= 0f)
        {
            UpdateActiveData();
            Activate();
            _coolDownTimer = _data.coolDown;
        }
    }

    public override void UpdateActiveData()
    {
        _data.projectileCount = _currentData.projectileCount + SkillManager.Instance.ProjectileCount;
        _data.skillDamage = _currentData.skillDamage * (1 + (SkillManager.Instance.SkillDamage / 100));
        _data.skillDuration = _currentData.skillDuration * (1 + (SkillManager.Instance.SkillDamage / 100));
        _data.coolDown = _currentData.coolDown * (1 - (SkillManager.Instance.CoolDown / 100));
        _data.skillRange = _currentData.skillRange * (1 + (SkillManager.Instance.SkillDamage / 100));
        _data.crit = _currentData.crit + StageManager.Instance.Player.stat.criticalChance + SkillManager.Instance.Crit / 100;

        _data.projectileSize = _currentData.projectileSize;
        _data.explosionRadius = _currentData.explosionRadius;
        _data.projectileSpeed = _currentData.projectileSpeed;
        _data.projectileDistance = _currentData.projectileDistance;
    }

    protected override void Activate()
    {
        if (_data == null || _fireballProjectilePrefab == null)
        {
            Debug.LogWarning("[Fireball] Data or Projectile Prefab is null");
            return;
        }

        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, 20f, _enemyLayer);
        var sortedTargets = hits.OrderBy(h => Vector2.Distance(origin, h.transform.position))
                                .Take(_data.projectileCount);

        foreach (var target in sortedTargets)
        {
            GameObject projGO = Instantiate(_fireballProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<FireballProjectile>();

            if (proj != null)
            {
                Vector2 dir = ((Vector2)target.transform.position - origin).normalized;
                proj.Initialize(_data, dir);
            }

            // 투사체 크기 조절
            projGO.transform.localScale = Vector3.one * _data.projectileSize;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}
