using UnityEngine;

public class Item_SmallHeal : Item
{
    [SerializeField] private float _healAmount;

    public override void Use(Player player)
    {
        Debug.Log("[Heal] heal");

        player.Heal(_healAmount);

        Destroy(gameObject);
    }
}
