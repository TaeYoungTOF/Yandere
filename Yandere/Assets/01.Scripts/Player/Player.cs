using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private StageManager _stageManager;
    public PlayerStat stat = new();
    private int _itemLayer;


    [Header("Player Controller")]
    public FloatingJoystick floatingJoystick;
    private Vector3 moveVec;
    public Vector3 MoveVec { get; private set; }
    private Vector3 lastMoveDir = Vector2.right;
    public PlayerAnim PlayerAnim { get; private set; }


    [Header("DOTween Setting")]
    [SerializeField] private float _pullDuration = 1f;
    [SerializeField] private float _pullSpeed = 1f;


    public void Init(StageManager stageManager)
    {
        _stageManager = stageManager;
        stat.ResetStats();

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

        // 방향 계산 및 애니메이션 적용
        if (moveVec.sqrMagnitude > 0)
        {
            lastMoveDir = moveVec;

            var direction = GetDirectionFromVector(moveVec);
            PlayerAnim.SetDirection(direction);
            PlayerAnim.SetAni(AniType.Move);
        }
        else
        {
            PlayerAnim.SetAni(AniType.Idle);
        }
    }

    private void FixedUpdate()
    {
        // 이동 처리
        transform.position += stat.FinalMoveSpeed * Time.fixedDeltaTime * moveVec;
    }

    private targetDirectType GetDirectionFromVector(Vector3 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return dir.x > 0 ? targetDirectType.right : targetDirectType.left;
        }
        else
        {
            return dir.y > 0 ? targetDirectType.backward : targetDirectType.forward;
        }
    }

    public Vector3 GetLastMoveDirection()
    {
        return lastMoveDir != Vector3.zero ? lastMoveDir : Vector3.right;
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

    public void LevelUp()
    {
        Debug.Log($"[Player] 레벨 업! 현재 레벨: {stat.level}");

        stat.level++;
        stat.requiredExp += 2f;

        _stageManager.LevelUpEvent();

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateLevel();
    }

    public void Heal(float amount)
    {
        stat.ChangeCurrentHp(amount);

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();
    }

    public void TakeDamage(float amount)
    {
        float actualDamage = amount * (1 - (stat.FinalDef / (stat.FinalDef + 500)));
        stat.ChangeCurrentHp(actualDamage);

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();
    }

    private void PullItemsInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stat.FinalPickupRadius);
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
            float duration = Mathf.Clamp(distance / _pullSpeed, 0.1f, _pullDuration);

            itemTransform.DOMove(destination, duration)
                         .SetEase(Ease.InOutQuad)
                         .SetUpdate(true); // TimeScale 영향을 받지 않도록
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stat.FinalPickupRadius);
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
