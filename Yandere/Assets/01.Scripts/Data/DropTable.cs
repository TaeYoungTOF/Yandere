using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    [Header("드롭 아이템 데이터(ItemData SO)")]
    public ItemData itemData;
    
    [Header("드롭 확률 (0~100%)")]
    [Range(0, 100)]
    public int probability;
    
    [Header("드롭 후 딜레이 (즉시 수집 방지용)")]
    public float pickupDelay = 5f;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Stage/DropTable", order = 2)]
public class DropTable : ScriptableObject
{
    [Header("등롭 항목 목록")]
    public List<DropEntry> entries = new List<DropEntry>();
}