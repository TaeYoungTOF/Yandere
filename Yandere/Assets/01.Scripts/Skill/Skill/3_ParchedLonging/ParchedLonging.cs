using System.Linq;
using UnityEngine;

[System.Serializable]
public class ParchedLongingDataWrapper : AcviteDataWapper
{
    public float duration;
    public float explosionRange;
}

public class ParchedLonging : ActiveSkill
{
    private LevelupData_ParchedLonging CurrentData => ActiveData as LevelupData_ParchedLonging;

    [SerializeField] private ParchedLongingDataWrapper _data;
    [SerializeField] private GameObject _parchedLongingProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    //[Header("Unupgradable Data")]
    //[SerializeField] private float _damageDoT = 10f;

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
            coolDownTimer = _data.coolTime;
        }
    }

    public override void UpdateActiveData()
    {
        _data.projectileCount = CurrentData.projectileCount + player.stat.ProjectileCount;
        _data.skillDamage = CurrentData.skillDamage;
        _data.coolTime = CurrentData.coolTime * (1 - player.stat.CoolDown / 100f);

        _data.duration = CurrentData.duration;
        _data.explosionRange = CurrentData.explosionRange;
    }

    protected override void Activate()
    {
    }
}
