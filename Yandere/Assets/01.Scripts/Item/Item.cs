using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Data")]
    //public int itemId;
    public string itemName;
    public string description;

    public abstract void Use(Player player);
}