using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningJeoulousy2Wrapper : UpgradeSkillWrapper
{
    
}

public class BurningJeoulousy2 : UpgradeSkill<BurningJeoulousy2Wrapper>
{
    [SerializeField] private float _explodeRadius = 4f;
    

    protected override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
