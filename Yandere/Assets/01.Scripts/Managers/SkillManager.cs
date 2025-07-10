using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public const int maxLevel = 5;

    public List<BaseSkill> availableSkills;

    public List<ActiveSkill> equipedActiveSkills;
    public List<PassiveSkill> equipedPassiveskills;

    private bool _isFirstDraw;

    [Header("Passive Skill Stats")]
    [SerializeField] private int _projectileCount = 0;
    public int ProjectileCount => _projectileCount;
    [SerializeField] private float _skillDamage = 0;
    public float SkillDamage => _skillDamage;
    [SerializeField] private float _skillDuration = 0;
    public float skillDuration => _skillDuration;
    [SerializeField] private float _coolDown = 0;
    public float CoolDown => _coolDown;
    [SerializeField] private float _skillRange = 0;
    public float SkillRange => _skillRange;
    [SerializeField] private float _crit = 0;
    public float Crit => _crit;

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
                availableSkills.Add(skill);
            }
        }

        _isFirstDraw = true;
    }

    private void Update()
    {
        foreach (var skill in equipedActiveSkills)
        {
            skill.UpdateCooldown();
            skill.TryActivate();
        }
    }

    public void UpdatePassiveStat()
    {
        foreach (var passiveSkill in equipedPassiveskills)
        {
            SkillId id = passiveSkill.skillId;
            float value = passiveSkill.PassiveData.value;

            switch ((int)id)
            {
                case 101:
                    _projectileCount = (int)value;
                    break;
                case 102:
                    _skillDamage = value;
                    break;
                case 103:
                    _skillDuration = value;
                    break;
                case 104:
                    _coolDown = value;
                    break;
                case 105:
                    _skillRange = value;
                    break;
                case 106:
                    _crit = value;
                    break;
                default:
                    Debug.Log("[SkillManager] Unknown Passive Type");
                    break;
            }
        }
    }

    public List<BaseSkill> GetSkillDatas(int count)
    {
        List<BaseSkill> shuffledList = new List<BaseSkill>(availableSkills);

        if (_isFirstDraw)
        {
            shuffledList.RemoveAll(skill => skill is PassiveSkill);
            _isFirstDraw = false;
        }

        if (count > shuffledList.Count)
            count = shuffledList.Count;

        for (int i = shuffledList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList.GetRange(0, count);
    }
}
