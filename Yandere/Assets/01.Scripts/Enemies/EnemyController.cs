using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IDamagable, IEnemy
{
    public EnemyData enemyData;
    public float _monsterCurrentHealth;
    protected Transform _playerTransform;
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    private EnemyAttackHandler _attackHandler;
    private IEnemyAttack _attackModule;
    protected bool isPatterning = false;

    
    // 에너미 상태 체크
    public bool isInAttackRange = false;
    public bool isDead = false;
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
    
    protected virtual void Start()
    {
        _monsterCurrentHealth = enemyData.monsterMaxHp;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;

        _attackModule = GetComponent<IEnemyAttack>();
    }
    void FixedUpdate()
    {
        MonsterMove();
    }

    void Update()
    {
        if (isDead) return;

        if (isInAttackRange)                            // 공격 범위(HitBox)안에 플레이어가 있으면
        {
            if (attackTimer > 0f)                       // 어택타이머가 0보다 크면
            {
                attackTimer -= Time.deltaTime;          // 어택타이머의 시간을 감소 시킴
            }

            if (attackTimer <= 0f)                      // 어택타이머가 0보다 같거나 작으면
            {
                MonsterAttack();                        // 몬스터어택을 실행하고
                attackTimer = enemyData.attackCooldown; // 어택타이머를 다시 값을 넣어줌 (쿨타임 리셋!)
            }
            
        }
        
    }

    // DropContext를 세팅하는 메서드입니다
    public void SetDropContext(DropContext context)
    {
        _dropContext = context;
        _dropContext.position = transform.position;
    }

    protected virtual void MonsterMove()
    {
        if (isDead || isDashing || isPatterning || _playerTransform == null)
        {
            _rigidbody2D.velocity = Vector2.zero;       // 움직임 완전 정지
            _animator.SetBool("Run", false);            // 애니메이션도 정지
            return;
        }

        Vector2 direction = (_playerTransform.position - transform.position).normalized;

        if (isInAttackRange)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetBool("Run", false);
        }
        else
        {
            _rigidbody2D.velocity = direction * enemyData.monsterMoveSpeed;
            _animator.SetBool("Run", _rigidbody2D.velocity.magnitude > 0.1f);
        }

        UpdateSpriteDirection(direction);
        AvoidOtherEnemies();
    }

    public void MonsterAttack()
    {
        _animator.SetTrigger("Attack");
        if (isDead) return;
        
        float damage = enemyData.monsterAttack;
        _attackModule?.Attack(damage); // null 체크

    }
    
    public virtual void TakeDamage(float damage)
    {
        SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
        if (isDead) return;                                                 // 죽은 상태이면 이코드를 빠져나가게 함

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        _monsterCurrentHealth -= damage;
        Debug.Log($"[에너미컨트롤러] {enemyData.monsterName}가(이) {damage}의 피해를 입었습니다.");
        
        _animator.SetTrigger("Hit");                                  // 애니메이터의 파라미터(트리거) "Hit"를 실행
        
        if (_monsterCurrentHealth <= 0)                                     // 몬스터의 현재 체력이 0이면 아래 코드 실행
        {
            MonsterDie();
        }
    }

    protected virtual void MonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행

        _dropContext.position = transform.position;
        StageManager.Instance.ItemDropManager.HandleDrop(_dropContext);
        //StageManager.Instance.TargetKillCount();

        StageManager.Instance.ChangeKillCount(1);
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
    
    public void UpdateSpriteDirection(Vector2 direction)
    {
        _spriteRenderer.flipX = direction.x < 0;
    }
    
    public void DelayAttack(float delay)
    {
        attackTimer = Mathf.Max(attackTimer, delay); // 현재 쿨보다 짧으면 덮지 않음
    }
    
}
