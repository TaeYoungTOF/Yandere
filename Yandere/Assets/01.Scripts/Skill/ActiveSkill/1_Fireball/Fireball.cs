using System.Linq;
using UnityEngine;

[System.Serializable]
public class FireballDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float projectileSize;
    public float explosionRadius;
    
    [Header("UnLeveling Data")]
    public float projectileDistance;
    public float enemySearchRange;
    
    [Header("Const Data")]
    public readonly float projectileSpeed = 15f;
}

public class Fireball : ActiveSkill<FireballDataWrapper>
{
    private LevelupData_Fireball CurrentData => ActiveData as LevelupData_Fireball;
    [SerializeField] private float _projectileDistance = 30f;
    [SerializeField] private float _enemySearchRange = 5f;

    [Header("References")]
    [SerializeField] private GameObject _fireballProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.projectileSize = CurrentData.projectileSize * player.stat.FinalSkillRange;
        data.explosionRadius = CurrentData.explosionRadius * player.stat.FinalSkillRange;

        // UnLeveling Data
        data.projectileDistance = _projectileDistance * player.stat.FinalSkillRange;
        data.enemySearchRange = _enemySearchRange * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        if (data == null || _fireballProjectilePrefab == null)
        {
            Debug.LogWarning("[Fireball] Data or Projectile Prefab is null");
            return;
        }

        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, data.enemySearchRange, _enemyLayer);
        var sortedTargets = hits.OrderBy(h => Vector2.Distance(origin, h.transform.position))
                                .Take(data.projectileCount);

        foreach (var target in sortedTargets)
        {
            GameObject projGO = Instantiate(_fireballProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<FireballProjectile>();

            if (proj != null)
            {
                Vector2 dir = ((Vector2)target.transform.position - origin).normalized;

                proj.Initialize(dir, data.projectileSpeed, data.projectileDistance, data.skillDamage, data.explosionRadius);
            }

            // 투사체 크기 조절
            projGO.transform.localScale = Vector3.one * data.projectileSize;
        }
    }
}
