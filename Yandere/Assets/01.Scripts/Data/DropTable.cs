using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject itemPrefab;

    [Range(0, 100)] public int probability;

    public float pickupDelay = 5f;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Stage/DropTable", order = 2)]
public class DropTable : ScriptableObject
{
    public List<DropEntry> entries;
}