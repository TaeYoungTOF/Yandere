using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public PlayerStat PlayerStat { get; private set; }
   public PlayerResource PlayerResource { get; private set; }

   private void Awake()
   {
       PlayerStat = new PlayerStat();
       PlayerResource = new PlayerResource();
   }
   
   public void AddGold()
   {
      
   }

   public void AddExp()
   {
      
   }

   public void LevelUP()
   {
      
   }

}
