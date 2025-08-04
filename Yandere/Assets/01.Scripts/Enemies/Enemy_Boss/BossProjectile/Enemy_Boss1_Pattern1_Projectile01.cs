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
   [SerializeField] private float speed = 10f;
   [SerializeField] private float lifeTime = 5f;
   [SerializeField] private float bulletDamage = 10f;
   
   
   [Header("회전")]
   public float moveAngleRad = 0f;
   public float spriteAngleRad = 0f;

   private void Start()
   {
      // 총알 방향대로 회전 (Sprite 시각용)
      transform.rotation = Quaternion.Euler(0, 0, spriteAngleRad * Mathf.Rad2Deg);

      // muzzle flash 생성
      if (muzzleFlashEffect != null)
      {
         GameObject flash = Instantiate(muzzleFlashEffect, transform.position, transform.rotation);
         Destroy(flash, 0.3f);
      }

      // 일정 시간 뒤 자동 파괴
      Destroy(gameObject, lifeTime);
   }

   private void Update()
   {
      Vector3 dir = new Vector3(Mathf.Cos(moveAngleRad), Mathf.Sin(moveAngleRad), 0f);
      transform.position += dir * speed * Time.deltaTime;
   }
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (!col.CompareTag("Player")) return;

      Player player = col.GetComponent<Player>();
      if (player != null)
      {
         player.TakeDamage(bulletDamage);
      }

      Impact();
   }
   
   private void OnBecameInvisible()
   {
      Impact();
   }

   private void Impact()
   {
      if (impactEffect != null)
      {
         GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
         Destroy(effect, 0.5f);
      }

      Destroy(gameObject);
   }
}
