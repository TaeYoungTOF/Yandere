using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    private EnemyController enemyController;

    void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HitBox Trigger Enter");
            enemyController.SetAttackRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemyController.SetAttackRange(false);
    }
}
