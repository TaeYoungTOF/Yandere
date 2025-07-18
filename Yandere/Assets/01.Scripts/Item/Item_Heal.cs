using UnityEngine;

public class Item_Heal : Item
{

    public override void Use(Player player)
    {
        Debug.Log("[Heal] heal");

        player.Heal(itemData.amount);

        Destroy(gameObject);
    }
}
