using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RagingEmotionsDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float skillDuration;
    public float knockbackDistance;
    
    [Header("UnLeveling Data")]
    public float playerDistance;  // player로부터 value만큼 떨어져서 공전
    public float projectileRadius;  // 투사체는 value의 반지름을 보유
    
    [Header("Const Data")]
    public float damageInterval = 0.3f;  // value초 마다 데미지
    public float rotationSpeed= 120f;   // 초당 value degree각 만큼 회전
}

public class RagingEmotions : ActiveSkill<RagingEmotionsDataWrapper>
{
    private LevelupData_RagingEmotions CurrentData => ActiveData as LevelupData_RagingEmotions;
    [SerializeField] private float _playerDistance = 2.5f;
    [SerializeField] private float _projectileRadius = 1f;
    
    [Header("References")]
    //[SerializeField] private GameObject _RagingEmotionsProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;

    private bool _isSkillOver = true;

    public override void UpdateCooldown()
    {
        if (_isSkillOver && coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;
        }
    }

    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        // Leveling Data
        data.skillDuration = CurrentData.skillDuration * player.stat.FinalSkillDuration;
        data.knockbackDistance = CurrentData.knockbackDistance;

        // UnLeveling Data
        data.playerDistance = _playerDistance * player.stat.FinalSkillRange;
        data.projectileRadius = _projectileRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        StartCoroutine(SkillDurationCoroutine());
    }

    private IEnumerator SkillDurationCoroutine()
    {
        _isSkillOver = false;
        
        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = (360f / data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            Vector3 spawnPos = player.transform.position + spawnDir * data.playerDistance;

            //GameObject go = Instantiate(_RagingEmotionsProjectilePrefab, spawnPos, Quaternion.identity);
            GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.RagingEmotionsProj, spawnPos, Quaternion.identity);
            RagingEmotionsProjectile projectile = go.GetComponent<RagingEmotionsProjectile>();
            SoundManager.Instance.PlayRandomSFX(SoundCategory.RagingEmotionsProjectile);
            projectile.Initialize(player.transform, angle, data, _enemyLayer);
        }
        
        yield return new WaitForSeconds(data.skillDuration);
        
        _isSkillOver = true;
    }
}
