using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public StageData currnetStageData;


    [SerializeField] private PlayerManager playerManager;
    public PlayerManager PlayerManager => playerManager;
    private LevelUpManager _levelUpManager;

    /** 임시코드*/
    [SerializeField] private GameObject _stageSelectGameObject;
    [SerializeField] private GameObject _stageClearUIGameObject;
    private UI_StageClear _stageClearUI;

    
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

        Init();
    }

    private void Start()
    {        
        _levelUpManager = GetComponent<LevelUpManager>();

        _stageClearUI = _stageClearUIGameObject.GetComponent<UI_StageClear>();
    }

    private void Init()
    {
        _stageSelectGameObject.SetActive(true);
    }

    public void PlayerLevelUp()
    {
        Debug.Log("[StageManager] Player Level Up!");
        
        _levelUpManager.OnLevelUp();
    }

    public void StageClear()
    {
        Debug.Log("[StageManager] Stage Clear");
        //Time.timeScale = 0;

        _stageClearUI.CallStageClearUI();

        ResetStage();
    }

    private void ResetStage()
    {

    }
}
