using UnityEngine;

[CreateAssetMenu(fileName = "Fireball_", menuName = "Skill/SkillData/Fireball", order = 1)]
public class LevelupData_Fireball : SkillData_Active
{
    [Header("Upgradable")]
    public float projectileSize;
    public float explosionRadius;
}
