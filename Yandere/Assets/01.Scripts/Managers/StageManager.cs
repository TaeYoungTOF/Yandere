using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public StageData currentStageData;
    public Player Player { get; private set; }
    private SpawnManager _spawnManager;
    public bool IsUIOpened = false;


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
        Player.Init(this);

        _spawnManager = GetComponentInChildren<SpawnManager>();

        currentStageData = GameManager.Instance.currentStageData;

        StartWave();
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
        }

        Time.timeScale = 1f;

        if (_elapsedTime < _maxTime)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _maxTime)
                _elapsedTime = _maxTime;

            
            UIManager.Instance.GetPanel<UI_GameHUD>().UpdateTime(ElapsedMinutes, ElapsedSeconds);
        }

    }

    private void StartWave()
    {
        StartCoroutine(_spawnManager.SpawnRoutine());
    }

    public void StageClear()
    {
        Debug.Log($"[StageManager] {currentStageData.stageIndex} Stage Clear!!");

        UIManager.Instance.SetUIState(UIState.StageClear);
    }

    public void GameOver()
    {
        Debug.Log("[StageManager] Game Over");

        UIManager.Instance.SetUIState(UIState.GameOver);
    }

    public void LevelUpEvent()
    {
        Debug.Log("[StageManager] Call Level Up Event");

        UIManager.Instance.SetUIState(UIState.SkillSelect);
    }
}
