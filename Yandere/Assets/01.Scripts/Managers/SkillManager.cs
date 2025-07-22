using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public readonly int MaxLevel = 5;
    
    public List<BaseSkill> availableSkills;

    public List<ActiveSkill> equipedActiveSkills;
    public List<PassiveSkill> equipedPassiveSkills;
    public List<UpgradeSkill> equipedUpgradeSkills;
    
    [SerializeField] private List<BaseSkill> _firstDrawPool;
    private bool _isFirstDraw;

    public List<UpgradeSkill> availableUpgradeSkills;
    private List<UpgradeSkill> _upgradableSkills;

    public bool isLevelUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Init();
    }

    private void Init()
    {
        availableSkills = new List<BaseSkill>();

        BaseSkill[] skills = GetComponentsInChildren<BaseSkill>();

        foreach (BaseSkill skill in skills)
        {
            if (skill != null)
            {
                skill.Init();
                availableSkills.Add(skill);
            }
        }
        
        availableSkills.RemoveAll(skill => skill is UpgradeSkill);

        _isFirstDraw = true;
    }

    private void Update()
    {
        foreach (ActiveSkill skill in equipedActiveSkills)
        {
            skill.UpdateCooldown();
            skill.TryActivate();
        }

        foreach (UpgradeSkill skill in equipedUpgradeSkills)
        {
            skill.UpdateCooldown();
            skill.TryActivate();
        }
    }

    public List<BaseSkill> GetSkillDatas(int count)
    {
        List<BaseSkill> resultList = new List<BaseSkill>();;
        
        if (_isFirstDraw)
        {
            //shuffledList.RemoveAll(skill => skill is PassiveSkill);
            
            resultList = new List<BaseSkill>(_firstDrawPool);

            for (int i = resultList.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (resultList[i], resultList[j]) = (resultList[j], resultList[i]);
            }

            // 개수 조정
            if (count < resultList.Count)
                resultList = resultList.GetRange(0, count);
            
            _isFirstDraw = false;
            return resultList;
        }

        if (CheckUpgradable())
        {
            resultList.AddRange(_upgradableSkills);
            count -= _upgradableSkills.Count;
        }
        
        if (count > 0)
        {
            List<BaseSkill> shuffledList = new List<BaseSkill>(availableSkills);

            // 셔플
            for (int i = shuffledList.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
            }

            // 추가
            for (int i = 0; i < shuffledList.Count && resultList.Count < count; i++)
            {
                resultList.Add(shuffledList[i]);
            }
        }

        return resultList;
    }

    private bool CheckUpgradable()
    {
        _upgradableSkills = new List<UpgradeSkill>();
        
        bool isUpgradable = false;
        
        Debug.Log(_upgradableSkills.Count);

        foreach (UpgradeSkill skill in availableUpgradeSkills)
        {
            if (skill.IsUpgradable())
            {
                _upgradableSkills.Add(skill);
                
                isUpgradable = true;
            }
        }
        
        return isUpgradable;
    }
}
