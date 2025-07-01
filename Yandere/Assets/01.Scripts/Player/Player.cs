using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private StageManager _stageManager;
    public PlayerStat stat = new();
    public PlayerController PlayerController { get; private set; }

    public void Init(StageManager stageManager)
    {
        _stageManager = stageManager;
        stat.ResetStat();

        PlayerController = GetComponent<PlayerController>();
    }

    // 경험치 획득 처리
    public void GainExp(float amount)
    {
        stat.currentExp += amount * stat.expGain;
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateExpImage();

        while (stat.currentExp >= stat.requiredExp)
        {
            stat.currentExp -= stat.requiredExp;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Debug.Log($"[Player] 레벨 업! 현재 레벨: {stat.level}");

        stat.level++;        
        stat.requiredExp *= 1.1f;  // 경험치통 공식 추후 수정
        
        _stageManager.LevelUpEvent();

        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateLevel();
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
        UIManager.Instance.GetPanel<UI_GameHUD>().UpdateHealthImage();

        Debug.Log($"[Player] 체력: {stat.currentHealth}/{stat.maxHealth}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Layer가 item이면 
    }
}
