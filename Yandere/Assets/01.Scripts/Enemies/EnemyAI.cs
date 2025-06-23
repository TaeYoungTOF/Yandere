using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 AI를 작성하는 스크립트입니다.
public class EnemyAI : MonoBehaviour
{
   public Transform player; // 플레이어의 Transform 값을 참조합니다, 추후 수정될 수 있습니다.
   public float moveSpeed = 5.0f; // 적의 이동속도를 정합니다.
   public float dectionRange = 5.0f; // 적의 추적 범위를 정합니다.
   
   // 매 프레임마다 호출되는 메서드로, AI의 동작을 구현한다.
   void Update()
   {
      // 플레이어와 적의 거리 계산
      float distanceToPlayer = Vector2.Distance(transform.position, player.position);
      
      // 플레이어 감지 범위 안에 있으면 추적 시작
      if (distanceToPlayer > dectionRange)
      {
         // 플레이어를 향한 방향 계산
         Vector2 direction = (player.position - transform.position).normalized;
         
         // 적이 그 방향으로 이동하도록 설정
         transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
      }
   }
}
