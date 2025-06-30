using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject itemPrefab;

    [Range(0, 100)] public int probability;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "DropTable", order = 0)]
public class DropTable : ScriptableObject
{
    public List<DropEntry> entries;
}