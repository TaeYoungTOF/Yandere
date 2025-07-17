using System.Linq;
using UnityEngine;

[System.Serializable]
public class PouringAffectionDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float explosionRadius;
    
    //[Header("UnLeveling Data")]
    
    //[Header("Const Data")]
}

public class PouringAffection : ActiveSkill<PouringAffectionDataWrapper>
{
    private LevelupData_PouringAffection CurrentData => ActiveData as LevelupData_PouringAffection;

    [Header("References")]
    [SerializeField] private GameObject _pouringAffectionProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.explosionRadius = CurrentData.explosionRadius * player.stat.FinalSkillRange;

        // UnLeveling Data
    }

    protected override void Activate()
    {
    }
}
