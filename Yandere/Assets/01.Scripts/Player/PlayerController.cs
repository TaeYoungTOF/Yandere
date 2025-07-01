using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    private Rigidbody2D _rigidbody;
    private Vector3 moveVec;

    private Vector3 lastMoveDir = Vector2.right;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 조이스틱에서 입력 값을 받아 옴
        float x = floatingJoystick.Horizontal;
        float y = floatingJoystick.Vertical;
        moveVec = new Vector3(x, y).normalized;

        //방향 저장 추가
        if (moveVec.sqrMagnitude > 0)
        {
            lastMoveDir = new Vector3(x, y).normalized;
        }

        // 이동 처리
        transform.position += StageManager.Instance.Player.stat.moveSpeed * Time.deltaTime * moveVec;

        // 회전 생략
    }

    public Vector3 GetLastMoveDirection()
    {
        return lastMoveDir != Vector3.zero ? lastMoveDir : Vector3.right;
    }
}
