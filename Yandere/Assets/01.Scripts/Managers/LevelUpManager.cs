// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LevelUpManager : MonoBehaviour
// {
//     public SkillSelectUI skillSelectUI;
//     public List<BaseSkill> allSkills;

//     public void OnLevelUp()
//     {
//         Debug.Log("레벨업 매니저 - 스킬 선택 UI 호출됨");
//         var options = GetRandomSkillOptions(3);
//         skillSelectUI.Show(options);
//     }

//     private List<BaseSkill> GetRandomSkillOptions(int count)
//     {
//         List<BaseSkill> available = new List<BaseSkill>();
//         foreach (var skill in allSkills)
//         {
//             if (!FindObjectOfType<SkillManager>().equippedSkills.Contains(skill) || skill.level < 5)
//                 available.Add(skill);
//         }

//         List<BaseSkill> result = new List<BaseSkill>();
//         for (int i = 0; i < count; i++)
//         {
//             if (available.Count == 0) break;
//             int rand = Random.Range(0, available.Count);
//             result.Add(available[rand]);
//             available.RemoveAt(rand);
//         }
//         return result;
//     }
// }
