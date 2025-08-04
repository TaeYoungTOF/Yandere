using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D  _collider2D;
    private StageManager _stageManager;
    private int _itemLayer;
    
    public PlayerStat stat = new();
    public bool isBlinded = false;

    [Header("Player Controller")]
    public FloatingJoystick floatingJoystick;
    private Vector3 _moveVec;
    private Vector3 _lastMoveVec;
    public PlayerAnim PlayerAnim { get; private set; }

    [Header("Level up")]
    private bool _isLeveling = false;
    private Queue<int> _levelUpQueue = new();

    [Header("Stun Debuff")]
    [SerializeField] private GameObject stunEffectOjbect;

    [Header("DOTween Setting")]
    [SerializeField] private float _pullDuration = 1f;
    [SerializeField] private float _pullSpeed = 1f;

    [Header("Debug")]
    [SerializeField] private bool _GodMod = false;

    public void Init(StageManager stageManager)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        _stageManager = stageManager;
        _itemLayer = LayerMask.NameToLayer("Item");
        PlayerAnim = GetComponentInChildren<PlayerAnim>();
        
        stat.ResetStats();
        GetDataFromGameManager();
        stat.UpdateStats();
    }

    private void GetDataFromGameManager()
    {
        float[] data = GameManager.Instance.InGameData;
        stat.GetBonusAtkPer(data[1]);
        stat.GetBonusHp(data[2]);
        stat.GetBonusHpRegen(data[3]);
        stat.GetBounusDef(data[4]);
        stat.GetBonusCrit(data[5]);
        stat.GetBonusCritDmg(data[6]);
        stat.GetBonusMoveSpeed(data[7]);
        stat.GetBonusPickupRadius(data[8]);
        stat.GetBonusCoolDown(data[9]);
    }

    private void Update()
    {
        if (isBlinded)
        {
            _moveVec = Vector3.zero;           // ✅ 입력도 막고
            PlayerAnim.SetAni(AniType.Idle);   // ✅ 애니메이션도 Idle 고정
            return;
        } 
        
        PullItemsInRange();

        // 조이스틱에서 입력 값을 받아 옴
        float x = floatingJoystick.Horizontal;
        float y = floatingJoystick.Vertical;
        _moveVec = new Vector3(x, y).normalized;

        // 방향 계산 및 애니메이션 적용
        if (_moveVec.sqrMagnitude > 0)
        {
            _lastMoveVec = _moveVec;

            var direction = GetDirectionFromVector(_moveVec);
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
        if (_moveVec == Vector3.zero) return;

        float moveDistance = stat.FinalMoveSpeed * Time.fixedDeltaTime;
        Vector2 currentPos = _rigidbody2D.position;
        Vector2 targetDir = _moveVec.normalized;
        Vector2 targetPos = currentPos + targetDir * moveDistance;

        RaycastHit2D hit = Physics2D.Raycast(currentPos, targetDir, moveDistance + _collider2D.radius, LayerMask.GetMask("Map"));
        Color rayColor = hit.collider ? Color.red : Color.green;
        Debug.DrawRay(currentPos, targetDir * (moveDistance + _collider2D.radius), rayColor, 0.05f);

        if (hit.collider != null)
        {
            Vector2 closestPoint = hit.collider.ClosestPoint(targetPos);
            float distanceToCollider = Vector2.Distance(closestPoint, targetPos);

            if (distanceToCollider < _collider2D.radius)
            {
                return;
            }
        }

        _rigidbody2D.MovePosition(targetPos);
        QuestManager.Instance.lastMoveTime = Time.time;
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
        return _lastMoveVec != Vector3.zero ? _lastMoveVec : Vector3.forward;
    }

    private void PullItemsInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stat.FinalPickupRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject.layer != _itemLayer) continue;

            Transform itemTransform = hit.transform;

            if (hit.TryGetComponent<Item>(out var item))
            {
                if (!item.CanPickup()) return;
            }

            // 이미 DOTween으로 이동 중이면 중복 이동 방지
            if (DOTween.IsTweening(itemTransform)) continue;

            // DOTween을 사용해 부드럽게 플레이어 쪽으로 이동
            Vector3 destination = transform.position;
            float distance = Vector3.Distance(itemTransform.position, destination);

            // 일정 거리 이상이면 더 빠르게 당김
            float duration = Mathf.Clamp(distance / _pullSpeed, 0.1f, _pullDuration);

            itemTransform.DOMove(destination, duration)
                .SetEase(Ease.InOutQuad)
                .SetLink(itemTransform.gameObject);
        }
    }

    // 경험치 획득 처리
    public void GainExp(float amount)
    {
        stat.currentExp += amount * stat.expGain;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateExpImage();

        while (stat.currentExp >= stat.requiredExp)
        {
            stat.currentExp -= stat.requiredExp;
            _levelUpQueue.Enqueue(1);
        }

        if (!_isLeveling)
            StartCoroutine(LevelUp());
    }

    private IEnumerator LevelUp()
    {
        _isLeveling = true;

        while (_levelUpQueue.Count > 0)
        {
            _levelUpQueue.Dequeue();

            stat.level++;
            stat.requiredExp += 1f;

            _stageManager.LevelUpEvent();
            UIManager.Instance.GetPanel<UI_GameHUD>().UpdateLevel();

            yield return new WaitForSeconds(0.1f);
        }

        _isLeveling = false;
        SkillManager.Instance.isLevelUp = true;
    }

    public void Heal(float amount)
    {
        stat.ChangeCurrentHp(amount);

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();
    }

    public void TakeDamage(float amount)
    {
        if (_GodMod) return;
        
        float actualDamage = amount * (1 - (stat.FinalDef / (stat.FinalDef + 500)));
        stat.ChangeCurrentHp(-actualDamage);

        QuestManager.Instance.lastDamageTime = Time.time;
        
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();
    }

    public void ShowStunEffect(bool isOn)
    {
        if(stunEffectOjbect != null)
            stunEffectOjbect.SetActive(isOn);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _itemLayer)
        {
            if (collision.TryGetComponent<Item>(out var item))
            {
                if (!item.CanPickup()) return;
                
                DOTween.Kill(collision.gameObject);
                item.Use(this);
            }
        }
    }
}
