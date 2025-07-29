using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
   
   [Header("세팅")]
   [SerializeField] private GameObject smokeZonePrefab;
   [SerializeField] private float moveSpeed = 5f;
   [SerializeField] private float stopDistance = 0.1f;

   [SerializeField] private GameObject warningEffectPrefab;
   
   private Vector3 _targetPosition;
   private bool _launched = false;

   // 호출 필수!
   public void Launch(Vector3 targetPos)
   {
      _targetPosition = targetPos;
      _launched = true;
      
      GameObject warn = Instantiate(warningEffectPrefab, targetPos, Quaternion.identity);
      Destroy(warn, 1f);
   }

   private void Update()
   {
      if (!_launched) return;

      // 목표 위치까지 이동
      transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

      // 거의 도착했으면 폭발
      if (Vector3.Distance(transform.position, _targetPosition) <= stopDistance)
      {
         Explode();
      }
   }

   private void Explode()
   {
      Instantiate(smokeZonePrefab, transform.position, Quaternion.identity);
      SoundManager.Instance.Play("InGame_EnemyBoss_SmokeSFX");
      Destroy(gameObject);
   }
   
}
