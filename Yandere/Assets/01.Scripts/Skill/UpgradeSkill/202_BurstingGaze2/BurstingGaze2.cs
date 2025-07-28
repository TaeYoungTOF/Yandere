using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingGaze2Wrapper : UpgradeSkillWrapper
{
    public float projectileSize;
    public float projectileDistance;

    [Header("Const Data")]
    public float angle = 45f;
    public float projectileSpeed = 25f;
    public float shootDelay = 0.1f;

    [Header("Second Data")]
    public float secondSkillDmg;
    public float secondPjtSize;
    public float secondPjtDuration;
    
    public float secondPjtSpeed = 10f;
    public float secondShootDelay = 1f;
    public float dmgInterval = 0.5f;

}

public class BurstingGaze2 : UpgradeSkill<BurstingGaze2Wrapper>
{
    [SerializeField] private float _projectileSize = 0.5f;
    [SerializeField] private float _projectileDistance = 25f;

    [SerializeField] private float _secondPjtDmg = 120f;
    [SerializeField] private float _secondPjtSize = 2f;
    [SerializeField] private float _secondPjtDuration = 3f;
    
    [Header("References")]
    [SerializeField] private LayerMask _enemyLayer;
    
    public override void UpdateActiveData()
    {
        base.UpdateActiveData();

        data.projectileSize = _projectileSize * player.stat.FinalSkillRange;
        data.projectileDistance = _projectileDistance * player.stat.FinalSkillRange;

        data.secondSkillDmg = CalculateDamage(_secondPjtDmg);
        data.secondPjtSize = _secondPjtSize * player.stat.FinalSkillRange;
        data.secondPjtDuration = _secondPjtDuration * player.stat.FinalSkillDuration;
    }

    protected override void Activate()
    {
        StartCoroutine(ShootProjectiles());
    }

    private IEnumerator ShootProjectiles()
    {
        for (int i = 0; i < data.projectileCount; i++)
        {
            float randomAngle = GetRandomAngle(-data.angle / 2f, data.angle / 2f, 0);
            Vector2 finalDir = Quaternion.Euler(0f, 0f, randomAngle) * player.GetLastMoveDirection();

            GameObject projGo = ObjectPoolManager.Instance.GetFromPool(PoolType.BurstingGaze2Proj, player.transform.position, Quaternion.identity);
            var proj = projGo.GetComponent<BurstingGaze2Proj>();
            proj.Initialize(finalDir, data, _enemyLayer);

            proj.transform.localScale = Vector3.one * data.projectileSize;

            yield return new WaitForSeconds(data.shootDelay);
        }

        yield return new WaitForSeconds(data.secondShootDelay);
            
        GameObject secondProjGo = ObjectPoolManager.Instance.GetFromPool(PoolType.BurstingGaze2Proj2, transform.position, Quaternion.identity);
        var secondProj = secondProjGo.GetComponent<BurstingGaze2Proj2>();
        secondProj.Initialize(player.GetLastMoveDirection(), data, _enemyLayer);
        secondProj.transform.localScale = Vector3.one * data.secondPjtSize;
    }

    private float GetRandomAngle(float min, float max, float mode)
    {
        float u = Random.value;
        float d = max - min;
        float f = (mode - min) / d;

        return u < f
            ? min + Mathf.Sqrt(u * d * (mode - min))
            : max - Mathf.Sqrt((1 - u) * d * (max - mode));
    }
}
