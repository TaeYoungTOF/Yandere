using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float moveSpeed;

    public float FinalMaxHealth => maxHealth;

    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 100;

    public void GainExp(int amount)
    {
        exp += amount;
        if (exp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        exp -= expToNextLevel;
        expToNextLevel = CalculateNextExp(level);
        FindObjectOfType<LevelUpManager>().OnLevelUp();
    }

    int CalculateNextExp(int level)
    {
        return 100 + (level - 1) * 20;
    }

    public void Heal(float amount)
    {
        Debug.Log($"체력 {amount} 회복!");
    }
}
