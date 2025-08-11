using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EType_Attack_Projectile01 : MonoBehaviour
{
   [Header("이펙트 설정")]
   [SerializeField] private GameObject explosionEffect;
   [SerializeField] private GameObject groundEffect;
   [SerializeField] private GameObject explosionWarningEffect;
   
   [Header("수류탄 설정")]
   [SerializeField] private float tickDamage;
   [SerializeField] private float tickDelay;
   [SerializeField] private float damageRadius;
   [SerializeField] private float damageDuration;

   private Vector3 targetPos;
   private Vector3 startPos;
   private float moveDuration;
   private float arcHeight;

   public void Init(Vector3 target, float height, float tickDamage, float tickDelay, float damageRadius, float ThrowDuration, Vector3 startPosition, float damgeDuration)
   {
      targetPos = target;
      arcHeight = height;
      this.tickDamage = tickDamage;
      this.tickDelay = tickDelay;
      this.damageRadius = damageRadius;
      this.damageDuration = damgeDuration;
      this.moveDuration = ThrowDuration;
      this.startPos = startPosition;

      if (explosionWarningEffect != null)
      {
         GameObject effectWarning = Instantiate(explosionWarningEffect, targetPos, Quaternion.identity);
         float scale = damageRadius * 2f * 1f;
         effectWarning.transform.localScale = new Vector3(scale, scale, 1f);
         Destroy(effectWarning, damageDuration+1.5f);
      }
      
      StartCoroutine(MoveToTarget());
   }
   
   private IEnumerator MoveToTarget()
   {
      float elapsed = 0f;

      while (elapsed < moveDuration)
      {
         float t = elapsed / moveDuration;
         float heightOffset = arcHeight * Mathf.Sin(t * Mathf.PI);
         
         transform.position = Vector3.Lerp(startPos, targetPos, t) + new Vector3(0, heightOffset, 0);
         
         elapsed += Time.deltaTime;
         yield return null;
      }
      
      Explode();
   }
   
   private void Explode()
   {
      // ✅ 폭발 이펙트 생성
      if (explosionEffect != null)
      {
           
         SoundManager.Instance.Play("InGame_EnemyBoss1Pattern3_SmokeSFX02");
         
         GameObject effect = ObjectPoolManager.Instance.GetFromPool(PoolType.EnemyEAttackGrenadeProj02,
            transform.position, Quaternion.identity);
            
         effect.transform.position = transform.position;
         ParticleSystem ps = effect.GetComponent<ParticleSystem>();
         if (ps != null)
         {
            ps.Clear(); // 이전 파티클 흔적 제거
            ps.Play();  // 다시 재생
         }
         
         StartCoroutine(ReturnToPoolAfterDelay(effect, damageDuration, PoolType.EnemyEAttackGrenadeProj02));
      }
      SoundManager.Instance.Play("InGame_EnemyBoss1Pattern3_SmokeSFX");
      // ✅ 데미지 주는 영역 생성 (코루틴 시작)
      StartCoroutine(DamageOverTime());
      
      
   }

   private IEnumerator DamageOverTime()
   {
      // 장판 이펙트 생성
      GameObject ground = null;
      if (groundEffect != null)
      {
         ground = ObjectPoolManager.Instance.GetFromPool(PoolType.EnemyEAttackGrenadeProj03, targetPos, Quaternion.identity);

         // 장판 크기를 데미지 범위에 맞춰 조절
         float scale = damageRadius * 2f * 0.2f;
         ground.transform.localScale = new Vector3(scale, scale, 1f);
         
         StartCoroutine(ReturnToPoolAfterDelay(ground, damageDuration, PoolType.EnemyEAttackGrenadeProj03));
      }
      
        float timer = 0f;

        while (timer < damageDuration)
        {
           Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, damageRadius, LayerMask.GetMask("Player"));

           foreach (var hit in hits)
           {
              Player player = hit.GetComponent<Player>();
              if (player != null)
              {
                 player.TakeDamage(tickDamage);
                 Debug.Log($"[E타입 에너미: 연막데미지]{tickDamage}피해 적용");
              }
           }
           yield return new WaitForSeconds(tickDelay);
           timer += tickDelay;
        }
        // 장판 제거
        if (ground != null)
           
        
        ObjectPoolManager.Instance.ReturnToPool(PoolType.EnemyEAttackGrenadeProj01, gameObject);
   }
   
   
   private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
   {
      yield return new WaitForSeconds(delay);

      if (obj != null && obj.activeInHierarchy)
      {
         ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
      }
   }
   
#if UNITY_EDITOR
   private void OnDrawGizmosSelected()
   {
      // 장판 지속 데미지 범위 표시
      if (targetPos != Vector3.zero)
      {
         Gizmos.color = Color.blue; // 장판 범위는 파랑색
         Gizmos.DrawWireSphere(targetPos, damageRadius);
      }
   }
#endif

}
