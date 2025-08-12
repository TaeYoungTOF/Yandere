using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    protected LayerMask enemyLayer;
    
    public abstract void Initialize();
}
