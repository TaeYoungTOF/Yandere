using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    
    [SerializeField] GameObject[] mapPrefabs;

    public Player Player { get; private set; }
    private SpawnManager _spawnManager;
    public ItemDropManager ItemDropManager { get; private set; }
    public StageData currentStageData;
    public WaveData currentSpawnData;

    public int Exp { get; private set; }
    public int KillCount { get; private set; }
    public int GoldCount { get; private set; }


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
        _spawnManager = GetComponentInChildren<SpawnManager>();
        ItemDropManager = GetComponentInChildren<ItemDropManager>();

        currentStageData = GameManager.Instance.currentStageData;
        currentSpawnData = currentStageData.waveDatas[0];

        if (mapPrefabs[currentStageData.stageIndex] == null)
        {
            Debug.LogError("[StageManager] Map Prefab is Null!");
            return;
        }

        Instantiate(mapPrefabs[currentStageData.stageIndex], Vector3.zero, Quaternion.identity);

        _maxTime = currentStageData.clearTime;

        StartCoroutine(StartWaveRoutine(currentSpawnData));

        Exp = 0;
        ChangeKillCount(0);
        ChangeGoldCount(0);
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
            _spawnManager.StopSpawn();

            int nextIndex = currentStageData.waveDatas.IndexOf(currentSpawnData) + 1;
            if (nextIndex < currentStageData.waveDatas.Count)
            {
                currentSpawnData = currentStageData.waveDatas[nextIndex];
                StartCoroutine(StartWaveRoutine(currentSpawnData));
            }
        }
    }

    public void ChangeKillCount (int amount)
    {
        KillCount += amount;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateKillCount();
    }

    public void ChangeGoldCount (int amount)
    {
        GoldCount += amount;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateGold();
    }

    private IEnumerator StartWaveRoutine(WaveData spawnData)
    {
        yield return StartCoroutine(_spawnManager.HandleWave(spawnData));
    }

    public void StageClear()
    {
        Debug.Log($"[StageManager] {currentStageData.stageIndex} Stage Clear!!");

        QuestManager.Instance.isStageCleared = true;
        QuestManager.Instance.UpdateValue();
        
        switch (currentStageData.stageIndex)
        {
            case 1:
                Exp = 100;
                GoldCount += KillCount;
                break;
            case 2:
                Exp = 200;
                GoldCount += (int)(KillCount * 1.25f);
                break;
            case 3:
                Exp = 300;
                GoldCount +=(int)(KillCount * 1.5f);
                break;
            case 4:
                Exp = 400;
                GoldCount += (int)(KillCount * 1.75f);
                break;
            default:
                Exp = 50;
                GoldCount += 1000;
                break;
        }

        switch (QuestManager.Instance.ReturnClearedQuest())
        {
            case 1:
                GoldCount = (int)(GoldCount * 1.1f);
                break;
            case 2:
                GoldCount = (int)(GoldCount * 1.2f);
                break;
            case 3:
                GoldCount = (int)(GoldCount * 1.4f);
                break;
            default:
                Debug.Log("[StageManager] cleared Quest 0");
                break;
        }
        
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

    public float CalculateReward()
    {
        ChangeGoldCount(KillCount);

        return GoldCount;
    }
}
