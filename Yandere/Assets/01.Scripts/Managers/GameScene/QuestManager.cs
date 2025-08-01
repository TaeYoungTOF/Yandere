using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest Fields")]
    public bool isStageCleared;
    [SerializeField] private float surviveTimer;
    public float SurviveTimer => surviveTimer;
    [SerializeField] private int killCount;
    public int  KillCount => killCount;
    public int eliteKillCount;
    public int healItemUseCount;
    public int expItemUseCount;
    public float lastDamageTime;
    public float lastMoveTime;
    
    [Header("Quest List")]
    public List<Quest> currentQuests;
    public List<Quest> allQuests;

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
        
        lastDamageTime = Time.time;
        lastMoveTime = Time.time;
    }

    public void UpdateValue()
    {
        surviveTimer = StageManager.Instance.ElapsedTime;
        killCount = StageManager.Instance.KillCount;

        foreach (var quest in currentQuests)
        {
            quest.CheckClear();
        }
    }

    public int ReturnClearedQuest()
    {
        int clearedQuests = 0;
        foreach (var quest in currentQuests)
        {
            clearedQuests += quest.isCleared ?  1 : 0;
        }
        
        return  clearedQuests;
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
    //ClearStageWithoutRevive, => 부활기능 미구현
    ClearWithHpOverNumPercent,
    ClearWithHpItemUseLessThanNum,
    
    SurviveNumSecWithoutDamage,
    SurviveNumSec,
    
    KillNumEnemy,
    KillNumEliteEnemy,
    
    GetNumActiveSkill,
    GetNumPassiveSkill,
    GetNumUpgradeSkill,
    
    GetNumExpItem,
    SurviveNumSecWithoutMove,
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
                currentValue = QuestManager.Instance.isStageCleared ?  1 : 0;
                if (QuestManager.Instance.isStageCleared) isCleared = true;
                break;

            /*case QuestContent.ClearStageWithoutRevive:
                break;*/

            case QuestContent.ClearWithHpOverNumPercent:
                currentValue = (int)(StageManager.Instance.Player.stat.CurrentHp / StageManager.Instance.Player.stat.FinalHp);
                if (QuestManager.Instance.isStageCleared && currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.ClearWithHpItemUseLessThanNum:
                currentValue = QuestManager.Instance.healItemUseCount;
                if (QuestManager.Instance.isStageCleared && currentValue < maxValue) isCleared = true;
                break;

            case QuestContent.SurviveNumSecWithoutDamage:
                currentValue = (int)(Time.time - QuestManager.Instance.lastDamageTime);
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.SurviveNumSec:
                currentValue = (int)QuestManager.Instance.SurviveTimer;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.KillNumEnemy:
                currentValue = QuestManager.Instance.KillCount;
                if  (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.KillNumEliteEnemy:
                currentValue = QuestManager.Instance.eliteKillCount;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.GetNumActiveSkill:
                currentValue = SkillManager.Instance.equipedActiveSkills.Count;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.GetNumPassiveSkill:
                currentValue = SkillManager.Instance.equipedPassiveSkills.Count;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.GetNumUpgradeSkill:
                currentValue = SkillManager.Instance.equipedUpgradeSkills.Count;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.GetNumExpItem:
                currentValue = QuestManager.Instance.expItemUseCount;
                if (currentValue >= maxValue) isCleared = true;
                break;

            case QuestContent.SurviveNumSecWithoutMove:
                currentValue = (int)(Time.time - QuestManager.Instance.lastMoveTime);
                if (currentValue >= maxValue) isCleared = true;
                break;

            default:
                isCleared = false;
                break;
        }
    }
}