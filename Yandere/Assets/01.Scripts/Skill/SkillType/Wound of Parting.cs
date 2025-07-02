using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundofParting : MonoBehaviour
{
    public GameObject projectilePrefab;
    private int projectileCount;
    private float angleSpread;
    private float projectileSpeed;
    private int damage;
    private float range;
    private int pierceCount;
    private LayerMask enemyLayer;

    private Transform player;
    private bool isInitialized = false;

    public void Initialize(SkillStatData stat, LayerMask enemyLayer)
    {
        this.projectileCount = stat.projectileCount;
        this.angleSpread = 45f; // 퍼짐 각도는 고정값 또는 stat에 넣어도 됨
        this.projectileSpeed = stat.speed;
        this.damage = stat.damage;
        this.range = stat.range;
        this.pierceCount = stat.pierceCount;
        this.enemyLayer = enemyLayer;

        isInitialized = true;
    }

    void Start()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("WoundofParting 초기화되지 않음");
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            Destroy(gameObject);
            return;
        }

        Vector2 lastDirection = Vector2.right;

        var controller = player.GetComponent<Player>();
        if (controller != null)
        {
            lastDirection = controller.GetLastMoveDirection();
            if (lastDirection == Vector2.zero)
                lastDirection = Vector2.right;
        }

        float startAngle = -angleSpread * 0.5f;
        float angleStep = angleSpread / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * lastDirection;

            Vector3 spawnPos = player.position + (Vector3)(direction.normalized * 1f);
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            var projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(damage, projectileSpeed, range, pierceCount, enemyLayer, direction);
            }
            else
            {
                Debug.LogWarning($">> Projectile [{i}]에 'Projectile' 스크립트가 없습니다!");
            }
        }

        Destroy(gameObject);
    }
}
