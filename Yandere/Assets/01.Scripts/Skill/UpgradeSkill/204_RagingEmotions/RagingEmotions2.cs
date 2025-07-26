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
    
    [Header("References")]
    [SerializeField] private GameObject _RagingEmotionsProjPrefab;
    [SerializeField] private LayerMask _enemyLayer;
    
    private Coroutine _activeCoroutine;
    private List<GameObject> _spawnedProjectiles = new List<GameObject>();
    
    public override void TryActivate()
    {
        if (SkillManager.Instance.isLevelUp)
        {
            UpdateActiveData();
            Activate();

            SkillManager.Instance.isLevelUp = false;
        }
    }
    
    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        data.playerDistance = _playerDistance * player.stat.FinalSkillRange;
        data.projRadius = _projRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        _activeCoroutine = StartCoroutine(SkillCoroutine());
    }
    
    private IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (_activeCoroutine != null)
        {
            Debug.Log("호출됨");
        
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        
            foreach (var proj in _spawnedProjectiles)
            {
                if (proj != null)
                    Destroy(proj);
            }
            _spawnedProjectiles.Clear();
        }
        
        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = (360f / data.projectileCount) * i;
            Vector3 spawnDir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            Vector3 spawnPos = player.transform.position + spawnDir * data.playerDistance;

            GameObject go = Instantiate(_RagingEmotionsProjPrefab, spawnPos, Quaternion.identity);
            RagingEmotions2Proj projectile = go.GetComponent<RagingEmotions2Proj>();
            projectile.Initialize(player.transform, angle, data, _enemyLayer);
            _spawnedProjectiles.Add(go);
            
            projectile.transform.localScale = Vector3.one * data.projRadius;
        }

        yield return null;
    }
}
