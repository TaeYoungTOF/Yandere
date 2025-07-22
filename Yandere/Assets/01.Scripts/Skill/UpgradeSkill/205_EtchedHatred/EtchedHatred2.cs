using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtchedHatred2Wrapper : UpgradeSkillWrapper
{
    public float searchRadius;
    public float explosionRadius;
    
    public float infectChance = 30f;
    public float searchTime = 1.7f;
}

public class EtchedHatred2 : UpgradeSkill<EtchedHatred2Wrapper>
{
    [SerializeField] private float _searchRadius = 8f;
    [SerializeField] private float _explosionRadius = 3f;

    [Header("References")]
    [SerializeField] private GameObject _wavePrefab;
    [SerializeField] private GameObject _etchedHatredProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;
    
    public readonly HashSet<Transform> attachedEnemies = new();
    
    public override void UpdateActiveData()
    {
        base.UpdateActiveData();
        
        data.searchRadius = _searchRadius * player.stat.FinalSkillRange;
        data.explosionRadius = _explosionRadius * player.stat.FinalSkillRange;
    }

    protected override void Activate()
    {
        attachedEnemies.Clear();
        _wavePrefab.transform.localScale = Vector3.one * data.searchRadius;
        _wavePrefab.SetActive(true);

        StartCoroutine(FindEnemies());
    }

    private IEnumerator FindEnemies()
    {
        float timer = 0;

        while (timer < data.searchTime)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, data.searchRadius, _enemyLayer);

            foreach (var e in enemies)
            {
                if (!attachedEnemies.Contains(e.transform) && e.TryGetComponent(out IDamagable target))
                {
                    Vector3 spawnPos = e.transform.position;

                    GameObject go = Instantiate(_etchedHatredProjectilePrefab, spawnPos, Quaternion.identity);
                    EtchedHatred2Proj projectile = go.GetComponent<EtchedHatred2Proj>();
                    projectile.Initialize(data, _enemyLayer, e.transform, attachedEnemies);

                    attachedEnemies.Add(e.transform); // 중복 방지용으로 등록
                }
            }
            
            timer += Time.deltaTime;
            yield return null;
        }

        RemoveWave();
    }

    private void RemoveWave()
    {
        _wavePrefab.SetActive(false);
    }
    
}
