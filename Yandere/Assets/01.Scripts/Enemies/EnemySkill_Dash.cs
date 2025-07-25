using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill_Dash : MonoBehaviour
{
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float duration = 0.3f;

    private float timer = 0f;
    private Rigidbody2D _rb;
    private EnemyController _controller;
    private Transform _player;
    private bool _canUseDash = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<EnemyController>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;

        // ✅ 대시 조건 체크를 여기서
        if (_canUseDash && timer <= 0f)
        {
            TryDash();
        }
    }
    public void TryDash()
    {
        if (timer > 0f || _player == null || !_canUseDash) return;

        Vector2 dir = (_player.position - transform.position).normalized;
        _controller.isDashing = true;
        _rb.velocity = dir * dashForce;
        timer = cooldown;
        StartCoroutine(StopDash());
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(duration);
        _rb.velocity = Vector2.zero;
        _controller.isDashing = false;
        
        _controller.DelayAttack(1f);
        
    }
    public void SetCanUseDash(bool value)
    {
        _canUseDash = value;
    }
    
    public bool IsDashing => _controller != null && _controller.isDashing;

    public float DashForce => dashForce;
    
    
}
