using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private StageManager _stageManager;
    public PlayerStat stat = new();


    [Header("Player Controller")]
    public FloatingJoystick floatingJoystick;    
    private Vector3 moveVec;
    private Vector3 lastMoveDir = Vector2.right;
    public PlayerAnim PlayerAnim { get; private set; }
    [SerializeField] private const float _runSpeed = 10;


    [Header("DOTween Setting")]
    [SerializeField] private float _pullDuration = 0.3f;
    [SerializeField] private float pullSpeed = 10f;

    private int _itemLayer;

    public void Init(StageManager stageManager)
    {
        _stageManager = stageManager;
        stat.ResetStat();

        PlayerAnim = GetComponentInChildren<PlayerAnim>();
        _itemLayer = LayerMask.NameToLayer("Item");
    }

    private void Update()
    {
        PullItemsInRange();

        // 조이스틱에서 입력 값을 받아 옴
        float x = floatingJoystick.Horizontal;
        float y = floatingJoystick.Vertical;
        moveVec = new Vector3(x, y).normalized;

        //방향 저장 추가
        if (moveVec.sqrMagnitude > 0)
        {
            lastMoveDir = new Vector3(x, y).normalized;
        }

        //PlayerAnim.targetAnimators[(int)targetDirectType.forward].gameObject.SetActive(y > 0);
        //PlayerAnim.targetAnimators[(int)targetDirectType.backward].gameObject.SetActive(y > 0);

        PlayerAnim.targetType = y > 0 ? targetDirectType.backward : targetDirectType.forward;

        if (x > 0 || y > 0)
        {
            PlayerAnim.SetAni(stat.moveSpeed >= _runSpeed ? AniType.run : AniType.walk);
        }
        else
        {
            PlayerAnim.SetAni(AniType.idle);
        }

        // 이동 처리
        transform.position += stat.moveSpeed * Time.deltaTime * moveVec;

        // 회전 생략
    }

    // 경험치 획득 처리
    public void GainExp(float amount)
    {
        stat.currentExp += amount * stat.expGain;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateExpImage();

        while (stat.currentExp >= stat.requiredExp)
        {
            stat.currentExp -= stat.requiredExp;
            LevelUp();
        }
    }

    public Vector3 GetLastMoveDirection()
    {
        return lastMoveDir != Vector3.zero ? lastMoveDir : Vector3.right;
    }

    public void LevelUp()
    {
        Debug.Log($"[Player] 레벨 업! 현재 레벨: {stat.level}");

        stat.level++;        
        stat.requiredExp *= 1.1f;  // 경험치통 공식 추후 수정
        
        _stageManager.LevelUpEvent();

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateLevel();
    }

    public void Heal(float amount)
    {
        stat.currentHealth = Mathf.Min(stat.currentHealth + amount, stat.maxHealth);
    }

    public void TakeDamage(float amount)
    {
        //방어력 계산 공식 추후 수정
        float actualDamage = Mathf.Max(amount - stat.defense, 1f);

        stat.currentHealth = Mathf.Max(stat.currentHealth - actualDamage, 0f);
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();

        Debug.Log($"[Player] 체력: {stat.currentHealth}/{stat.maxHealth}");
    }

    private void PullItemsInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stat.pickupRange);
        foreach (var hit in hits)
        {
            if (hit.gameObject.layer != _itemLayer) continue;

            Transform itemTransform = hit.transform;

            // 이미 DOTween으로 이동 중이면 중복 이동 방지
            if (DOTween.IsTweening(itemTransform)) continue;

            // DOTween을 사용해 부드럽게 플레이어 쪽으로 이동
            Vector3 destination = transform.position;
            float distance = Vector3.Distance(itemTransform.position, destination);

            // 일정 거리 이상이면 더 빠르게 당김
            float duration = Mathf.Clamp(distance / pullSpeed, 0.1f, _pullDuration);

            itemTransform.DOMove(destination, duration)
                         .SetEase(Ease.InOutQuad)
                         .SetUpdate(true); // TimeScale 영향을 받지 않도록
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stat.pickupRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _itemLayer)
        {
            if (collision.TryGetComponent<Item>(out var item))
            {
                item.Use(this);
            }
        }
    }
}
