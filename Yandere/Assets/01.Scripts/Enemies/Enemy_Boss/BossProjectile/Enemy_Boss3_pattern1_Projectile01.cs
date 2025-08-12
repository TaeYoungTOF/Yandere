using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy_Boss3_pattern1_Projectile01 : MonoBehaviour
{
    [Header("총알 관련 설정")]
    [SerializeField] private float bulletDamage = 150;
    [SerializeField] private float duration = 1f;
    [SerializeField] private LayerMask targetLayer;

    [Header("레이저 판정")]
    [SerializeField] private Vector2 boxCastSize = new Vector2(0.5f, 5f);
    [SerializeField] private float boxCastDistance = 0.1f;
    [SerializeField] private Vector2 boxCastOffset = new Vector2(0f, 2.5f); // ✅ 오프셋 설정 가능

    private PoolType poolType;
    private Vector3 moveDir;
    private bool hasHit = false;

    public void Init(Vector3 direction, float dmg, PoolType type)
    {
        moveDir = direction.normalized;
        bulletDamage = dmg;
        poolType = type; // 저장!

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(0.05f);

        if (!hasHit)
        {
            // ✅ 기준 위치를 로컬 오프셋 적용한 월드 위치로 변경
            Vector2 offsetWorld = transform.TransformVector(boxCastOffset); // 로컬 → 월드 변환
            Vector2 origin = (Vector2)transform.position + offsetWorld;
            float angle = transform.eulerAngles.z;

            RaycastHit2D hit = Physics2D.BoxCast(
                origin,
                boxCastSize,
                angle,
                moveDir,
                boxCastDistance,
                targetLayer
            );

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>()?.TakeDamage(bulletDamage);
                Debug.Log($"[BossLaser] 플레이어에게 {bulletDamage} 데미지 입힘");
                hasHit = true;
            }
        }

        yield return new WaitForSeconds(duration);
        StartCoroutine(ReturnToPoolAfterDelay(gameObject, duration, poolType));
        
        //Destroy(gameObject);
    }
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null && obj.activeInHierarchy)
        {
            ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // ✅ 디버그 시에도 오프셋 반영
        Vector2 offsetWorld = transform.TransformVector(boxCastOffset);
        Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3)offsetWorld, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxCastSize);
    }

}
