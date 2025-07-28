using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagingEmotionsProjectile : BaseProjectile
{
    [SerializeField] private GameObject[] _projectilePrefabs;
    
    private float _currentAngle;
    private RagingEmotionsDataWrapper _data;
    private Transform _player;
    
    private Dictionary<Collider2D, float> _lastHitTimes = new();

    
    public override void Initialize() { }
    public void Initialize(Transform player, float startAngle, RagingEmotionsDataWrapper data, LayerMask enemyLayer)
    {
        _player = player;
        _currentAngle = startAngle;
        _data = data;
        this.enemyLayer = enemyLayer;
            
        transform.localScale = Vector3.one * _data.projectileRadius;

        // 랜덤하게 하나의 projectile prefab 활성화
        int randomIndex = Random.Range(0, _projectilePrefabs.Length);
        for (int i = 0; i < _projectilePrefabs.Length; i++)
        {
            _projectilePrefabs[i].SetActive(i == randomIndex);
        }
        
        // 시작 위치 설정
        UpdatePosition();

        // 공전 애니메이션 시작
        RotateAroundPlayer(data.skillDuration);
    }

    private void UpdatePosition()
    {
        if (!_player) return;

        Vector3 offset = Quaternion.Euler(0f, 0f, _currentAngle) * Vector3.right * _data.playerDistance;
        transform.position = _player.position + offset;
    }

    private void RotateAroundPlayer(float duration)
    {
        DOTween.To(() => _currentAngle, x =>
            {
                _currentAngle = x;
                UpdatePosition();
            }, _currentAngle + 360f, 360f / _data.rotationSpeed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        StartCoroutine(EndAfterDuration(duration));
    }

    private IEnumerator EndAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        DOTween.Kill(this);
        ObjectPoolManager.Instance.ReturnToPool(PoolType.RagingEmotionsProj, gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        if (!other.TryGetComponent(out IDamagable target)) return;
        
        if (target == null) return;

        float lastTime = _lastHitTimes.ContainsKey(other) ? _lastHitTimes[other] : -999f;
        if (Time.time - lastTime >= _data.damageInterval)
        {
            target.TakeDamage(_data.skillDamage);
            _lastHitTimes[other] = Time.time;

            if (other.CompareTag("Enemy_Normal"))
            {
                ApplyKnockback(other);
            }
        }
    }
    
    private void ApplyKnockback(Collider2D enemy)
    {
        if (enemy.attachedRigidbody is null) return;

        Vector2 knockbackDir = (enemy.transform.position - _player.position).normalized;
        Vector2 targetPos = (Vector2)enemy.transform.position + knockbackDir * _data.knockbackDistance;

        enemy.attachedRigidbody.DOMove(targetPos, 0.2f)
            .SetEase(Ease.OutQuad)
            .SetLink(enemy.gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_player == null) return;

        // 회전 반경 표시 (회전 궤도)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_player.position, _data.playerDistance);

        // 현재 위치까지의 offset 벡터
        Gizmos.color = Color.yellow;
        Vector3 offset = Quaternion.Euler(0f, 0f, _currentAngle) * Vector3.right * _data.playerDistance;
        Gizmos.DrawLine(_player.position, _player.position + offset);

        // 회전 방향 (시계방향 기준)
        Gizmos.color = Color.red;
        Vector3 tangentDir = Quaternion.Euler(0f, 0f, _currentAngle + 90f) * Vector3.right;
        Vector3 arrowStart = _player.position + offset;
        Vector3 arrowEnd = arrowStart + tangentDir.normalized * 0.5f;
        Gizmos.DrawLine(arrowStart, arrowEnd);

        // 화살촉
        Vector3 headLeft = Quaternion.Euler(0, 0, -20) * -tangentDir.normalized * 0.2f;
        Vector3 headRight = Quaternion.Euler(0, 0, 20) * -tangentDir.normalized * 0.2f;
        Gizmos.DrawLine(arrowEnd, arrowEnd + headLeft);
        Gizmos.DrawLine(arrowEnd, arrowEnd + headRight);
    }
#endif
}