using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Data")]
    //public int itemId;
    public string itemName;
    public string description;

    private float _pickupDelay;
    private float _spawnTime;

    public abstract void Use(Player player);

    public void SetPickupDelay(float delay)
    {
        _pickupDelay = delay;
        _spawnTime = Time.time;
    }

    public bool CanPickup()
    {
        return Time.time >= _spawnTime + _pickupDelay;
    }
}