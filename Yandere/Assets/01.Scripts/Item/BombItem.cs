using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                if (enemy.enemyType == EnemyType.Normal || enemy.enemyType == EnemyType.Elite)
                {
                    enemy.Die(withDrop: false); // 경험치 드랍 안함
                }
                else if (enemy.enemyType == EnemyType.Boss)
                {
                    enemy.TakeDamage(enemy.maxHp * 0.05f);
                }
            }
            Destroy(gameObject);
        }
        */
    }
}
