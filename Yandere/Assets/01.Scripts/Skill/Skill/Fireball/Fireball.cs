using System.Linq;
using UnityEngine;

public class Fireball : ActiveSkill
{
    private LevelupData_Fireball _currentData => ActiveData as LevelupData_Fireball;

    [SerializeField] private GameObject _fireballProjectilePrefab;

    [SerializeField] private LayerMask _enemyLayer;

    protected override void Activate()
    {
        base.Activate();

        if (_currentData == null || _fireballProjectilePrefab == null)
        {
            Debug.LogWarning("[Fireball] Data or Projectile Prefab is null");
            return;
        }

        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, 20f, _enemyLayer);
        var sortedTargets = hits.OrderBy(h => Vector2.Distance(origin, h.transform.position))
                                .Take(_currentData.projectileCount);

        foreach (var target in sortedTargets)
        {
            GameObject projGO = Instantiate(_fireballProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<FireballProjectile>();

            if (proj != null)
            {
                Vector2 dir = ((Vector2)target.transform.position - origin).normalized;
                proj.Initialize(_currentData, dir);
            }

            // 투사체 크기 조절
            projGO.transform.localScale = Vector3.one * _currentData.projectileSize;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}
