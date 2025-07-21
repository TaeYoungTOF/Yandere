using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BurstingGaze2Pjt : BaseProjectile
{
    private Vector2 _dir;
    private BurstingGaze2Wrapper _data;
    
    private Dictionary<Collider2D, float> _lastHitTimes = new();
    
    public override void Initialize() { }
    public void Initialize(Vector2 dir, BurstingGaze2Wrapper data, LayerMask enemyLayer)
    {
        _dir = dir.normalized;
        _data = data;
        this.enemyLayer = enemyLayer;

        Vector3 targetPos = transform.position + (Vector3)(_dir * _data.projectileDistance);
        transform.DOMove(targetPos, _data.secondPjtDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        if (!other.TryGetComponent(out IDamagable target)) return;
        
        if (target == null) return;

        float lastTime = _lastHitTimes.ContainsKey(other) ? _lastHitTimes[other] : -999f;
        if (Time.time - lastTime >= _data.dmgInterval)
        {
            target.TakeDamage(_data.secondSkillDmg);
            _lastHitTimes[other] = Time.time;
        }
    }
}
