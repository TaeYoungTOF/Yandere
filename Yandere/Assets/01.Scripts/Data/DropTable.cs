using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "DropTable", order = 1)]
public class DropTable : ScriptableObject
{
    [System.Serializable]
    public class DropEntry
    {
        public GameObject itemPrefab;

        [Range(0f, 1f)] public float probability;
    }

    public List<DropEntry> entries = new();
}