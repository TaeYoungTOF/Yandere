using System.Linq;
using UnityEngine;

[System.Serializable]
public class EtchedHatredDataWrapper : AcviteDataWapper
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

public class EtchedHatred : ActiveSkill<EtchedHatredDataWrapper>
{
    private LevelupData_EtchedHatred CurrentData => ActiveData as LevelupData_EtchedHatred;
    [SerializeField] private float _projectileDistance = 30f;
    [SerializeField] private float _enemySearchRange = 5f;

    [Header("References")]
    [SerializeField] private GameObject _fireballProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

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
            coolDownTimer = data.coolTime;
        }
    }

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
    }
}
