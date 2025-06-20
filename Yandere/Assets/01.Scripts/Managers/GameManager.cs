using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MoneyManager MoneyManager { get; private set; }

    private AutoSaveSystem _autoSaveSystem;
    [SerializeField] private float autoSaveInterval = 30f;
    private float _timer;

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
    }

    private void Start()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.unscaledDeltaTime;
        if (_timer >= autoSaveInterval)
        {
            _autoSaveSystem.AutoSave();
        }
    }

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
    }

}
