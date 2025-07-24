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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<EnemyController>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    public void TryDash()
    {
        if (timer > 0f || _player == null) return;

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
    }
}
