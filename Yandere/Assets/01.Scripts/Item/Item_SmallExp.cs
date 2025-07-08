using UnityEngine;

public class Item_SmallExp : Item
{
    [SerializeField] private float _expAmount = 10;

    public override void Use(Player player)
    {
        Debug.Log("[ExpOrb] Exp");

        player.GainExp(_expAmount);

        Destroy(gameObject);
    }
}
