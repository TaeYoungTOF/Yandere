using UnityEngine;

public class MagnetItem : MonoBehaviour
{
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
