using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float objectMaxHealth = 10;
    [SerializeField] private DropContext _dropContext;
    private float currentHealth;
    
    private bool isBroken = false;
    private Animator _animator;
    

    void Awake()
    {
        currentHealth = objectMaxHealth;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("게임 오브젝트 테스트 단축키 : E");
            TakeDamage(10f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSkill"))
        {
            TakeDamage(10f);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if(isBroken) return;
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            ObjectBreak();
        }
    }
    
    void ObjectBreak()
    {
        isBroken = true;
        
        Debug.Log("오브젝트가 파괴 됨");
        _animator.SetTrigger("Break");
        _dropContext.position = transform.position;
        StageManager.Instance.ItemDropManager.HandleDrop(_dropContext);
        
    }
    
    public void ObjectBreakAnimationEnd()
    {
        Destroy(gameObject);
    }

}
