using UnityEngine;

public class Player : MonoBehaviour
{
    private StageManager _stageManager;
    public PlayerStat stat = new();

    public void Init(StageManager stageManager)
    {
        _stageManager = stageManager;
        stat.ResetStat();
    }

    // 경험치 획득 처리
    public void GainExp(float amount)
    {
        stat.currentExp += amount * stat.expGain;
        while (stat.currentExp >= stat.requiredExp)
        {
            stat.currentExp -= stat.requiredExp;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        stat.level++;

        // 경험치통 공식 추후 수정
        stat.requiredExp *= 1.1f;

        Debug.Log($"[Player] 레벨 업! 현재 레벨: {stat.level}");
        _stageManager.LevelUpEvent();
    }

    public void Heal(float amount)
    {
        stat.currentHealth = Mathf.Min(stat.currentHealth + amount, stat.maxHealth);
    }

    public void TakeDamage(float amount)
    {
        //방어력 계산 공식 추후 수정
        float actualDamage = Mathf.Max(amount - stat.defense, 1f);


        stat.currentHealth = Mathf.Max(stat.currentHealth - actualDamage, 0f);

        Debug.Log($"[Player] 체력: {stat.currentHealth}/{stat.maxHealth}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Layer가 item이면 
    }
}
