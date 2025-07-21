using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParchedLonging2Proj : BaseProjectile
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject _secondProjPrefab;
    
    private ParchedLonging2Wrapper _data;
    private Dictionary<Collider2D, float> _lastHitTimes = new();

    public override void Initialize() { }
    public void Initialize(ParchedLonging2Wrapper data, LayerMask enemyLayer)
    {
        blackholePrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
        _lastHitTimes.Clear();
        
        _data = data;
        this.enemyLayer = enemyLayer;

        StartCoroutine(Explode());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        if (!other.TryGetComponent(out IDamagable target)) return;
        
        if (target == null) return;

        float lastTime = _lastHitTimes.ContainsKey(other) ? _lastHitTimes[other] : -999f;
        if (Time.time - lastTime >= _data.damageInterval)
        {
            pullEnemy(other);
            target.TakeDamage(_data.damageDoT);
            _lastHitTimes[other] = Time.time;
        }
    }
    
    private void pullEnemy(Collider2D enemy)
    {
        if (!enemy.CompareTag("Enemy_Normal")) return;
        if (enemy.attachedRigidbody is null) return;

        Vector2 knockbackDir = (transform.position - enemy.transform.position).normalized;
        Vector2 targetPos = (Vector2)enemy.transform.position + knockbackDir;

        enemy.attachedRigidbody.DOMove(targetPos, 0.2f)
            .SetEase(Ease.OutQuad)
            .SetLink(enemy.gameObject);
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_data.duration);
        
        blackholePrefab.SetActive(false);
        explosionPrefab.SetActive(true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.projRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }
        
        GameObject go = Instantiate(_secondProjPrefab, transform.position, Quaternion.identity);
        ParchedLonging2Proj2 proj = go.GetComponent<ParchedLonging2Proj2>();
        proj.Initialize(_data, enemyLayer);
        proj.transform.localScale = Vector3.one * _data.secondProjSize;
        
        //explosionPrefab.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        //Destroy(gameObject, _data.secondDuration + 2f);
        Destroy(gameObject, 0.5f);
    }
}
