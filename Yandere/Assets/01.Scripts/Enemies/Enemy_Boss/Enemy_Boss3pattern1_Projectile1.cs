using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy_Boss3pattern1_Projectile1 : MonoBehaviour
{
    [SerializeField] private float duration = 1f;         // 레이저 유지 시간
    [SerializeField] private int damage = 150;            // 데미지
    [SerializeField] private LayerMask targetLayer;       // 플레이어 레이어
    private Vector3 moveDir;
    private float moveSpeed;
    //[SerializeField] private GameObject hitEffectPrefab;  // 피격 이펙트 (선택)

    private bool hasHit = false;

    public void Init(Vector3 direction, float speed, int dmg)
    {
        moveDir = direction.normalized;
        moveSpeed = speed;
        damage = dmg;

        // 위쪽 기준 회전
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);

        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(0.05f); // 발사 후 약간의 딜레이 후 판정

        if (!hasHit)
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, 0f, targetLayer);
            if (hit != null && hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>()?.TakeDamage(damage);

                // 넉백 또는 디버프 등 추가 가능
                Debug.Log($"[BossLaser] 플레이어에게 {damage} 데미지 입힘");
                
                /*
                if (hitEffectPrefab)
                    Instantiate(hitEffectPrefab, hit.transform.position, Quaternion.identity);
                */
                hasHit = true;
            }
        }

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (TryGetComponent(out BoxCollider2D box))
            Gizmos.DrawWireCube(transform.position, box.size);
    }
}
