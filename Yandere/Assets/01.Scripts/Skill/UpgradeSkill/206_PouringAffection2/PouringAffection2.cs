using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringAffection2Wrapper : UpgradeSkillWrapper
{
    public float explodeRadius;
    public float secondExplodeRadius;
    public float spawnDuration;
    
    public float spawnInterval = 0.4f;
}

public class PouringAffection2 : UpgradeSkill<PouringAffection2Wrapper>
{
    [SerializeField] private float _explodeRadius = 14f;
    [SerializeField] private float _secondExplodeRadius = 4f;
    [SerializeField] private float _spawnDuration = 2f;
    
    [Header("References")]
    [SerializeField] private GameObject _projPrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        data.explodeRadius =_explodeRadius * player.stat.FinalSkillRange;
        data.secondExplodeRadius = _secondExplodeRadius * player.stat.FinalSkillRange;
        data.spawnDuration = _spawnDuration * player.stat.FinalSkillDuration;
    }

    protected override void Activate()
    {
        Vector2 spawnPosition = new(Random.Range(-6, 6), Random.Range(-10, 10));
            
        GameObject go = Instantiate(_projPrefab, spawnPosition, Quaternion.identity);
        PouringAffection2Proj projectile = go.GetComponent<PouringAffection2Proj>();
        projectile.Initialize(data, _enemyLayer);

        projectile.transform.localScale = Vector3.one * data.explodeRadius;
    }
}
