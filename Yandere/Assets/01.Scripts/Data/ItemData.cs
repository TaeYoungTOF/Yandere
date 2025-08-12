using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
   ExpItem,
   HealItem,
   GoldItem,
   BoomItem,
   MagnetItem
}

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
   [Header("아이템 데이터")]
   public string itemName;
   [Multiline (3)]public string description;
   public Sprite icon;
   public ItemType itemtpye;
   public GameObject itemEffectPrefab;
   public float amount;

   [Header("폭탄 전용")]
   public float explosionRadius;
   public float explosionDamage;

   [Header("자석 전용")]
   public float magnetRadius;


}
