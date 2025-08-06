using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossController1 : EnemyController
{
   [Header("패턴 쿨타임")]
   [SerializeField] private float pattern1Cooldown = 15f;
   [SerializeField] private float pattern2Cooldown = 30f;
   [SerializeField] private float pattern3Cooldown = 30f;
   
   [Header("보스패턴1 권총 단발사격")]
   [SerializeField] private GameObject pattern1BulletPrefab;                  // 총알 프리팹 입니다.
   [SerializeField] private Transform pattern1BulletSpawnPoint;               // 총알이 발사 시작 될 위치입니다.
   [SerializeField] private float bulletDamage = 10f;                         // 총알 한발 당 데미지
   [SerializeField] private float bulletSpeed = 10f;                          // 총알 속도
   [SerializeField] private float shootDelay = 0.5f;                          // 한발쏘고 그 다음 간격 입니다.
   [SerializeField] private float bulletCount = 3;                            // 몇 세트=
   [SerializeField] private float pattern1AttackRange = 10f;                  // 공격 범위 입니다.
   [SerializeField] private float pattern1Interval = 1f;                      // 공격 시작 대기 시간 입니다.
   [SerializeField] private float spreadAngle = 20f;                          // 총알 갈래 각도 입니다.
   
   [Header("보스패턴2 돌진")]
   [SerializeField] private GameObject dashEffectPrefab;
   [SerializeField] private GameObject pattern2ChargeEffectPrefab;
   [SerializeField] private float pattern2DashRange = 8f;
   [SerializeField] private float dashSpeed = 20f;
   [SerializeField] private float dashDuration = 0.5f;
   [SerializeField] private float dashDamage = 50f;
   [SerializeField] private float dashKnockbackForce = 10f;
   [SerializeField] private LayerMask playerLayer;
   [SerializeField] private float pattern2Interval = 2f;
   [SerializeField] private float patternWarningTime = 1f;
   [SerializeField] private LineRenderer dashLinePreview;
  

   [Header("보스패턴3 수류탄 투척")]
   [SerializeField] private GameObject pattern3GrenadeProjectilePrefab;
   [SerializeField] private Transform pattern3GrenadeSpawnPoint;
   [SerializeField] private float pattern3Interval = 1f;
   [SerializeField] private float pattern3GrenadeRange = 7f;
   [SerializeField] private float grenadeThrowHeight  = 3f;
   [SerializeField] private float grenadeDuration = 1.5f;
   
   [Header("수류탄 설정")]
   [SerializeField] private float smokeTickDamage = 10f;
   [SerializeField] private float smokeDamagePerTickTime = 1f;
   [SerializeField] private float smokeRadius = 3f;
   [SerializeField] private float smokeDuration = 4f; 

   private Vector2 cachedDashDir;
   private float pattern1Timer = 0f;
   private float pattern2Timer = 0f;
   private float pattern3Timer = 0f;
   
   

   protected override void Start()
   {
      base.Start();
      StartCoroutine(BossPatternRoutine());
      
      if (dashLinePreview != null)
         dashLinePreview.enabled = false;
   }
   
   void Update()
   {
      if (isDead) return;
      
      pattern1Timer -= Time.deltaTime;
      pattern2Timer -= Time.deltaTime;
      pattern3Timer -= Time.deltaTime;
      
   }

   #region 보스 몬스터1 : TakeDamage 코드
   public override void TakeDamage(float damage)
   {
      SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
      if (isDead) return;

      damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
      _monsterCurrentHealth -= damage;

      Debug.Log($"[보스컨트롤러3] {enemyData.monsterName}가 {damage} 피해를 입었습니다");

      _animator.SetTrigger("Hit");

 
      if (_monsterCurrentHealth <= 0)
      {
         BossMonsterDie();
      }
   }
    
   #endregion
   
   #region 보스 몬스터1 : Die 코드
    
   void BossMonsterDie()
   {
      isDead = true;                                                      // 죽은 상태체크
      _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
      _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행
        
      StageManager.Instance.ChangeKillCount(1);
      
      StartCoroutine(DelayedReturnToPool(1));


      StageManager.Instance.StageClear();
      StartCoroutine(DelayedReturnToPool(1f));
   }
    
   #endregion

   #region 보스 패턴 루틴

   private IEnumerator BossPatternRoutine()
   {
      while (!isDead)
      {
         yield return new WaitForSeconds(1f);
         if (isPatterning) continue;

         List<int> availablePatterns = new List<int>();

         float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

         if (pattern1Timer <= 0f && distanceToPlayer <= pattern1AttackRange)
            availablePatterns.Add(1);

         if (pattern2Timer <= 0f && distanceToPlayer <= pattern2DashRange)
            availablePatterns.Add(2);
         if (pattern3Timer <= 0f && distanceToPlayer <= pattern3GrenadeRange)
            availablePatterns.Add(3);

         if (availablePatterns.Count > 0)
         {
            int selected = availablePatterns[Random.Range(0, availablePatterns.Count)];

            switch (selected)
            {
               case 1:
                  StartCoroutine(ExcutePattern1());
                  break;
               case 2:
                  StartCoroutine(ExecutePattern2());
                  break;
               case 3:
                  StartCoroutine(ExecutePattern3()); 
                  break;
            }
         }
      }
   }

   #endregion

   #region 패턴1: 권총 3갈래 단발 사격

   private IEnumerator ExcutePattern1()
   {
      isPatterning = true;
      pattern1Timer = pattern1Cooldown;
      
      Debug.Log("[보스 패턴1] 권총 단발사격 시작");
      
      //animator.SetTrigger("GunAttack"); // 애니메이션 트리거
      yield return new WaitForSeconds(pattern1Interval);
      
     
      if(_playerTransform == null) yield break;
      
      for (int i = 0; i < bulletCount; i++)             // bulletCount만큼 갈래 세트 발사
      {
         Vector2 dir = (_playerTransform.position - transform.position).normalized;
         float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

         _spriteRenderer.flipX = dir.x < 0;
         
         FireSpread(baseAngle);                         // 총알 발사 함수

         yield return new WaitForSeconds(shootDelay);   // 연사 간격
      } 
      
      yield return new WaitForSeconds(1f);              // 후딜
      isPatterning = false;
   }

   private void FireSpread(float baseAngle)
   {
      float[] offsets = { 0f, -spreadAngle, spreadAngle };

      foreach (float offset in offsets)
      {
         float angleDeg = baseAngle + offset;
         float angleRad = angleDeg * Mathf.Deg2Rad;

         // 총알 생성
         GameObject bullet = ObjectPoolManager.Instance.GetFromPool(
            PoolType.Stage1BossSkillPattern1Proj01,
            pattern1BulletSpawnPoint.position,
            Quaternion.identity);

         // ✅ bullet 오브젝트를 나중에 풀로 되돌리도록 코루틴 실행 (중요!)
         StartCoroutine(ReturnToPoolAfterDelay(bullet, 3.5f, PoolType.Stage1BossSkillPattern1Proj01));

         // 사운드 이펙트
         SoundManager.Instance.Play("InGame_EnemyBoss1_Pattern1_GunSFX");

         // 보스1용 Projectile 컴포넌트 가져오기
         Enemy_Boss1_Pattern1_Projectile01 proj = bullet.GetComponent<Enemy_Boss1_Pattern1_Projectile01>();

         if (proj == null)
         {
            Debug.LogWarning("Projectile 스크립트가 프리팹에 없음!");
            return;
         }

         proj.Init(bulletDamage, bulletSpeed, angleRad);
         bool facingLeft = _spriteRenderer.flipX;
         proj.SetFacingDirection(facingLeft);
         
      }
   }


   #endregion

   #region 패턴2: 돌진

    private IEnumerator ExecutePattern2()
   {
      isPatterning = true;
      pattern2Timer = pattern2Cooldown;

      Debug.Log("[보스 패턴2] 돌진 시작");

      // ✅ 플레이어 조준 방향 고정
      cachedDashDir = (_playerTransform.position - transform.position).normalized;

      // ✅ 시전 준비시간
      GameObject chargeEffect = Instantiate(pattern2ChargeEffectPrefab, transform.position, Quaternion.identity);
      yield return new WaitForSeconds(pattern2Interval);
     

      // ✅ 라인 렌더러 켜고 방향 미리보기 표시
      if (dashLinePreview != null)
      {
         dashLinePreview.enabled = true;
         ShowDashPreview(cachedDashDir); // 방향 고정
      }

      yield return new WaitForSeconds(patternWarningTime);  // 경고 시간

      if (dashLinePreview != null)
         dashLinePreview.enabled = false;
      Destroy(chargeEffect);

      // ✅ 돌진 시작
      bool hasHitPlayer = false;
      float timer = 0f;
      while (timer < dashDuration)
      {
         GameObject  dashEffect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage1BossSkillPattern2Proj01, transform.position, Quaternion.identity);
         //GameObject dasheffect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
         StartCoroutine(ReturnToPoolAfterDelay(dashEffect, 0.5f, PoolType.Stage1BossSkillPattern2Proj01));
         //Destroy(dasheffect, 0.5f);
        
         // ✅ 돌진 방향 고정
         transform.position += (Vector3)(cachedDashDir * dashSpeed * Time.deltaTime);
         timer += Time.deltaTime;

         if (!hasHitPlayer)
         {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector2.zero, 0f, playerLayer);
            if (hit.collider != null)
            {
               Player player = hit.collider.GetComponent<Player>();
               if (player != null)
               {
                  Vector2 knockDir = (hit.transform.position - transform.position).normalized;
                  StageManager.Instance.Player.TakeDamage(dashDamage);
                  player.GetComponent<Rigidbody2D>()?.AddForce(knockDir * dashKnockbackForce, ForceMode2D.Impulse);
                  hasHitPlayer = true;
               }
            }
         }
         yield return null;
      }

      yield return new WaitForSeconds(1f);
      isPatterning = false;
   }
   
   private void ShowDashPreview(Vector2 dir)
   {
      if (dashLinePreview == null) return;

      Vector3 start = transform.position;
      float dashDistance = dashSpeed * dashDuration; // ✅ 실제 돌진 거리 계산
      Vector3 end = start + (Vector3)(dir.normalized * dashDistance);

      dashLinePreview.SetPosition(0, start);
      dashLinePreview.SetPosition(1, end);
   }

   #endregion

   #region 패턴3: CS 수류탄 투척

   private IEnumerator ExecutePattern3()
   {
      isPatterning = true;
      pattern3Timer = pattern3Cooldown;
      
      yield return new WaitForSeconds(pattern3Interval);          // 수류탄 던지기 전 준비 시간
      
      Debug.Log("[보스 패턴3] 수류탄 투척 시작");

      ThrowFlashGrenade();                                        // 수류탄 던지는 메서드
      
      
      yield return new WaitForSeconds(1f);                        // 후딜레이
      isPatterning = false;
   }
   
   private void ThrowFlashGrenade()
   {
      if (_playerTransform == null || pattern3GrenadeProjectilePrefab == null)
      {
         Debug.LogWarning("섬광 수류탄 투척 실패: 대상 또는 프리팹이 없음");
         return;
      }
      Vector3 startPos = pattern3GrenadeSpawnPoint.position;
      Vector3 targetPos = _playerTransform.position;
        
       
      //GameObject grenade = Instantiate(pattern3GrenadeProjectilePrefab, startPos, Quaternion.identity);
      SoundManager.Instance.Play("InGame_EnemyBoss_ThrowingSFX");



// 🎯 3. 수류탄 생성
      GameObject grenade = ObjectPoolManager.Instance.GetFromPool(
         PoolType.Stage1BossSkillPattern3Proj01,
         pattern3GrenadeSpawnPoint.position,
         Quaternion.identity);

// 🎯 4. 컴포넌트 찾고 Init
      Enemy_Boss1_Pattern3_Projectile01 grenadeScript = grenade.GetComponent<Enemy_Boss1_Pattern3_Projectile01>();

      if (grenadeScript != null)
      {
         grenadeScript.Init(
            targetPos,
            grenadeThrowHeight,
            grenadeDuration,
            smokeTickDamage,
            smokeDamagePerTickTime,
            smokeRadius,
            smokeDuration,
            pattern3GrenadeSpawnPoint.position);
      }
      else
      {
         Debug.LogError("💥 Init 실패! GetComponent<Enemy_Boss1_Pattern3_Projectile01>() 결과가 null");
      }
   }
   
   private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
   {
      yield return new WaitForSeconds(delay);

      if (obj != null && obj.activeInHierarchy)
      {
         ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
      }
   }

   #endregion

   #region Scene창 사거리 기즈모 표시

   private void OnDrawGizmosSelected()
   {
#if UNITY_EDITOR
      if (pattern1BulletSpawnPoint != null)
      {
         // 🔴 패턴1 - 사정거리 원
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, pattern1AttackRange);

         // 🟡 3갈래 궤적 (플레이어 방향 기준)
         if (Application.isPlaying && _playerTransform != null)
         {
            Vector2 dir = (_playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Gizmos.color = Color.yellow;
            float[] offsets = { 0f, -spreadAngle, spreadAngle };

            foreach (float offset in offsets)
            {
               float angle = (baseAngle + offset) * Mathf.Deg2Rad;
               Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
               Vector3 endPos = pattern1BulletSpawnPoint.position + direction * pattern1AttackRange;
               Gizmos.DrawLine(pattern1BulletSpawnPoint.position, endPos);
            }
         }
      }

      // 🔵 패턴2 - 돌진 거리 선 (방향 표시용)
      Gizmos.color = Color.cyan;
      Gizmos.DrawLine(transform.position, transform.position + transform.right * pattern2DashRange);

      // 🟣 패턴2 - 돌진 가능 범위 원 (시전 조건 시각화)
      Gizmos.color = Color.magenta;
      Gizmos.DrawWireSphere(transform.position, pattern2DashRange);
      
      // 🔵 패턴3 - 사정거리 원
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, pattern3GrenadeRange);
#endif
   }

   #endregion
   
}
