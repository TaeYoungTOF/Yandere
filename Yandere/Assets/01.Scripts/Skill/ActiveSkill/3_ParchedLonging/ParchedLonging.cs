using System.Linq;
using UnityEngine;

[System.Serializable]
public class ParchedLongingDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float projectileRadius;                  // 블랙홀은 value 반지름을 갖음
    
    [Header("UnLeveling Data")]
    public float duration;                          // 블랙홀은 value초 동안 적을 끌어당김
    public float damageDoT;                         // 일정 시간마다 value의 데미지를 줌

    [Header("Const Data")]
    public readonly float playerDistance = 3f;      // 플레이어로부터 value 만큼 떨어진 곳에 블랙홀 생성
    public readonly float damageInterval = 0.5f;    // value초 마다 일정 데미지를 줌
}

public class ParchedLonging : ActiveSkill<ParchedLongingDataWrapper>
{
    private LevelupData_ParchedLonging CurrentData => ActiveData as LevelupData_ParchedLonging;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _damageDoT = 10f;

    [Header("References")]
    //[SerializeField] private GameObject _parchedLongingProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        // Leveling Data
        data.projectileCount = CurrentData.projectileCount;
        data.projectileRadius = CurrentData.projectileRadius * player.stat.FinalSkillRange;
        
        // UnLeveling Data
        data.duration = _duration * player.stat.FinalSkillDuration;
        data.damageDoT = CalculateDamage(_damageDoT);
    }

    protected override void Activate()
    {
        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = (360f / data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
            Vector3 spawnPos = player.transform.position + spawnDir * data.playerDistance;

            //GameObject go = Instantiate(_parchedLongingProjectilePrefab, spawnPos, Quaternion.identity);
            GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.ParchedLongingProj, spawnPos, Quaternion.identity);
            SoundManager.Instance.PlayRandomSFX(SoundCategory.ParchedLongingProjectile);
            ParchedLongingProjectile projectile = go.GetComponent<ParchedLongingProjectile>();
            projectile.Initialize(data, _enemyLayer);
        }
        
    }
}
