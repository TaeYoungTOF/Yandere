using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
   public FloatingJoystick floatingJoystick;
   private Rigidbody2D rigidbody;
   private Vector2 moveVec;
   


   private void Awake()
   {
      rigidbody = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      // 조이스틱에서 입력 값을 받아 옴
      float x = floatingJoystick.Horizontal;
      float y = floatingJoystick.Vertical;
      Vector3 moveVec = new Vector3(x, y, 0).normalized;
   
      
      // 받아온 입력값 * 플레이어 이동스탯 * 델타타임 을 "moveVec"에 값을 넣어줌
      transform.position += moveVec * GameManager.Instance.PlayerManager.PlayerStat.BasePlayerMoveSpeed * Time.deltaTime;
      
      
      // 이동 입력값이 0 일시 리턴 (회전 X) 
      if (moveVec.sqrMagnitude == 0)
         return;
      
      // 1. 이동 방향에 따른 회전 각도 계산 (z축 기준, 단위: 도)
      float angle = Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg;
   
      // 2. 각도를 Rigidbody2D에 직접 적용 (바로 회전)
      rigidbody.rotation = angle;
   }
   
   public void TakeDamage(int damage)
   {
      
   }

   public void Die()
   {
      
   }

   public void PlayerAttackAnimation()
   {
      
   }

   
}
