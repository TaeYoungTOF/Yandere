using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public int BaseAttack { get; set; }                 //플레이어 기본 공격력
    public int BaseArmor { get; set; }                  //플레이어 기본 아머
    public int CurrentHealth { get; set; }              //플레이어 현재 체력
    public int MaxHealth { get; set; }                  //플레이어 최대 체력
    public float HealthRegen { get; set; }              //플레이어 체력 재생
    public int CurrentEXP { get; set; }                 //플레이어 현재 경험치
    public int MaxEXP { get; set; }                     //플레이어 최대 경험치
    public float PlayerMoveSpeed { get; set; }          //플레이어 이동속도
    

}

public class PlayerResource
{
    public int CurrentGold { get; set; }                //플레이어 현재 보유골드
    public int PremiumCurrency { get; set; }            //플레이어 유료 재화
    public int SkillPoint { get; set; }                 //플레이어 스킬 포인트
    public int ObsessionPoint { get; set; }             //플레이어 집착 포인트
}
