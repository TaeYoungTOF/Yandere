using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<Quest> allQuests;
    public List<Quest> currentQuests;

    [Header("Quest Fields")]
    [SerializeField] private float surviveTimer;
    [SerializeField] private int killCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        GenerateQuest();
    }

    private void GenerateQuest()
    {
        if (allQuests.Count < 3)
        {
            Debug.LogWarning("퀘스트가 3개 미만입니다. 퀘스트를 더 추가해주세요.");
            return;
        }
        
        currentQuests.Clear();

        List<int> usedIndices = new List<int>();
        while (currentQuests.Count < 3)
        {
            int randomIndex = Random.Range(0, allQuests.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                currentQuests.Add(allQuests[randomIndex]);
            }
        }

        Debug.Log("퀘스트 3개가 생성되었습니다.");
    }

    public void UpdateValue()
    {
        surviveTimer = StageManager.Instance.ElapsedTime;
        killCount = StageManager.Instance.KillCount;
    }
}

public enum QuestCategory
{
    Clear,
    Survive,
    Battle,
    Growth,
    Etc,
}

public enum QuestContent
{
    ClearStage,
    ClearStageWithoutRevive,
    ClearWithHpOver80Percent,
    ClearWithHpItemUseLessThan2,
    
    Survive120SecWithoutDamage,
    Survive300Sec,
    
    Kill1000Enemy,
    Kill2000Enemy,
    Kill20EliteEnemy,
    
    Get6ActiveSkill,
    Get6PassiveSkill,
    Get1UpgradeSkill,
    Get2UpgradeSkill,
    
    Get5000ExpItem,
    Survive60SecWithoutMove,
}

[System.Serializable]
public class Quest
{
    public QuestCategory category;
    public QuestContent content;
    public bool isCleared;
    [Multiline(3)] public string description;
    public int currentValue;
    public int maxValue;

    public void CheckClear()
    {
        switch (content)
        {
            case QuestContent.ClearStage:
                break;

            case QuestContent.ClearStageWithoutRevive:
                break;

            case QuestContent.ClearWithHpOver80Percent:
                break;

            case QuestContent.ClearWithHpItemUseLessThan2:
                break;

            case QuestContent.Survive120SecWithoutDamage:
                break;

            case QuestContent.Survive300Sec:
                break;

            case QuestContent.Kill1000Enemy:
                break;

            case QuestContent.Kill2000Enemy:
                break;

            case QuestContent.Kill20EliteEnemy:
                break;

            case QuestContent.Get6ActiveSkill:
                break;

            case QuestContent.Get6PassiveSkill:
                break;

            case QuestContent.Get1UpgradeSkill:
                break;

            case QuestContent.Get2UpgradeSkill:
                break;

            case QuestContent.Get5000ExpItem:
                break;

            case QuestContent.Survive60SecWithoutMove:
                break;

            default:
                isCleared = false;
                break;
        }
    }
}