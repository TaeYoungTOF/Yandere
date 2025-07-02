using UnityEngine;

public class Item_Magnet : Item
{
    public override void Use(Player player)
    {
        Debug.Log("[Magnet] Magnet");

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            foreach (var exp in FindObjectsOfType<Experience>())
            {
                exp.StartMagnetEffect();
            }
            Destroy(gameObject);
        }
        */
    }
}
