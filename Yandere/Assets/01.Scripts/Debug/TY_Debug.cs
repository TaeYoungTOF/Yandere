using NaughtyAttributes;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TY_Debug : MonoBehaviour
{
    [Foldout("📦 프리팹 디버그용")]
    public GameObject normalEnemyPrefab;
    [Foldout("📦 프리팹 디버그용")]
    public GameObject eliteEnemyPrefab;
    [Foldout("📦 프리팹 디버그용")]
    public GameObject bossEnemyPrefab;
    [Foldout("📦 프리팹 디버그용")]
    public GameObject itemObjectPrefab;
    
    [Button]
    public void DebugLevelUp()
    {
        Debug.Log("[Debug] Level Up");

        StageManager.Instance.Player.GainExp(10);
    }

    [Button]
    public void DebugStageClear()
    {
        Debug.Log("[Debug] Stag Clear");

        StageManager.Instance.StageClear();
    }

    [Button]
    public void DebugHealHealth()
    {
        Debug.Log("[Debug] Heal 100 Health");
        
        StageManager.Instance.Player.Heal(100);
    }

    [Button]
    public void AddTimeScale()
    {
        StageManager.Instance.timeScale += 1;
    }

    [Button]
    public void ReduceTimeScale()
    {
        StageManager.Instance.timeScale -= 1;
    }

    [Button]
    public void DebugSpawnNormalEnemy()
    {
        SpawnEnemy(normalEnemyPrefab);
    }

    [Button]
    public void DebugSpawnEliteEnemy()
    {
        SpawnEnemy(eliteEnemyPrefab);
    }
    [Button]
    public void DebugSpawnBossEnemy()
    {
        SpawnEnemy(bossEnemyPrefab);
    }

    [Button]
    public void DebugSpawnObject()
    {
        SpawnItemObject(itemObjectPrefab);
    }
    
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is not assigned.");
            return;
        }

        Vector3 spawnPos = StageManager.Instance.Player.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    
    private void SpawnItemObject(GameObject itemObject)
    {
        if (itemObject == null)
        {
            Debug.LogWarning("Enemy prefab is not assigned.");
            return;
        }

        Vector3 spawnPos = StageManager.Instance.Player.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        Instantiate(itemObject, spawnPos, Quaternion.identity);
    }
    
}
