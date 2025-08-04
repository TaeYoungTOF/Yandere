using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /** @todo SaveSystem 추후 조정
    private AutoSaveSystem _autoSaveSystem;
    [SerializeField] private float autoSaveInterval = 30f;
    private float _timer;*/

    public float[] InGameData { get; private set; }

    [Header("Stage Data")]
    public StageData[] stageDatas;
    [SerializeField] private int _maxStageIndex;
    public int MaxStageIndex => _maxStageIndex;
    public StageData currentStageData;

    private bool _rewardPending;
    private float _pendingExp;
    private float _pendingGold;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        // StageData 개수만큼 _maxStageIndex 자동 설정
        stageDatas = Resources.LoadAll<StageData>("Stage");
        _maxStageIndex = stageDatas.Length;
        currentStageData = stageDatas[0];
        Debug.Log($"[GameManager] Loaded {_maxStageIndex} stages from Stages folder");
    }

    private void Start()
    {
        //_timer = 0f;
        //SoundManagerTest.Instance.Play("Title_BGM");
    }

    private void Update()
    {
        /** @새애 SaveSystem 추후 조정
        _timer += Time.unscaledDeltaTime;
        if (_timer >= autoSaveInterval)
        {
            _autoSaveSystem.AutoSave();
        }*/
    }

    public void SetStage(StageData stageData)
    {
        currentStageData = stageData;
    }

    public void LoadScene(SceneName sceneName)
    {
        switch (sceneName)
        {
            case SceneName.TitleScene:
                _rewardPending = true;
                _pendingExp = StageManager.Instance.Exp;
                _pendingGold = StageManager.Instance.GoldCount;
                break;
            case SceneName.GameScene:
                InGameData = DataManager.Instance.inGameDatas;
                break;
            default:
                Debug.LogError("[GameManager] Invalid scene name: " + sceneName);
                break;
        }
        
        //StartCoroutine(SceneLoader.Instance.LoadAsync(sceneName));
        SceneLoader.Instance.LoadAsync(sceneName);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleScene":
                SoundManager.Instance.Play("Title_BGM");

                if (_rewardPending)
                {
                    DataManager.Instance.CalculateReward(_pendingExp, _pendingGold);
                    _pendingExp = 0;
                    _pendingGold = 0;
                    _rewardPending = false;
                }
                break;
            case "GameScene":
                SoundManager.Instance.Play("Stage1_BGM");
                break;
        }
    }

    /** @todo SaveSystem 추후 조정
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _autoSaveSystem.SaveOnPauseOrQuit();
        }
    }

    private void OnApplicationQuit()
    {
        _autoSaveSystem.SaveOnPauseOrQuit();
    }*/

}
