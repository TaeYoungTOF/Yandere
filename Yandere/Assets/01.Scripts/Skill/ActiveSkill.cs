using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActiveSkill", menuName = "Skills/ActiveSkill")]
public class ActiveSkill : BaseSkill
{
    public List<SkillStatData> levelStats;
    public GameObject projectilePrefab;
    public LayerMask enemyLayer;

    public override void Activate(Transform caster)
    {
        if (projectilePrefab == null || caster == null || levelStats == null || levelStats.Count == 0)
            return;

        SkillStatData stat = levelStats[Mathf.Clamp(level - 1, 0, levelStats.Count - 1)];

        // 자동 분기: 스킬 타입에 따라 처리
        if (projectilePrefab.GetComponent<WoundofParting>() != null)
        {
            GameObject obj = Instantiate(projectilePrefab, caster.position, Quaternion.identity);
            var skill = obj.GetComponent<WoundofParting>();
            if (skill != null)
                skill.Initialize(stat, enemyLayer);
        }
        else if (projectilePrefab.GetComponent<Tornado>() != null)
        {
            GameObject obj = Instantiate(projectilePrefab, caster.position, Quaternion.identity);
            var skill = obj.GetComponent<Tornado>();
            if (skill != null)
                skill.Initialize(stat, enemyLayer);
        }
        else
        {
            if (caster.TryGetComponent(out MonoBehaviour mb))
            {
                mb.StartCoroutine(FireSequentially(stat, caster));
            }
            else
            {
                Debug.LogWarning("caster에 MonoBehaviour가 없어 Coroutine 실행 불가");
            }
        }
    }

    private IEnumerator FireSequentially(SkillStatData stat, Transform caster)
    {
        int count = stat.projectileCount;
        float delay = 0.1f; // 발사 간격

        for (int i = 0; i < count; i++)
        {
            Vector2 direction = caster.up;
            Vector3 spawnPos = caster.position + (Vector3)(direction * 1f);

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            var fireball = proj.GetComponent<Fireball>();
            if (fireball != null)
            {
                fireball.Initialize(stat, enemyLayer, direction);
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
