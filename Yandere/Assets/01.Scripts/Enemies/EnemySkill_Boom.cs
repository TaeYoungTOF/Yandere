using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill_Boom : MonoBehaviour
{
   [SerializeField] private GameObject boomPrefab;
   [SerializeField] private Transform boomSpawnPoint;
   [SerializeField] private float cooldown = 10f;
   [SerializeField] private float spreadAngle = 15f;

   private float timer;
   private Transform _player;

   void Awake()
   {
      _player = GameObject.FindGameObjectWithTag("Player")?.transform;
   }

   void Update()
   {
      if (timer > 0f) timer -= Time.deltaTime;
   }

   public void TryBoom()
   {
      if (timer > 0f || _player == null) return;

      Vector2 baseDir = (_player.position - transform.position).normalized;

      for (int i = -1; i <= 1; i++)
      {
         float angle = spreadAngle * i;
         Vector2 dir = Rotate(baseDir, angle);

         GameObject bullet = Instantiate(boomPrefab, boomSpawnPoint.position, Quaternion.identity);
         bullet.GetComponent<EnemyBoomProjectile>().Init(dir);
      }
      
      timer = cooldown;
   }

   Vector2 Rotate(Vector2 v, float degrees)
   {
      return Quaternion.Euler(0, 0, degrees) * v; // ✅ 회전된 방향 벡터 반환
   }
}
