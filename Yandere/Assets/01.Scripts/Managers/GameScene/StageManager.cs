using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public Player Player { get; private set; }
    public SpawnManager SpawnManager { get; private set; }
    public ItemDropManager ItemDropManager { get; private set; }
    public StageData currentStageData;
    public WaveData currentSpawnData;

    private bool isStageCleared;
    public bool IsStageCleared => isStageCleared;
    
    private bool hasPlayerBeenHit;
    public bool HasPlayerBeenHit => hasPlayerBeenHit;
    
    private int killCount = 0;
    public int KillCount => killCount; 

    
    public bool IsUIOpened = false;

    public float timeScale = 1f;


    [Header("Timer")]
    [SerializeField] private float _maxTime;
    private float _elapsedTime = 0f;
    public float ElapsedTime => _elapsedTime;

    [Header("Global")]
    [SerializeField] private float _globalPlayerDamageMultiplier = 1;
    public float GlobalPlayerDamageMultiplier => _globalPlayerDamageMultiplier;

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

        Player = FindObjectOfType<Player>();
        Player.Init(this);
    }

    private void Start()
    {

        SpawnManager = GetComponentInChildren<SpawnManager>();
        ItemDropManager = GetComponentInChildren<ItemDropManager>();

        currentStageData = GameManager.Instance.currentStageData;
        currentSpawnData = currentStageData.waveDatas[0];

        _maxTime = currentStageData.clearTime;

        StartCoroutine(StartWaveRoutine(currentSpawnData));

        Player.GainExp(5);
    }

    private void Update()
    {
        // Achievement UI는 게임을 멈추지 않도록 예외 처리
        if (IsUIOpened/** && UIManager.Instance._currentState != UIState.Achievement*/)
        {
            Time.timeScale = 0f;
            return;
        }


        if (Player.stat.CurrentHp <= 0)
        {
            Time.timeScale = 0f;
            GameOver();
            return;
        }

        Time.timeScale = timeScale;

        if (_elapsedTime >= _maxTime)
        {
            Debug.Log("[StageManager] Time Over");
            GameOver();
            return;
        }

        _elapsedTime += Time.deltaTime;

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateTime(_elapsedTime);

        if (!currentSpawnData)
        {
            Debug.Log("[StageManager] current Spawn Data is null");
            return;
        }

        if (currentSpawnData.endTime <= _elapsedTime)
        {
            SpawnManager.StopSpawn();

            int nextIndex = currentStageData.waveDatas.IndexOf(currentSpawnData) + 1;
            if (nextIndex < currentStageData.waveDatas.Count)
            {
                currentSpawnData = currentStageData.waveDatas[nextIndex];
                StartCoroutine(StartWaveRoutine(currentSpawnData));
            }
        }
    }

    private IEnumerator StartWaveRoutine(WaveData spawnData)
    {
        yield return StartCoroutine(SpawnManager.HandleWave(spawnData));
    }

    public void StageClear()
    {
        isStageCleared = true;
        Debug.Log($"[StageManager] {currentStageData.stageIndex} Stage Clear!!");
        

        UIManager.Instance.SetUIState(UIState.StageClear);
    }

    private void GameOver()
    {
        UIManager.Instance.SetUIState(UIState.GameOver);
    }

    public void LevelUpEvent()
    {
        UIManager.Instance.SetUIState(UIState.SkillSelect);
    }

    public void OnPlayerHit()
    {
        hasPlayerBeenHit = true;
    }

    /*public void TargetKillCount()
    {
        killCount++;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateKillCount(killCount);
    }*/
}
