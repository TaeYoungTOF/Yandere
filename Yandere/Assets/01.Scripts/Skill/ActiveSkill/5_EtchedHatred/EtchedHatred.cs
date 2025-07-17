using System.Linq;
using UnityEngine;

[System.Serializable]
public class EtchedHatredDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float searchRadius;
    
    [Header("UnLeveling Data")]
    public float explosionRadius;
}

public class EtchedHatred : ActiveSkill<EtchedHatredDataWrapper>
{
    private LevelupData_EtchedHatred CurrentData => ActiveData as LevelupData_EtchedHatred;
    [SerializeField] private float _explosionRadius = 2.5f;

    [Header("References")]
    [SerializeField] private GameObject _etchedHatredProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.searchRadius = CurrentData.searchRadius * player.stat.FinalSkillRange;

        // UnLeveling Data
        data.explosionRadius = _explosionRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
    }
}
