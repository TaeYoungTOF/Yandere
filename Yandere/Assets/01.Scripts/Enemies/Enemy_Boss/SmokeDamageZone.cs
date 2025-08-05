using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDamageZone : MonoBehaviour
{
   [Header("설정")]
   [SerializeField] private float duration = 3f;
   [SerializeField] private float damageInterval = 1f;
   [SerializeField] private int damagePerTick = 100;
   [SerializeField] private GameObject rangeIndicator;
   
   

   private List<Transform> targets = new();
   private Coroutine damageRoutine;

   private void Start()
   {
      if (rangeIndicator != null)
         rangeIndicator.SetActive(true);

      damageRoutine = StartCoroutine(DamageOverTime());

      // 지속 시간 후 사라지기
      StartCoroutine(DestroyAfterDelay());
   }

   private IEnumerator DestroyAfterDelay()
   {
      yield return new WaitForSeconds(duration);
      
      if (damageRoutine != null)
         StopCoroutine(damageRoutine);

      Destroy(gameObject);
   }

   IEnumerator DamageOverTime()
   {
      while (true)
      {
         yield return new WaitForSeconds(damageInterval);
         
         foreach (var t in targets)
         {
            if (t != null)
            {
               StageManager.Instance.Player.TakeDamage(damagePerTick);
              
               Debug.Log($"연막 데미지 : {damagePerTick}");
            }
         }
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         targets.Add(other.transform);
      }
   }

   void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         targets.Remove(other.transform);
      }
   }
}
