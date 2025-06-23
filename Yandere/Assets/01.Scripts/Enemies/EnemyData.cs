using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCneter", menuName = "ScrptableObject/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    
    public GameObject Prefab;
    public string Name;
    public float HP;
    public float Damage;
}
