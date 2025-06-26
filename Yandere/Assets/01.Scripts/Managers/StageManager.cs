using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public StageData currentStageData;
    public Player Player { get; private set; }
    private SpawnManager _spawnManager;
    public bool IsUIOpened = false;


    [SerializeField] private PlayerManager playerManager;
    public PlayerManager PlayerManager => playerManager;
    private LevelUpManager _levelUpManager;

    /** 임시코드*/
    [SerializeField] private UI_StageClear _stageClearUI;
    [SerializeField] private UI_Timer _uiTimer;

    [Header("Timer")]
    private float _elapsedTime = 0f;
    private const float _maxTime = 15 * 60f; // 15분

    public int ElapsedMinutes => Mathf.FloorToInt(_elapsedTime / 60f);
    public int ElapsedSeconds => Mathf.FloorToInt(_elapsedTime % 60f);
    public float ElapsedTime => _elapsedTime;


    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {        
        Player = FindObjectOfType<Player>();
        _spawnManager = GetComponentInChildren<SpawnManager>();

        currentStageData = GameManager.Instance.currentStageData;

        _levelUpManager = GetComponent<LevelUpManager>();

        StartWave();
    }

    private void Update()
    {
        if (IsUIOpened)
        {
            Time.timeScale = 0f;

            return;
        }

        if (_elapsedTime < _maxTime)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _maxTime)
                _elapsedTime = _maxTime;

            _uiTimer.UpdateTime(ElapsedMinutes, ElapsedSeconds);
        }
        // 추가 기능

    }

    private void StartWave()
    {
        StartCoroutine(_spawnManager.SpawnRoutine());
    }

    public void PlayerLevelUp()
    {
        Debug.Log("[StageManager] Player Level Up!");
        
        _levelUpManager.OnLevelUp();
    }

    public void StageClear()
    {
        Debug.Log("[StageManager] Stage Clear!!");

        _stageClearUI.CallStageClearUI();
    }
}
