using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossGrenadeProjectile02 : MonoBehaviour
{
   [Header("이펙트 설정")]
   [SerializeField] private GameObject explosionEffect;
   [SerializeField] private GameObject warningEffectPrefab; // ⛳ 범위 경고 이펙트
   
   [Header("수류탄 설정")]
   [SerializeField] private float damageRadius = 2f;
   [SerializeField] private int damageAmount = 10;
   [SerializeField] private float blindDuration = 5f; 

   private Vector3 targetPos;
   private float moveDuration;
   private float arcHeight;

   public void Init(Vector3 target, float height, float duration)
   {
      targetPos = target;
      arcHeight = height;
      moveDuration = duration;

      // ✅ 도착 지점에 경고 이펙트 생성
      if (warningEffectPrefab != null)
      {
         GameObject war = Instantiate(warningEffectPrefab, targetPos, Quaternion.identity);
         Destroy(war, 0.8f);
      }

      StartCoroutine(MoveToTarget());
   }


   private IEnumerator MoveToTarget()
   {
      Vector3 startPos = transform.position;
      float elapsed = 0f;

      while (elapsed < moveDuration)
      {
         float t = elapsed / moveDuration;
         float heightOffset = arcHeight * Mathf.Sin(t * Mathf.PI); // 포물선

         transform.position = Vector3.Lerp(startPos, targetPos, t) + new Vector3(0, heightOffset, 0);

         elapsed += Time.deltaTime;
         yield return null;
      }

      Explode();
   }

   private void Explode()
   {
      // 💥 폭발 이펙트 (도착 지점 기준)
      if (explosionEffect != null)
         Instantiate(explosionEffect, targetPos, Quaternion.identity);

      // TODO: 디버프 영역 생성 예정 (ex: SmokeDamageZone)
      Collider2D hit = Physics2D.OverlapCircle(targetPos, damageRadius, LayerMask.GetMask("Player"));
      if (hit != null)
      {
         Player player = hit.GetComponent<Player>();
         if (player != null)
         {
            player.TakeDamage(damageAmount);

            ApplyBlindDebuff(blindDuration);
         }
      }

      Destroy(gameObject);
   }
    
   private void ApplyBlindDebuff(float duration)
   {
       StartCoroutine(BlindDebuffRoutine(duration));
   }

   private IEnumerator BlindDebuffRoutine(float duration)
   {
       UIManager.Instance.ShowBlindOverlay(true); // 시야 가림
       yield return new WaitForSeconds(duration);
       UIManager.Instance.ShowBlindOverlay(false); // 해제
   }
   
   private void OnDrawGizmosSelected()
   
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(targetPos, damageRadius);
   }
}
