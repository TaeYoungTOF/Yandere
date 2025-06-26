using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    private Transform player;

    void Start() { player = FindObjectOfType<Player>().transform; }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) > 100f)
            Destroy(gameObject);
    }
}
