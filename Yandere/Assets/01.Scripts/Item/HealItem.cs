using UnityEngine;

public class HealItem : Item
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Heal(player.stat.maxHealth * 0.3f);
            Destroy(gameObject);
        }
    }
}
