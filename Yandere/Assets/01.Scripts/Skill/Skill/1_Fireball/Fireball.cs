using System.Linq;
using UnityEngine;

[System.Serializable]
public class FireballDataWrapper : AcviteDataWapper
{
    public float projectileSize;
    public float explosionRadius;
}

public class Fireball : ActiveSkill
{
    private LevelupData_Fireball _currentData => ActiveData as LevelupData_Fireball;

    [SerializeField] private FireballDataWrapper _data;
    [SerializeField] private GameObject _fireballProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Unupgradable Data")]
    [SerializeField] private float _projectileSpeed = 15f;
    [SerializeField] private float _projectileDistance = 30f;
    [SerializeField] private float _enemySearchRange = 5f;

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

        _data.projectileSize = _currentData.projectileSize;
        _data.explosionRadius = _currentData.explosionRadius;
    }

    protected override void Activate()
    {
        if (_data == null || _fireballProjectilePrefab == null)
        {
            Debug.LogWarning("[Fireball] Data or Projectile Prefab is null");
            return;
        }

        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, _enemySearchRange, _enemyLayer);
        var sortedTargets = hits.OrderBy(h => Vector2.Distance(origin, h.transform.position))
                                .Take(_data.projectileCount);

        foreach (var target in sortedTargets)
        {
            GameObject projGO = Instantiate(_fireballProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<FireballProjectile>();

            if (proj != null)
            {
                Vector2 dir = ((Vector2)target.transform.position - origin).normalized;

                proj.Initialize(dir, _projectileSpeed, _projectileDistance, CalculateDamage(_data.skillDamage), _data.explosionRadius);
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
