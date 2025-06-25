using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public int BaseAttackPower { get; set; }                //플레이어 기본 공격력
    public int BaseDefense { get; set; }                    //플레이어 기본 아머
    public int CurrentHealth { get; set; }                  //플레이어 현재 체력
    public int BaseMaxHealth { get; set; }                  //플레이어 기본 최대 체력
    public float HealthRegen { get; set; }                  //플레이어 체력 재생
    public float BaseCritChance { get; set; }               //플레이어 기본 치명타 확률
    public float BaseCritDamage { get; set; }               //플레이어 기본 치명타 배율
    public float BasePlayerMoveSpeed { get; set; }          //플레이어 이동속도
    public int CurrentEXP { get; set; }                     //플레이어 현재 경험치
    public int MaxEXP { get; set; }                         //플레이어 최대 경험치
    public int PlayerLevel { get; set; }                    //플레이어 레벨
    public int BasePickupRadius { get; set; }               //플레이어 기본 아이템/경험치 획득 반경


    public PlayerStat()
    {
        PlayerLevel = 1;
        BaseAttackPower = 10;
        BaseDefense = 5;
        CurrentHealth = 100;
        BaseMaxHealth = 100;
        BaseCritChance = 0.05f;
        BaseCritDamage = 2.0f;
        BasePlayerMoveSpeed = 5;
        CurrentEXP = 0;
        MaxEXP = 100;
        BasePickupRadius = 3;
    }
    
}

public class PlayerResource
{
    public int CurrentGold { get; set; }                //플레이어 현재 보유골드
    public int PremiumCurrency { get; set; }            //플레이어 유료 재화
    public int SkillPoint { get; set; }                 //플레이어 스킬 포인트
    public int ObsessionPoint { get; set; }             //플레이어 집착 포인트
}



