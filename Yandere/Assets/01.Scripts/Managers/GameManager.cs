using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /** @todo SaveSystem 추후 조정
    private AutoSaveSystem _autoSaveSystem;
    [SerializeField] private float autoSaveInterval = 30f;
    private float _timer;*/

    [Header("Stage Data")]
    public StageData[] stageDatas;
    [SerializeField] private int _maxStageIndex;
    public int MaxStageIndex
    {
        get => _maxStageIndex;
    }
    public StageData currentStageData;

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
        Debug.Log($"[GameManager] Loaded {_maxStageIndex} stages from Stages folder");
    }

    private void Start()
    {
        //_timer = 0f;
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

        Debug.Log($"[GameManager] Set Stage {currentStageData.stageIndex}");
    }

    public void LoadGameScene()
    {
        Debug.Log("[GameManager] Call Game Scene");

        SceneManager.LoadScene("Jubin_EnemyTest2");
    }

    public void LoadTitleScene()
    {
        Debug.Log("[GameManager] Call Title Scene");

        SceneManager.LoadScene("TitleScene");
    }

    public void LoadNextStage()
    {
        int nextIndex = currentStageData.stageIndex;

        if (nextIndex >= MaxStageIndex)
        {
            Debug.Log("[GameManager] Last Stage. Return to Title");

            LoadTitleScene();
            return;
        }

        nextIndex++;

        currentStageData = stageDatas[nextIndex - 1];

        Debug.Log($"[GameManager] Load Next Stage: {currentStageData.stageIndex}");

        LoadGameScene();
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
