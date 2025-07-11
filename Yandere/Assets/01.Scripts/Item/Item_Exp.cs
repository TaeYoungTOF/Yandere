using UnityEngine;

public class Item_Exp : Item
{
    [SerializeField] private float _expAmount = 1;

    public override void Use(Player player)
    {
        Debug.Log("[ExpOrb] Exp");

        player.GainExp(_expAmount);

        Destroy(gameObject);
    }
}
