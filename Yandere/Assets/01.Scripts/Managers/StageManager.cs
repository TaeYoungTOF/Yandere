using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public StageData currentStageData;
    public Player Player { get; private set; }
    private SpawnManager _spawnManager;
    public bool IsUIOpened = false;

    /** 임시코드*/
    //[SerializeField] private UI_StageClear _stageClearUI;
    [SerializeField] private UI_SkillSelect _skillSelectUI;
    public List<BaseSkill> allSkills;
    /*UIManager로 이관*/

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

        if (_elapsedTime < _maxTime)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _maxTime)
                _elapsedTime = _maxTime;

            
            //UIManager.Instance.GetPanel<UI_GameHUD>().UpdateTime(ElapsedMinutes, ElapsedSeconds);
        }

    }

    private void StartWave()
    {
        StartCoroutine(_spawnManager.SpawnRoutine());
    }

    public void StageClear()
    {
        Debug.Log("[StageManager] Stage Clear!!");

        //_stageClearUI.CallStageClearUI();
        UIManager.Instance.GetPanel<UI_StageClear>().CallStageClearUI();
    }

    public void GameOver()
    {
        Debug.Log("[StageManager] Game Over");

        // GameOver UI 부르기
    }

    public void LevelUpEvent()
    {
        Debug.Log("[StageManager] Call Level Up Event");        
        
        var options = GetRandomSkillOptions(3);
        _skillSelectUI.Show(options);
    }

    private List<BaseSkill> GetRandomSkillOptions(int count)
    {
        List<BaseSkill> available = new List<BaseSkill>();
        foreach (var skill in allSkills)
        {
            if (!FindObjectOfType<SkillManager>().equippedSkills.Contains(skill) || skill.level < 5)
                available.Add(skill);
        }

        List<BaseSkill> result = new List<BaseSkill>();
        for (int i = 0; i < count; i++)
        {
            if (available.Count == 0) break;
            int rand = Random.Range(0, available.Count);
            result.Add(available[rand]);
            available.RemoveAt(rand);
        }
        return result;
    }
}
