using DG.Tweening;
using UnityEngine;

public class RagingEmotionsProjectile : MonoBehaviour
{
    [SerializeField] private GameObject[]_projectilePrefabs;
    
    private Transform _pivot;
    private float _currentAngle;
    private float _distance;
    private float _rotationSpeed;
    private Transform _player;

    public void Initialize(Transform player, float startAngle, RagingEmotionsDataWrapper data)
    {
        _player = player;
        _currentAngle = startAngle;
        _distance = data.playerDistance;
        _rotationSpeed = 360f / (360f / data.projectileCount) * data.projectileCount; // Ensure full rotation

        // 랜덤하게 하나의 projectile prefab 활성화
        int randomIndex = Random.Range(0, _projectilePrefabs.Length);
        for (int i = 0; i < _projectilePrefabs.Length; i++)
        {
            _projectilePrefabs[i].SetActive(i == randomIndex);
        }
        
        // 시작 위치 설정
        UpdatePosition();

        // 공전 애니메이션 시작
        RotateAroundPlayer(data.skillDuration, data.projectileCount);
    }

    private void RotateAroundPlayer(float duration, int projectileCount)
    {
        DOTween.To(() => _currentAngle, x => {
                _currentAngle = x;
                UpdatePosition();
            }, _currentAngle + 360f, 360f / _rotationSpeed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetId(this) // 나중에 개별 삭제 위해 ID 지정 가능
            .OnKill(() => Destroy(gameObject));
    }

    private void UpdatePosition()
    {
        if (_player == null) return;

        Vector3 offset = Quaternion.Euler(0f, 0f, _currentAngle) * Vector3.right * _distance;
        transform.position = _player.position + offset;
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}