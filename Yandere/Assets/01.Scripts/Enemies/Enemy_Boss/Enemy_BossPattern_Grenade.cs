using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BossPattern_Grenade : MonoBehaviour, IBossPattern
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private float throwForce = 7;

    private float timer;
    private Transform _player;
    
    public bool IsDone { get; private set; }
    public bool CanExecute() => timer <= 0;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    public void Execute()
    {
        StartCoroutine(GrenadeRoutine());
    }

    IEnumerator GrenadeRoutine()
    {
        timer = cooldown;
        IsDone = false;

        Vector2 dir = (_player.position - transform.position).normalized;

        var grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Quaternion.identity);
        grenade.GetComponent<GrenadeProjectile>().Launch(_player.position);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.5f);
        IsDone = true;
    }
}
