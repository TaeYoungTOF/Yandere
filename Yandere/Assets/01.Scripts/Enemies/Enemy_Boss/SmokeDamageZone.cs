using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDamageZone : MonoBehaviour
{
   [SerializeField] private float duration = 3f;
   [SerializeField] private float damageInterval = 1f;
   [SerializeField] private int damagePerTick = 100;

   private List<Transform> targets = new();

   private void Start()
   {
      StartCoroutine(DamageOverTime());
      Destroy(gameObject, duration);
      
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
