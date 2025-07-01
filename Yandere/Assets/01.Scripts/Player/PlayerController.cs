using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;    
    private Vector3 moveVec;
    private Vector3 lastMoveDir = Vector2.right;

    private Rigidbody2D _rigidbody;
    public PlayerAnim PlayerAnim { get; private set; }
    private PlayerStat _stat;
    [SerializeField] private const float _runSpeed = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponentInChildren<PlayerAnim>();
        _stat = StageManager.Instance.Player.stat;
    }

    private void Update()
    {
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
            PlayerAnim.SetAni(_stat.moveSpeed >= _runSpeed ? AniType.run : AniType.walk);
        }
        else
        {
            PlayerAnim.SetAni(AniType.idle);
        }

        // 이동 처리
        transform.position += _stat.moveSpeed * Time.deltaTime * moveVec;

        // 회전 생략
    }

    public Vector3 GetLastMoveDirection()
    {
        return lastMoveDir != Vector3.zero ? lastMoveDir : Vector3.right;
    }
}
