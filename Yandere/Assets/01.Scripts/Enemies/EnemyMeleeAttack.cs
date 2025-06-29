using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private Transform playerTransform;
    private EnemyData enemyData;

    private void Awake()
    {
        // 플레이어 찾아서 기억해두기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("플레이어가 씬에 없음!");
        }
    }

    /// <summary>
    /// EnemyController가 호출하는 근접 공격 실행 메서드
    /// </summary>
    /// <param name="damage">몬스터 데미지</param>
    public void DoAttack(float damage)
    {
        if (playerTransform == null)
        {
            Debug.Log("EnemyMeleeAttack: 플레이어가 없음!");
            return;
        }

        // 플레이어한테 데미지 주기
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
            Debug.Log($"근접 공격 성공! {damage} 데미지를 플레이어에게 줌");
        }
        else
        {
            Debug.Log("EnemyMeleeAttack: PlayerController 컴포넌트 없음!");
        }
    }
}
