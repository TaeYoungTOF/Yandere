using System.Collections;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    private Transform _playerTransform;
    private EnemyController _enemyController;
    
    [SerializeField] private float enemyDashCooldown = 5f;
    [SerializeField] private float enemyDashTimer = 0f;

    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null)
        {
            Debug.Log("EnemyAttackHandler : EnemyController를 못찾음.");
        }
        
        // 플레이어 찾아서 기억해두기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("플레이어가 씬에 없음!");
        }
    }

    public void UpdateDashTimer()
    {
        if (enemyDashTimer > 0f)
        {
            enemyDashTimer -= Time.deltaTime;
        }
    }

    public bool CanDash()
    {
        return enemyDashTimer <= 0f;
    }

    public void ResetDashTimer()
    {
        enemyDashTimer = enemyDashCooldown;
    }
    
    
    public void MonsterAttack(EnemyAttackType attackType, float damage)
    {
        switch (attackType)
        {
            case EnemyAttackType.AttackType_A:
                DoTypeA_Attack(damage);
                break;
            case EnemyAttackType.AttackType_B:
                DoTypeB_Attack(damage);
                break;
            case EnemyAttackType.AttackType_C:
                DoTypeC_Attack(damage);
                break;
            default:
                Debug.LogWarning("Unknown attack type!");
                break;
        }
    }

    private void DoTypeA_Attack(float damage)
    {
        if (_playerTransform == null)
        {
            Debug.Log("EnemyAttackHandler A타입 : 플레이어가 없음!");
            return;
        }

        // 플레이어한테 데미지 주기
        StageManager.Instance.Player.TakeDamage(damage);
        Debug.Log($"공격타입A 성공! {damage} 데미지를 플레이어에게 줌");
        // 예: 근접 타격
    }

    private void DoTypeB_Attack(float damage)
    {
        Debug.Log($"Type B 공격! {damage}");
        // 예: 발사체 쏘기
    }

    private void DoTypeC_Attack(float damage)
    {
        if (_playerTransform == null)
        {
            Debug.Log("EnemyAttackHandler C타입 : 플레이어가 없음!");
            return;
        }
        
        StageManager.Instance.Player.TakeDamage(damage);
        if (CanDash())
        {
            Debug.Log("공격타입C 돌진 스킬 발동!");
            EnemyDashSkill();
            ResetDashTimer();
        }
        
        
        
        Debug.Log($"공격타입C 성공! {damage} 데미지를 플레이어에게 줌");
        // 예: 대쉬 공격
    }

    public void EnemyDashSkill()
    {
        if (_playerTransform == null) return;
        
        Vector2 dashDir = (_playerTransform.position - transform.position).normalized;
        Rigidbody2D _rigidbody2D = GetComponent<Rigidbody2D>();

        float dashForce = 10f;                                                              // 돌진 속도
        
        _enemyController.isDashing = true;
        _rigidbody2D.velocity = dashDir * dashForce;
        Debug.Log("[Dash] 돌진 시작!");

        StartCoroutine(StopDashAfter(0.3f));
    }

    IEnumerator StopDashAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _enemyController.isDashing = false;
        Debug.Log("[Dash] 돌진 끝!");
    }
}
