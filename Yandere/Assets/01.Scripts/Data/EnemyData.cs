using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    // 적 프리펩
    [Header("몬스터 프리펩")]
    [Tooltip("제작한 몬스터 프리펩을 넣는 공간입니다.")]
    public GameObject monsterPrefab;
    
    //몬스터 등급 타입
    [Header("몬스터 등급 타입")]
    [Tooltip("몬스터의 등급 타입을 적는 공간입니다.")]
    public EnemyType enemyType;
    
    //몬스터 공격 타입
    [Header("몬스터 공격 타입")]
    [Tooltip("몬스터의 공격 타입을 적는 공간입니다.")]
    public EnemyAttackType enemyAttackType;
    
    //몬스터 이름
    [Header("몬스터 이름")]
    [Tooltip("몬스터의 이름을 적습니다.")]
    public string monsterName;
    
    //몬스터 체력
    [Header("몬스터 체력")]
    [Tooltip("몬스터의 체력을 설정합니다.")]
    public float monsterMaxHp;
    
    //몬스터 공격력
    [Header("몬스터 공격력")] 
    [Tooltip("몬스터의 공격력을 설정합니다.")] 
    public int monsterAttack;

    [Header("몬스터 공격속도")]
    [Tooltip("몬스터의 공격속도를 설정합니다.")]
    public float attackCooldown;
    
    //몬스터 이동속도
    [Header("몬스터 이동속도")] 
    [Tooltip("몬스터의 이동속도를 설정합니다.")] 
    public int monsterMoveSpeed;
    
    //몬스터가 죽을 때 떨어트리는 경험치
    [Header("몬스터가 드랍하는 경험치")]
    [Tooltip("몬스터가 쓰러질 때 얻는 경험치를 설정합니다")]
    public float monsterExp;
}
