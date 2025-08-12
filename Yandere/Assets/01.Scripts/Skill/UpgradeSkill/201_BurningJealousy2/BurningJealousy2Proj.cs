using DG.Tweening;
using UnityEngine;

public class BurningJealousy2Proj : BaseProjectile
{
    private Vector2 _direction;
    private BurningJealousy2Wrapper _data;
    private Tween _moveTween;
    private bool _hasExploded;
    
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public override void Initialize() { }
    public void Initialize(Vector2 direction, BurningJealousy2Wrapper data, LayerMask enemyLayer)
    {
        fireballPrefab.SetActive(true);
        explosionPrefab.SetActive(false);
        
        _direction = direction;
        _data = data;
        this.enemyLayer = enemyLayer;
        _hasExploded = false;

        transform.localScale = Vector3.one * _data.pjtSize;

        Vector3 targetPos = transform.position + (Vector3)(_direction * _data.pjtDistance);
        _moveTween = transform.DOMove(targetPos, _data.pjtDistance / _data.pjtSpeed)
                              .SetEase(Ease.Linear)
                              .OnComplete(Explode);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;
        
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        
        _moveTween?.Kill();
        
        fireballPrefab.SetActive(false);
        explosionPrefab.SetActive(true);
        
        transform.localScale = Vector3.one * _data.explodeRadius;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _data.explodeRadius, enemyLayer);

        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out IDamagable target))
            {
                target.TakeDamage(_data.skillDamage);
            }
        }
        
        for (int i = 0; i < _data.secondPjtCount; i++)
        {
            float angle = (360f / _data.secondPjtCount) * i;
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            GameObject projGO = ObjectPoolManager.Instance.GetFromPool(PoolType.BurningJealousy2Proj2, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayRandomSFX(SoundCategory.UpgradeFireProjectile);
            var proj = projGO.GetComponent<BurningJealousy2Proj2>();
            proj.Initialize(dir, _data, enemyLayer);
        }

        Invoke(nameof(ReturnToPool), 1f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.BurningJealousy2Proj, gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_data == null) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f); // 주황색 반투명
        Gizmos.DrawSphere(transform.position, _data.explodeRadius);
    }
#endif
}
