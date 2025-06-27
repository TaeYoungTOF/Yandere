using UnityEngine;

public class TestLevelUp : MonoBehaviour
{
    public Player player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("테스트용 경험치 +50 획득");
            player.GainExp(50);
        }
    }
}
