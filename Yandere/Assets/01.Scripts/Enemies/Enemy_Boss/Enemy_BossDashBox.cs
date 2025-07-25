using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossDashBox : MonoBehaviour
{
   private Enemy_BossPattern_Charge _dashPattern;

   void Awake()
   {
      _dashPattern = GetComponentInParent<Enemy_BossPattern_Charge>();
   }

   void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         _dashPattern.SetCanUseDash(true);
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         _dashPattern.SetCanUseDash(false);
      }
   }
}
