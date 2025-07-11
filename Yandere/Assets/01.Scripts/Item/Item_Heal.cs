using UnityEngine;

public class Item_Heal : Item
{
    [SerializeField] private float _healAmount;

    public override void Use(Player player)
    {
        Debug.Log("[Heal] heal");

        player.Heal(_healAmount);

        Destroy(gameObject);
    }
}
