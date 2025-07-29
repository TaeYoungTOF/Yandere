using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    
    
    public string itemName =>  itemData.itemName;
    public string description =>  itemData.description;

    private float _pickupDelay;
    private float _spawnTime;

    public void Initialize(ItemData itemData)
    {
        this.itemData = itemData;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && itemData.icon != null)
        {
            spriteRenderer.sprite = itemData.icon;
        }
    }

    public virtual void Use(Player player)
    {
       
        switch (itemData.itemtpye)
        {
            case ItemType.ExpItem:
                player.GainExp(itemData.amount);
                SoundManager.Instance.Play("InGame_Player_ExpItemPickUpSFX");
                break;
            case ItemType.HealItem:
                player.Heal(itemData.amount);
                SoundManager.Instance.Play("InGame_Player_HealItemPickUpSFX");
                break;
            // case ItemType.GoldItem:
            //     player.GainGold((int)itemData.amount);
            //     break;
            case ItemType.BoomItem:
                DoExplosionEffect();
                break;
            case ItemType.MagnetItem:
                DoMagnetEffect(player);
                break;
        }
        Destroy(gameObject);

    }
    
    public void SetPickupDelay(float delay)
    {
        _pickupDelay = delay;
        _spawnTime = Time.time;
    }

    public bool CanPickup()
    {
        return Time.time >= _spawnTime + _pickupDelay;
    }
    
    private void DoExplosionEffect()
    {
        Debug.Log("[Item] 폭탄 효과 발동!");

        Vector2 center = transform.position;
        float radius = itemData.explosionRadius;
        float damage = itemData.explosionDamage;

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    
    private void DoMagnetEffect(Player player)
    {
        Debug.Log("[Item] 자석 효과 발동!");

        float radius = itemData.magnetRadius;
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item_ExpOrb");

        foreach (GameObject obj in allItems)
        {
            if (obj == gameObject) continue;

            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance <= radius)
            {
                if (obj.TryGetComponent<Item_ItemDropEffect>(out var dropEffect))
                {
                    dropEffect.MoveToPlayerInstantly(5f); // 속도는 정해도 되고, SO에 추가해도 됨
                }
            }
        }
    }
    
    
    
}