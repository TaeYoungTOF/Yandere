using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossPattern_Shoot : MonoBehaviour, IBossPattern
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float spreadAngle = 20f;
    [SerializeField] private float cooldown = 15f;

    private float timer = 0f;
    private Transform _player;
    public bool IsDone { get; private set; }
    public bool CanExecute() => timer <= 0f;
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
    }
    
    public void Execute()
    {
        StartCoroutine(FireRoutine());
    }
    
    private IEnumerator FireRoutine()
    {
        if (_player == null)
        {
            Debug.LogWarning("[BossPattern_Shoot] 플레이어가 없습니다.");
            yield break;
        }
        
        timer = cooldown;
        IsDone = false;

        Vector2 direction = (_player.position - transform.position).normalized;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"[FireRoutine] {i + 1}세트 발사 중");

            for (int j = -1; j <= 1; j++)
            {
                float angle = spreadAngle * j;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * direction;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bullet.GetComponent<Enemy_BossBullet>().Init(dir);
                SoundManager.Instance.Play("InGame_EnemyBoss_ShootSkillSFX");
            }

            yield return new WaitForSeconds(0.5f);
        }

        IsDone = true;
    }
}
