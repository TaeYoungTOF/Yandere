using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss1_Pattern1_Projectile01 : MonoBehaviour
{
   
   [Header("이펙트 프리팹")]
   [SerializeField] private GameObject impactEffect;
   [SerializeField] private GameObject muzzleFlashEffect;
   
   [Header("총알 관련 설정")]
   public float bulletSpeed = 10f;
   public float bulletDamage = 10f;
   [SerializeField] private float impactEffectLifeTime = 0.5f;
   
   [Header("회전 관련")]
   public float moveAngleRad = 0f;
   public float spriteAngleRad = 0f;
   
   [Header("설정")]
   public string targetTag = "Player";
   public bool rotateSprite = true;
   public bool muzzleFlash = true;
   public float rotationSpeed = 0;
   public float rotationRange = 0;
   
   private bool rotateClockwise = false;
   private float timeSinceLastFrame = 0;

   private bool isFacingLeft = false;
   public void SetFacingDirection(bool facingLeft)
   {
      isFacingLeft = facingLeft;
   }

   private void Start()
   {
      if (rotateSprite)
      {
         transform.rotation = Quaternion.Euler(0, 0, spriteAngleRad * Mathf.Rad2Deg);
      }

      if (muzzleFlash && muzzleFlashEffect != null)
      {
         
         //GameObject flash = Instantiate(muzzleFlashEffect, transform.position, Quaternion.identity);
         GameObject flash = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage1BossSkillPattern1Proj02,
            transform.position, Quaternion.identity);
       
        

         // Z축 회전으로 파티클 방향 조정
         float rotZ = isFacingLeft ? 180f : 0f;
         flash.transform.rotation = Quaternion.Euler(0, 0, rotZ);

         StartCoroutine(ReturnToPoolAfterDelay(flash, 0.5f, PoolType.Stage1BossSkillPattern1Proj02));
        // Destroy(flash, 0.5f);
      }
   }

   private void Update()
   {
      BulletMove();
      RotateProjectile();
   }

   public void Init(float damage, float speed, float angle)
   {
      bulletDamage = damage;
      bulletSpeed = speed;
      moveAngleRad = angle;
      spriteAngleRad = angle;

      // 명확히 회전 설정
      if (rotateSprite)
      {
         transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
      }
   }

   void BulletMove()
   {
      Vector3 dir = new Vector3(Mathf.Cos(moveAngleRad), Mathf.Sin(moveAngleRad), 0f);
      transform.position += dir * bulletSpeed * Time.deltaTime;
   }
   
   void RotateProjectile()
   {
      if (rotationRange > 0 && rotationSpeed > 0)
      {
         float zRot = rotationSpeed * timeSinceLastFrame;
         if (!rotateClockwise)
         {
            transform.Rotate(0, 0, zRot);
            if (transform.rotation.z * Mathf.Rad2Deg >= rotationRange)
               rotateClockwise = true;
         }
         else
         {
            transform.Rotate(0, 0, -zRot);
            if (transform.rotation.z * Mathf.Rad2Deg <= -rotationRange)
               rotateClockwise = false;
         }
      }
   }
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (!col.CompareTag(targetTag)) return;

      Player player = col.GetComponent<Player>();
      if (player != null)
      {
         player.TakeDamage(bulletDamage);
      }

      Impact();
   }
   
   private void Impact()
   {
      if (impactEffect != null)
      {
         //GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
         GameObject effect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage1BossSkillPattern1Proj03,
            transform.position, Quaternion.identity);

         StartCoroutine(DelayedReturnToPool(impactEffectLifeTime));
         StartCoroutine(ReturnToPoolAfterDelay(effect, impactEffectLifeTime, PoolType.Stage1BossSkillPattern1Proj03));
         //Destroy(effect, impactEffectLifeTime);
      }
   
      ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage1BossSkillPattern1Proj01, gameObject);
      //Destroy(gameObject);
   }

   IEnumerator DelayedReturnToPool(float delay)
   {
      yield return new WaitForSeconds(delay);
   }
   
   private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
   {
      yield return new WaitForSeconds(delay);

      if (obj != null && obj.activeInHierarchy)
      {
         ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
      }
   }
}
