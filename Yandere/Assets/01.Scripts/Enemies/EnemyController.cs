using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IDamagable, IEnemy
{
    private EnemySkill_Dash _dashSkill;
    public EnemyData enemyData;
    public float _monsterCurrentHealth;
    protected Transform _playerTransform;
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    private IEnemyAttack _attackModule;
    protected bool isPatterning = false;

    
    // 에너미 상태 체크
    public bool isInAttackRange = false;
    public bool isDead = false;
    
    private float attackTimer = 0f;
    

    // Item Drop 컴포넌트
    [SerializeField] private DropContext _dropContext;
    
    [Header("separationRadius 세팅")]
    [SerializeField] private float separationRadius = 0.5f;                 // 주변 탐지 반경
    [SerializeField] private float pushForce = 1.0f;                        // 밀어내는 힘
    [SerializeField] private LayerMask enemyLayer;                          // 레이어 구분

    public EnemyID EnemyId { get; private set; }
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        _dashSkill = GetComponent<EnemySkill_Dash>();
        
    }

    public virtual void SetEnemyId(EnemyID id)
    {
        EnemyId = id;
    }
    
    void FixedUpdate()
    {
        MonsterMove();
    }

    void Update()
    {
        if (isDead) return;

        if (_dashSkill != null)
        {
            if (PlayerInDashSkillRange() && !_dashSkill.IsDashing())
            {
                _dashSkill.SetCanUseDash(true);
            }
            else
            {
                _dashSkill.SetCanUseDash(false);
            }
        }

        // 공격 로직
        if (isInAttackRange)
        {
            if (attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            }

            if (attackTimer <= 0f)
            {
                MonsterAttack();
                attackTimer = enemyData.attackCooldown;
            }
        }
    }

    // DropContext를 세팅하는 메서드입니다
    public void SetDropContext(DropContext context)
    {
        _dropContext = context;
        _dropContext.position = transform.position;
    }

    #region 몬스터 이동 로직

    protected virtual void MonsterMove()
    {
        if (isDead || isPatterning || _playerTransform == null)
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

    #endregion

    #region 몬스터 어택 로직

    public virtual void MonsterAttack()
    {
        _animator.SetTrigger("Attack");
        if (isDead) return;
        
        float damage = enemyData.monsterAttack;
        _attackModule?.Attack(damage); // null 체크

    }

    #endregion

    #region 몬스터 피격 로직

    public virtual void TakeDamage(float damage)
    {
        SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
        if (isDead) return;                                                 // 죽은 상태이면 이코드를 빠져나가게 함

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        _monsterCurrentHealth -= damage;
        //Debug.Log($"[에너미컨트롤러] {enemyData.monsterName}가(이) {damage}의 피해를 입었습니다.");
        
        _animator.SetTrigger("Hit");                                  // 애니메이터의 파라미터(트리거) "Hit"를 실행
        
        if (_monsterCurrentHealth <= 0)                                     // 몬스터의 현재 체력이 0이면 아래 코드 실행
        {
            MonsterDie();
        }
    }

    #endregion

    #region 몬스터 다이 로직

    protected virtual void MonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행

        _dropContext.position = transform.position;
        StageManager.Instance.ItemDropManager.HandleDrop(_dropContext);

        if (gameObject.CompareTag("Enemy_Elite"))
        {
            QuestManager.Instance.eliteKillCount++;
        }

        StageManager.Instance.ChangeKillCount(1);
        StartCoroutine(DelayedReturnToPool(1f));

    }

    #endregion

    #region 몬스터 겹침 충돌 방지 로직

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

    #endregion
   

    

    
    
  
    
    public void UpdateSpriteDirection(Vector2 direction)
    {
        _spriteRenderer.flipX = direction.x < 0;
    }
    
    public void DelayAttack(float delay)
    {
        attackTimer = Mathf.Max(attackTimer, delay); // 현재 쿨보다 짧으면 덮지 않음
    }
    
    private bool PlayerInDashSkillRange()
    {
        if (_dashSkill == null || _playerTransform == null) return false;
        return Vector2.Distance(transform.position, _playerTransform.position) <= _dashSkill.GetDetectRadius();
    }

    protected IEnumerator DelayedReturnToPool(float amount)
    {
        yield return new WaitForSeconds(amount);
        ObjectPoolManager.Instance.ReturnEnemyToPool(EnemyId, gameObject);
    }

    #region  Scene창 사거리 기즈모 표시

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_dashSkill != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _dashSkill.GetDetectRadius());
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
#endif
  

    #endregion
  
    
}
