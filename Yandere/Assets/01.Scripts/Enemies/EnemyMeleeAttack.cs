using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    
    
    public void Attack(float damage)
    {
        StageManager.Instance.Player.TakeDamage(damage);
        
        
        //Debug.Log($"[Melee] 플레이어에게 {damage} 근접 피해");
    }
}
