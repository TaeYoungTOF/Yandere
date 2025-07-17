using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EtchedHatredDataWrapper : AcviteDataWapper
{
    [Header("Leveling Data")]
    public float searchRadius;
    
    [Header("UnLeveling Data")]
    public float explosionRadius;

    [Header("Const Data")] 
    public float searchTime = 1.7f;
}

public class EtchedHatred : ActiveSkill<EtchedHatredDataWrapper>
{
    private LevelupData_EtchedHatred CurrentData => ActiveData as LevelupData_EtchedHatred;
    [SerializeField] private float _explosionRadius = 2.5f;

    [Header("References")]
    [SerializeField] private GameObject _wavePrefab;
    [SerializeField] private GameObject _etchedHatredProjectilePrefab;
    [SerializeField] private LayerMask _enemyLayer;
    
    private readonly HashSet<Transform> _attachedEnemies = new();

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
                if (!_attachedEnemies.Contains(e.transform) && e.TryGetComponent(out IDamagable target))
                {
                    Vector3 spawnPos = e.transform.position;

                    GameObject go = Instantiate(_etchedHatredProjectilePrefab, spawnPos, Quaternion.identity);
                    EtchedHatredProjectile projectile = go.GetComponent<EtchedHatredProjectile>();
                    projectile.Initialize(data, _enemyLayer, e.transform);

                    _attachedEnemies.Add(e.transform); // 중복 방지용으로 등록
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
