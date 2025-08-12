using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagingEmotions2Wrapper : UpgradeSkillWrapper
{
    public float playerDistance;
    public float projRadius;
    
    public float knockbackDistance = 4f;
    public float damageInterval = 0.3f;
    public float rotationSpeed = 120f;
    
   
}

public class RagingEmotions2 : UpgradeSkill<RagingEmotions2Wrapper>
{
    [SerializeField] private float _playerDistance = 2.5f;
    [SerializeField] private float _projRadius = 1f;
    
    
    private Coroutine _sfxLoopCo;                 // 🔹 SFX 루프 코루틴 핸들
    private bool _skillActive;                    // 🔹 스킬 활성 상태
    
    [Header("References")]
    [SerializeField] private LayerMask _enemyLayer;
    
    private Coroutine _activeCoroutine;
    private List<RagingEmotions2Proj> _spawnedProjectiles = new List<RagingEmotions2Proj>();
    
    public override void TryActivate()
    {
        if (SkillManager.Instance.isLevelUp == false) return;
        
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        
            foreach (var proj in _spawnedProjectiles)
            {
                if (proj != null)
                    proj.ReturnToPool();
            }
            _spawnedProjectiles.Clear();
        }
            
        UpdateActiveData();
        Activate();

        SkillManager.Instance.isLevelUp = false;
    }
    
    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        data.projectileCount = UpgradeData.projectileCount;
        
        data.playerDistance = _playerDistance * player.stat.FinalSkillRange;
        data.projRadius = _projRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        _skillActive = true; // ✅ 스킬 활성 플래그 ON (먼저!)
    
        // SFX 루프는 하나만
        if (_sfxLoopCo == null)
            _sfxLoopCo = StartCoroutine(SfxLoop(2f));
        
        _activeCoroutine = StartCoroutine(SkillCoroutine());
    }
    
    private IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = (360f / data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            Vector3 spawnPos = player.transform.position + spawnDir * data.playerDistance;

            //GameObject go = Instantiate(_RagingEmotionsProjPrefab, spawnPos, Quaternion.identity);
            GameObject go = ObjectPoolManager.Instance.GetFromPool(PoolType.RagingEmotions2Proj, spawnPos, Quaternion.identity);
            RagingEmotions2Proj projectile = go.GetComponent<RagingEmotions2Proj>();
            projectile.Initialize(player.transform, angle, data, _enemyLayer);
            _spawnedProjectiles.Add(projectile);
        }

        yield return null;
    }
    
    private void StopSfxLoop()
    {
        _skillActive = false;
        if (_sfxLoopCo != null)
        {
            StopCoroutine(_sfxLoopCo);
            _sfxLoopCo = null;
        }
    }

    private IEnumerator SfxLoop(float interval)
    {
        // 스킬이 활성화된 동안 2초마다 1번만 재생
        while (_skillActive)
        {
            SoundManager.Instance.PlayRandomSFX(SoundCategory.UpgradeRagingEmotionsProjectile);
            yield return new WaitForSeconds(interval);
        }
        _sfxLoopCo = null;
    }
    
    private void Deactivate()
    {
        _skillActive = false; // ✅ 플래그 off
        if (_activeCoroutine != null) { StopCoroutine(_activeCoroutine); _activeCoroutine = null; }
        StopSfxLoop();

        foreach (var proj in _spawnedProjectiles)
            if (proj != null) proj.ReturnToPool();
        _spawnedProjectiles.Clear();
    }

    
    
    private void OnDisable()
    {
        // 오브젝트 비활성화 시 정리
        Deactivate();
    }
}
