using UnityEngine;

[CreateAssetMenu(fileName = "LevelupData_Fireball", menuName = "Skill/SkillData/Fireball", order = 1)]
public class LevelupData_Fireball : SkillData_Active
{
    [Header("Upgradable")]
    public float projectileSize;
    public float explosionRadius;

    [Header("Unupgradable")]
    public float projectileSpeed = 15f;
    public float projectileDistance = 30f;    
}
