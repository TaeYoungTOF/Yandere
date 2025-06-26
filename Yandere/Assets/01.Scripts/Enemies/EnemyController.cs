using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IDamagable
{
    private Transform target;

    [SerializeField] private float followRange = 15f;

    public void InIt(Transform target)
    {
        this.target = target;
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

     /* protected override void HandleAction()
    {
        base.HandleAction();

        if (target == null)
        {
            if (!MoveDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }
    }
    
    float distance = DistanceToTarget();
    private Vector2 direction = DirectionToTarget();  */

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"[EnemyController] {name}이 {damage}의 피해를 입었습니다.");
    }
}
