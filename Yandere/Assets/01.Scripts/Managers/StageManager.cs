using System;
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

    public bool IsUIOpened = false;


    [Header("Timer")]
    [SerializeField] private float _maxTime;
    private float _elapsedTime = 0f;
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
        Player.Init(this);

        SpawnManager = GetComponentInChildren<SpawnManager>();
        ItemDropManager = GetComponentInChildren<ItemDropManager>();

        currentStageData = GameManager.Instance.currentStageData;
        currentSpawnData = currentStageData.spwanDatas[0];

        _maxTime = currentStageData.clearTime;

        StartCoroutine(StartWaveRoutine(currentSpawnData));
    }

    private void Update()
    {
        if (IsUIOpened)
        {
            Time.timeScale = 0f;

            return;
        }

        if (Player.stat.currentHealth <= 0)
        {
            Time.timeScale = 0f;
            GameOver();
            return;
        }

        Time.timeScale = 1f;

        if (_elapsedTime < _maxTime)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _maxTime)
                _elapsedTime = _maxTime;


            UIManager.Instance.GetPanel<UI_GameHUD>().UpdateTime(_elapsedTime);
        }

        if (currentSpawnData.endTime <= _elapsedTime)
        {
            SpawnManager.StopSpawn();

            int nextIndex = currentStageData.spwanDatas.IndexOf(currentSpawnData) + 1;
            if (nextIndex < currentStageData.spwanDatas.Count)
            {
                currentSpawnData = currentStageData.spwanDatas[nextIndex];
                StartCoroutine(StartWaveRoutine(currentSpawnData));
            }
            else
            {
                Debug.Log("[StageManager] All Spawn Event End");
            }
        }
    }

    private IEnumerator StartWaveRoutine(WaveData spawnData)
    {
        yield return StartCoroutine(SpawnManager.HandleWave(spawnData));
    }

    public void StageClear()
    {
        Debug.Log($"[StageManager] {currentStageData.stageIndex} Stage Clear!!");

        UIManager.Instance.SetUIState(UIState.StageClear);

        Player.PlayerController.PlayerAnim.SetAni(AniType.win);
    }

    public void GameOver()
    {
        Debug.Log("[StageManager] Game Over");

        UIManager.Instance.SetUIState(UIState.GameOver);

        Player.PlayerController.PlayerAnim.SetAni(AniType.lose);
    }

    public void LevelUpEvent()
    {
        Debug.Log("[StageManager] Call Level Up Event");

        UIManager.Instance.SetUIState(UIState.SkillSelect);
    }
}
