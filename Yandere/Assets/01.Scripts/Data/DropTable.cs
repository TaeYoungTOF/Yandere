using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public string itemId;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Drop/DropTable", order = 1)]
public class DropTable : ScriptableObject
{
    [System.Serializable]
    public class DropEntry
    {
        public ItemData itemData;
        [Range(0f, 1f)]
        public float probability;
    }

    public List<DropEntry> entries = new();
}