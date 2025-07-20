using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurningJealousy2Wrapper : UpgradeSkillWrapper
{
    [Header("Unleveling Data")]
    public float enemySearchRange;
    public float pjtDistance;
    public float explodeRadius;
    public float pjtSize;
    public float secondDmg;
    public float secondExplodeRadius;

    [Header("Const Data")]
    public float pjtSpeed = 15f;
    public int secondPjtCount = 8;
    public float secondPjtSize = 0.6f;
}

public class BurningJealousy2 : UpgradeSkill<BurningJealousy2Wrapper>
{
    [SerializeField] private float _enemySearchRange = 5f;
    [SerializeField] private float _pjtDistance = 30f;
    [SerializeField] private float _explodeRadius = 4f;
    [SerializeField] private float _pjtSize = 0.8f;
    [SerializeField] private float _secondDmg = 25f;
    [SerializeField] private float _secondExplodeRadius = 3f;

    [Header("References")]
    [SerializeField] private GameObject _fireballProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;
    
    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        data.enemySearchRange = _enemySearchRange * player.stat.FinalSkillRange;
        data.pjtDistance = _pjtDistance * player.stat.FinalSkillRange;
        data.explodeRadius =  _explodeRadius * player.stat.FinalSkillRange;
        data.pjtSize = _pjtSize * player.stat.FinalSkillRange;
        
        data.secondDmg = CalculateDamage(_secondDmg);
        data.secondExplodeRadius = _secondExplodeRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        Debug.Log("BurningJeoulousy2 is active");

        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, data.enemySearchRange, _enemyLayer);
        var sortedTargets = hits.OrderBy(h => Vector2.Distance(origin, h.transform.position))
            .Take(data.projectileCount);

        foreach (var target in sortedTargets)
        {
            Vector2 dir = ((Vector2)target.transform.position - origin).normalized;
            
            GameObject projGO = Instantiate(_fireballProjectilePrefab, origin, Quaternion.identity);
            var proj = projGO.GetComponent<BurningJealousy2Projectile>();
            proj.Initialize(dir, data, _enemyLayer);

            // 투사체 크기 조절
            projGO.transform.localScale = Vector3.one * data.pjtSize;
        }
    }
}
