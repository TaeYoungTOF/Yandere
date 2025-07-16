using System.Linq;
using UnityEngine;

[System.Serializable]
public class ParchedLongingDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float pullRange;                         // 블랙홀은 반지름 value 안의 적을 끌어당김
    public float duration;                          // 블랙홀은 value초 동안 적을 끌어당김
    public float explosionRange;                    // 블랙홀은 일정 시간 후 value 안의 적에게 피해를 입힘
    
    [Header("UnLeveling Data")]
    public float damageDoT;                         // 일정 시간마다 value의 데미지를 줌

    [Header("Const Data")]
    public readonly float damageInterval = 0.5f;    // value초 마다 일정 데미지를 줌
}

public class ParchedLonging : ActiveSkill<ParchedLongingDataWrapper>
{
    private LevelupData_ParchedLonging CurrentData => ActiveData as LevelupData_ParchedLonging;
    [SerializeField] private float _damageDoT = 10f;

    [Header("References")]
    [SerializeField] private GameObject _parchedLongingProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        // Leveling Data
        data.duration = CurrentData.duration;
        data.explosionRange = CurrentData.explosionRange;
        
        // UnLeveling Data
        data.damageDoT = CalculateDamage(_damageDoT);
    }

    protected override void Activate()
    {
        
    }
}
