using System.Collections;
using UnityEngine;

[System.Serializable]
public class RagingEmotionsDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float skillDuration;
    public float knockbackDistance;
    
    [Header("UnLeveling Data")]
    public float playerDistance;
    public float projectileRadius;
}

public class RagingEmotions : ActiveSkill
{
    private LevelupData_RagingEmotions CurrentData => ActiveData as LevelupData_RagingEmotions;

    [SerializeField] private RagingEmotionsDataWrapper _data;
    [SerializeField] private GameObject _RagingEmotionsProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("UnLeveling Data")]
    [SerializeField] private float _playerDistance = 2.5f;  // player로부터 value만큼 떨어져서 공전
    [SerializeField] private float _projectileRadius = 1f;  // 투사체는 value의 반지름을 보유
    
    [Header("Const Data")]
    [SerializeField] private float _damageInterval = 0.3f;  // value초 마다 데미지
    [SerializeField] private float _rotationSpeed = 120f;   // 초당 value degree각 만큼 회전

    private bool _isSkillOver = true;

    public override void UpdateCooldown()
    {
        if (!_isSkillOver && coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;

            _isSkillOver = coolDownTimer <= 0f;
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
        // Active Skill Data
        _data.projectileCount = CurrentData.projectileCount + player.stat.ProjectileCount;
        _data.skillDamage = CurrentData.skillDamage;
        _data.coolTime = CurrentData.coolTime * (1 - player.stat.CoolDown / 100f);

        // Leveling Data
        _data.skillDuration = CurrentData.skillDuration * player.stat.FinalSkillDuration;
        _data.knockbackDistance = CurrentData.knockbackDistance;

        // UnLeveling Data
        _data.playerDistance = _playerDistance * player.stat.FinalSkillRange;
        _data.projectileRadius = _projectileRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        _isSkillOver = false;

        for (int i = 0; i < _data.projectileCount; i++)
        {
            float angle = (360f / _data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            Vector3 spawnPos = player.transform.position + spawnDir * _data.playerDistance;

            GameObject go = Instantiate(_RagingEmotionsProjectilePrefab, spawnPos, Quaternion.identity);
            RagingEmotionsProjectile projectile = go.GetComponent<RagingEmotionsProjectile>();
            projectile.Initialize(player.transform, angle, _data);
        }

        StartCoroutine(SkillDurationCoroutine());
    }

    private IEnumerator SkillDurationCoroutine()
    {
        yield return new WaitForSeconds(_data.skillDuration);
        _isSkillOver = true;
    }
}
