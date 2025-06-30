using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IDamagable, IEnemy
{
    public EnemyData enemyData;
    private float monsterCurrentHealth;
    private Transform playerTransform;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    // 에너미 상태 체크
    private bool isInAttackRange = false;
    private bool isDead = false;
    
    private float attackTimer = 0f;
    
    // 공격 타입 컴포넌트
    private EnemyMeleeAttack _enemyMeleeAttack;
    private EnemyRangedAttack _enemyRangedAttack;

    // Item Drop 컴포넌트
    [SerializeField] private DropTable _dropTable;
    
    
    [Header("separationRadius 세팅")]
    [SerializeField] private float separationRadius = 0.5f;                 // 주변 탐지 반경
    [SerializeField] private float pushForce = 1.0f;                        // 밀어내는 힘
    [SerializeField] private LayerMask enemyLayer;                          // 레이어 구분
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        _enemyRangedAttack = GetComponent<EnemyRangedAttack>();
    }
    
    public void SetAttackRange(bool inRange)
    {
        isInAttackRange = inRange;
    }
    
    void Start()
    {
        monsterCurrentHealth = enemyData.monsterMaxHp;                      // 최대체력을 현재체력에 넣어줌
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");     //씬에 존재하는 "player"태그를 가진 오브젝트를 player 변수에 넣어줌
        if (player != null)
        {
            playerTransform = player.transform;                             // 찾은 플레이어의 트랜스폼을 playerTransform 변수에 값을 넣어줌
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

       
        if (isInAttackRange)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                MonsterAttack();
                attackTimer = enemyData.attackCooldown; // 쿨 리셋
            }
        }
    }
    public void MonsterMove()
    {
       
        if (isDead) return;                                                 // 죽은 상태이면 이 코드를 빠져 나가게 함
        
        if (playerTransform == null) return;                                // 플레이어트랜스폼이 Null이면 리턴

        if (isInAttackRange)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        else
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;      // 플레이어의 위치 - 몬스터의 위치의 값을 direction에 넣어줌.

            _rigidbody2D.velocity = direction * enemyData.monsterMoveSpeed; // direction(벡터값) * 몬스터 스피드를 넣어 줌

            if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
            
            
        }
        AvoidOtherEnemies();
        
    }

    public void MonsterAttack()
    {
        switch (enemyData.enemyAttackType)
        {
            case EnemyAttackType.Melee:
                if (_enemyMeleeAttack != null)
                    _enemyMeleeAttack.DoAttack(enemyData.monsterAttack);
                break;

            // case EnemyAttackType.Ranged:
            //     if (_enemyRangedAttack != null)
            //         _enemyRangedAttack.DoAttack(enemyData.monsterAttack);
            //     break;

            default:
                Debug.Log("어택 타입이 선택되지 않았어요");
                break;
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;                                                 // 죽은 상태이면 이코드를 빠져나가게 함
        
        monsterCurrentHealth -= damage;
        Debug.Log($"[EnemyController] {enemyData.monsterName}이 {damage}의 피해를 입었습니다.");
        _animator.SetTrigger("Hit");                                  // 애니메이터의 파라미터(트리거) "Hit"를 실행
        if (monsterCurrentHealth <= 0)                                      // 몬스터의 현재 체력이 0이면 아래 코드 실행
        {
            MonsterDie();
        }
    }

    public void MonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                 // 애니메이터의 파라미터(트리거) "Dead"를 실행

        var context = new DropContext
        {
            Position = transform.position,
            DropTable = _dropTable
        };

        ItemDropManager.Instance.HandleDrop(context);

        Destroy(gameObject, 1.0f);
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
