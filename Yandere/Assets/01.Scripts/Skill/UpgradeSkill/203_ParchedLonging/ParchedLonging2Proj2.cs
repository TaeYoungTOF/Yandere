using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchedLonging2Proj2 : BaseProjectile
{
    private ParchedLonging2Wrapper _data;
    private Dictionary<Collider2D, float> _lastHitTimes = new();
    
    public override void Initialize() { }
    public void Initialize(ParchedLonging2Wrapper data, LayerMask enemyLayer)
    {
        _lastHitTimes.Clear();
        
        _data = data;
        this.enemyLayer = enemyLayer;

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_data.secondDuration);
        
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        if (!other.TryGetComponent(out IDamagable target)) return;
        
        if (target == null) return;

        float lastTime = _lastHitTimes.ContainsKey(other) ? _lastHitTimes[other] : -999f;
        if (Time.time - lastTime >= _data.damageInterval)
        {
            target.TakeDamage(_data.secondDmg);
            _lastHitTimes[other] = Time.time;
        }
    }
}
