using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchedLonging2Wrapper : UpgradeSkillWrapper
{
    public float projRadius;
    public float duration;
    public float damageDoT;
    public float playerDistance;
    
    public float damageInterval = 0.5f;
    
    [Header("Second Data")]
    public float secondDuration;
    public float secondDmg;
    public float secondProjSize;
}

public class ParchedLonging2 : UpgradeSkill<ParchedLonging2Wrapper>
{
    [SerializeField] private float _projRadius = 5f;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _damageDoT = 10f;
    [SerializeField] private float _playerDistance = 3f;
    
    [SerializeField] private float _secondDuration = 4f;
    [SerializeField] private float _secondDmg = 90f;
    [SerializeField] private float _secondProjSize = 4f;

    [Header("References")]
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        data.projRadius = _projRadius * player.stat.FinalSkillRange;
        data.duration = _duration * player.stat.FinalSkillDuration;
        data.damageDoT = CalculateDamage(_damageDoT);
        data.playerDistance =  _playerDistance * player.stat.FinalSkillRange;
        
        data.secondDuration = _secondDuration * player.stat.FinalSkillDuration;
        data.secondDmg = CalculateDamage(_secondDmg);
        data.secondProjSize = _secondProjSize * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = (360f / data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
            Vector3 spawnPos = player.transform.position + spawnDir * data.playerDistance;

            GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.ParchedLonging2Proj, spawnPos, Quaternion.identity);
            ParchedLonging2Proj proj = go.GetComponent<ParchedLonging2Proj>();
            proj.Initialize(data, _enemyLayer);
        }
    }
}
