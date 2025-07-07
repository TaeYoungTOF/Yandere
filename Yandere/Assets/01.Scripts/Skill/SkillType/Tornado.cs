using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    // public GameObject tornadoVisualPrefab;
    // public int tornadoCount = 3;
    // public float orbitRadius = 2.5f;
    // public float rotateSpeed = 120f; // degrees per second
    // public float duration = 8f;
    // public int damage = 15;
    // public float tickInterval = 0.3f;
    // public float knockbackDistance = 1f;
    // public LayerMask enemyLayer;

    // private Transform player;
    // private List<Transform> tornadoVisuals = new List<Transform>();
    // private float elapsed;
    // private float tickTimer;
    // private float currentAngle;

    // void Start()
    // {
    //     player = GameObject.FindGameObjectWithTag("Player")?.transform;
    //     if (player == null)
    //     {
    //         Debug.LogError("플레이어를 찾을 수 없습니다!");
    //         Destroy(gameObject);
    //         return;
    //     }

    //     // 토네이도 비주얼 오브젝트 생성
    //     for (int i = 0; i < tornadoCount; i++)
    //     {
    //         GameObject visual = Instantiate(tornadoVisualPrefab, transform);
    //         tornadoVisuals.Add(visual.transform);
    //     }
    // }

    // void Update()
    // {
    //     if (player == null) return;

    //     elapsed += Time.deltaTime;
    //     tickTimer += Time.deltaTime;

    //     if (elapsed >= duration)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }

    //     // 회전 각도 누적 (시계 방향)
    //     currentAngle += rotateSpeed * Time.deltaTime;
    //     RotateTornados();

    //     // 틱 대미지 체크
    //     if (tickTimer >= tickInterval)
    //     {
    //         tickTimer = 0f;
    //         CheckCollisions();
    //     }
    // }

    // void RotateTornados()
    // {
    //     for (int i = 0; i < tornadoVisuals.Count; i++)
    //     {
    //         float angleOffset = i * (360f / tornadoCount);
    //         float angle = currentAngle + angleOffset;
    //         Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
    //         tornadoVisuals[i].position = player.position + dir * orbitRadius;
    //     }
    // }

    // void CheckCollisions()
    // {
    //     foreach (var tornado in tornadoVisuals)
    //     {
    //         Collider2D[] hits = Physics2D.OverlapCircleAll(tornado.position, 0.4f, enemyLayer);
    //         foreach (var hit in hits)
    //         {
    //             var enemy = hit.GetComponent<IDamagable>();
    //             if (enemy != null)
    //             {
    //                 enemy.TakeDamage(damage);

    //                 // 넉백
    //                 Vector2 dir = (hit.transform.position - player.position).normalized;
    //                 hit.transform.position += (Vector3)(dir * knockbackDistance);
    //             }
    //         }
    //     }
    // }

    // public void Initialize(LevelupData data, LayerMask enemyLayer)
    // {
    //     this.tornadoCount = data.projectileCount;
    //     this.damage = data.damage;
    //     this.duration = data.range;
    //     this.tickInterval = data.cooldown;
    //     this.enemyLayer = enemyLayer;
    // }
}
