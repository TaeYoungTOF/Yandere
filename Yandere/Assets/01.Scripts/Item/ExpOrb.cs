using UnityEngine;

public class ExpOrb : Item
{
    [SerializeField] private float _expAmount = 10;
    public float moveSpeed = 8f;

    /**
    private Transform player;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.Player.GainExp(_expAmount);

            Destroy(gameObject);
        }
    }
}
