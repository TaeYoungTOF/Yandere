using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IDamagable, IEnemy
{
    public EnemyData enemyData;
    private float _monsterCurrentHealth;
    private Transform _playerTransform;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EnemyAttackHandler _attackHandler;
    
    // 에너미 상태 체크
    private bool isInAttackRange = false;
    private bool isDead = false;
    public bool isDashing = false;
    
    private float attackTimer = 0f;
    

    // Item Drop 컴포넌트
    [SerializeField] private DropContext _dropContext;
    
    [Header("separationRadius 세팅")]
    [SerializeField] private float separationRadius = 0.5f;                 // 주변 탐지 반경
    [SerializeField] private float pushForce = 1.0f;                        // 밀어내는 힘
    [SerializeField] private LayerMask enemyLayer;                          // 레이어 구분
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _attackHandler = GetComponent<EnemyAttackHandler>();
    }
    
    public void SetAttackRange(bool inRange)
    {
        isInAttackRange = inRange;                                          // EnemyHitBox에서 OnTriggerEnter2D 상태로 "true" 또는 "false"값을 inRange로 받아서 isInAttackRange에 넣어주기
    }
    
    void Start()
    {
        _monsterCurrentHealth = enemyData.monsterMaxHp;                      // 최대체력을 현재체력에 넣어줌
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");     //씬에 존재하는 "player"태그를 가진 오브젝트를 player 변수에 넣어줌
        if (player != null)
        {
            _playerTransform = player.transform;                             // 찾은 플레이어의 트랜스폼을 playerTransform 변수에 값을 넣어줌
        }
        else
        {
            Debug.Log("EnemyController : 플레이어가 지금 Null 상태 입니다.");
        }
    }
    void FixedUpdate()
    {
        MonsterMove();
    }

    void Update()
    {
        // ✅ 테스트: H 키 누르면 데미지 주기
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("테스트: H 키로 데미지!");
            TakeDamage(10f);
        }
      
        if (isDead) return;

        if (isInAttackRange)                            // 공격 범위(HitBox)안에 플레이어가 있으면
        {
            if (attackTimer > 0f)                       // 어택타이머가 0보다 크면
            {
                attackTimer -= Time.deltaTime;          // 어택타이머의 시간을 감소 시킴
            }

            if (attackTimer <= 0f)                      // 어택타이머가 0보다 같거나 작으면
            {
                Debug.Log("몬스터 어택 실행 전");
                MonsterAttack();                        // 몬스터어택을 실행하고
                attackTimer = enemyData.attackCooldown; // 어택타이머를 다시 값을 넣어줌 (쿨타임 리셋!)
            }
            
            // _attackHandler.UpdateDashTimer();
            //
            // if (_attackHandler.CanDash() && enemyData.enemyAttackType == EnemyAttackType.AttackType_C)
            // {
            //     Debug.Log("EnemyController: 돌진 스킬 발동!");
            //     _attackHandler.EnemyDashSkill();
            //     _attackHandler.ResetDashTimer();
            // }
        }
        else
        {
            // attackTimer = 0; ❌ 이거 절대 리셋하지 말기!
        }
    }

    // DropContext를 세팅하는 메서드입니다
    public void SetDropContext(DropContext context)
    {
        _dropContext = context;
        _dropContext.position = transform.position;
    }

    public void MonsterMove()
    {
       
        if (isDead) return;                                                 // 죽은 상태이면 이 코드를 빠져 나가게 함
        if (isDashing) return;                                              // 돌진 중이면 기본 이동 금지
        if (_playerTransform == null) return;                               // 플레이어트랜스폼이 Null이면 리턴
        

        if (isInAttackRange)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("Run", false);
        }

        else
        {
            Vector2 direction = (_playerTransform.position - transform.position).normalized;      // 플레이어의 위치 - 몬스터의 위치의 값을 direction에 넣어줌.

            _rigidbody2D.velocity = direction * enemyData.monsterMoveSpeed;                          // direction(벡터값) * 몬스터 스피드를 넣어 줌

            if (_rigidbody2D.velocity.magnitude > 0.1f)                                              // 몬스터의 _rigidbody2D.velocity의 값이 0.1 보다 크면
            {
                _animator.SetBool("Run", true);                                                 // 몬스터의 애니메이션을 Run 상태로 변경
            }
            else                                                                                     // 몬스터의 _rigidbody2D.velocity의 값이 0.1 보다 작으면
            {
                _animator.SetBool("Run", false);                                                // 몬스터의 애니메이션을 Idle로 변경
            }
            
            if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;                                                        // x의 값이 0보다 크면 flipX를 true로 
            }
            else
            {
                _spriteRenderer.flipX = false;                                                       // x의 값이 0보다 작으면 flipX를 false로 변경 (스프라이트 방향 전환) 
            }
            
            
        }
        AvoidOtherEnemies();
        
    }

    public void MonsterAttack()
    {
        _animator.SetTrigger("Attack");
        if (isDead) return;
        
        EnemyAttackType attackType = enemyData.enemyAttackType;
        float damage = enemyData.monsterAttack;
        
        _attackHandler.MonsterAttack(attackType, damage);

    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;                                                 // 죽은 상태이면 이코드를 빠져나가게 함

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        
        _monsterCurrentHealth -= damage;
        Debug.Log($"[EnemyController] {enemyData.monsterName}가(이) {damage}의 피해를 입었습니다.");
        _animator.SetTrigger("Hit");                                  // 애니메이터의 파라미터(트리거) "Hit"를 실행
        if (_monsterCurrentHealth <= 0)                                     // 몬스터의 현재 체력이 0이면 아래 코드 실행
        {
            MonsterDie();
        }
    }

    public void MonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행

        _dropContext.position = transform.position;
        StageManager.Instance.ItemDropManager.HandleDrop(_dropContext);
        StageManager.Instance.TargetKillCount();

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateKillCount(1);
        Destroy(gameObject, 1.0f);
        
        RandomAchievementManager.Instance.UpdateProgress(ConditionType.TotalMonsterKills, 1); // 업적에서 몬스터 죽음 체크
    }

    void AvoidOtherEnemies()
    {
        Collider2D[] nearEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);

        Vector2 pushDir = Vector2.zero;

        foreach (var enemy in nearEnemies)
        {
            if (enemy.gameObject == gameObject) continue;

            Vector2 diff = transform.position - enemy.transform.position;
            if (diff.magnitude > 0)
                pushDir += diff.normalized / diff.magnitude;
        }

        _rigidbody2D.velocity += pushDir * pushForce;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
