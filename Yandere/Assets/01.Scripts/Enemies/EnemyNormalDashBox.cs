using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalDashBox : MonoBehaviour
{
    private EnemySkill_Dash _dashSkill;

    private void Awake()
    {
        _dashSkill = GetComponentInParent<EnemySkill_Dash>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dashSkill.SetCanUseDash(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dashSkill.SetCanUseDash(false);
        }
    }
}
