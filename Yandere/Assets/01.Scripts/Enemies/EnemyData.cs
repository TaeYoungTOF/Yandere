using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCneter", menuName = "ScrptableObject/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    // 적 프리펩
    [Header("적 프리펩")]
    [Tooltip("제작한 적 프리펩을 넣는 공간입니다.")]
    [SerializeField]
    public GameObject Prefab;
    
    [Header("적 이름")]
    [Tooltip("적의 이름을 적습니다.")]
    [SerializeField]
    public string Name;
    
    [Header("적 체력")]
    [Tooltip("적의 체력을 설정합니다.")]
    public float HP;
    
    [Header("적 데미지")]
    [Tooltip("적의 데미지를 설절합니다.")]
    public float Damage;
}
