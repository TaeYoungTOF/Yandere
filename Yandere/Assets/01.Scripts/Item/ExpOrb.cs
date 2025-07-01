using UnityEngine;

public class ExpOrb : Item
{
    [SerializeField] private float _expAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.Player.GainExp(_expAmount);

            Destroy(gameObject);
        }
    }
}
