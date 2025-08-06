using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashSkillDamageBox : MonoBehaviour
{
    private EnemySkill_Dash _dash;

    private void Awake()
    {
        _dash = GetComponentInParent<EnemySkill_Dash>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       // if (!_dash.IsDashing) return;
        if (!other.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            //playerRb.AddForce(dir * _dash.DashForce, ForceMode2D.Impulse);
        }

       // float damage = PlayerManager.Instance.MaxHP * 0.1f;
        StageManager.Instance.Player.TakeDamage(10);
        Debug.Log("[Dash] 대쉬 피해 처리 완료");
    }
}
