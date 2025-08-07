using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    
    [SerializeField] private GameObject[] mapPrefabs;

    [SerializeField] private Player player;
    public Player Player => player;
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

        Player.Init();
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

        UIManager.Instance.gameHUD.Init();
        
        _spawnManager.SetSpawnArea(currentStageData.spawnAreaMin, currentStageData.spawnAreaMax);
        StartCoroutine(Init());
        StartCoroutine(_spawnManager.HandleWave(currentSpawnData));
        StartCoroutine(_spawnManager.SpawnJarRoutine());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(1f);
        
        ChangeKillCount(0);
        ChangeGoldCount(0);
        Player.GainExp(5);
    }

    private void Update()
    {
        if (IsUIOpened)
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
            GameOver();
            return;
        }

        _elapsedTime += Time.deltaTime;

        UIManager.Instance.gameHUD.UpdateTime(_elapsedTime);

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
                StartCoroutine(_spawnManager.HandleWave(currentSpawnData));
            }
        }
    }

    public void ChangeKillCount (int amount)
    {
        KillCount += amount;
        UIManager.Instance.gameHUD.UpdateKillCount();
    }

    public void ChangeGoldCount (int amount)
    {
        GoldCount += amount;
        UIManager.Instance.gameHUD.UpdateGold();
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
        SoundManager.Instance.Play("InGame_Player_Die");
        SoundManager.Instance.StopBGM();
        
        UIManager.Instance.SetUIState(UIState.GameOver);
    }
}
