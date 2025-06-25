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
    
    //적 타입
    [Header("적 타입")]
    [Tooltip("적의 타입을 적는 공간입니다.")]
    [SerializeField]
    public string Type;
    
    // 적 이름
    [Header("적 이름")]
    [Tooltip("적의 이름을 적습니다.")]
    [SerializeField]
    public string Name;
    
    // 적 체력
    [Header("적 체력")]
    [Tooltip("적의 체력을 설정합니다.")]
    [SerializeField]
    public float HP;
    
    // 적 공격력
    [Header("적 공격력")] 
    [Tooltip("적의 공격력을 설정합니다.")] 
    [SerializeField]
    public int Atk;
    
    // 적 이동속도
    [Header("적 이동속도")] 
    [Tooltip("적의 이동속도를 설정합니다.")] 
    [SerializeField]
    public int Movespeed;
    
    // 적이 맞는 데미지
    [Header("적 데미지")]
    [Tooltip("적이 받는 데미지를 설절합니다.")]
    [SerializeField]
    public float Damage;
    
    // 적이 처치할 때 얻는 경험치
    [Header("적 경험치")]
    [Tooltip("적이 쓰러질 때 얻는 경험치를 설정합니다")]
    [SerializeField]
    public float Exp;
}
