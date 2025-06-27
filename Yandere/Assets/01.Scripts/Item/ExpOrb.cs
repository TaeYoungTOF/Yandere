using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expAmount = 10;
    public float moveSpeed = 8f;
    private Transform player;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance < 0.3f)
            {
                var p = player.GetComponent<Player>();
                if (p != null)
                {
                    p.GainExp(expAmount);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null) p.GainExp(expAmount);

            Destroy(gameObject);
        }
    }

    public void SetExp(int exp)
    {
        expAmount = exp;
    }
}
