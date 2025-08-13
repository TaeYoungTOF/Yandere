using UnityEngine;

public class ItemObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float objectMaxHealth = 10;
    [SerializeField] private DropContext _dropContext;
    private float currentHealth;
    
    private bool isBroken;
    private Animator _animator;
    

    public void Init()
    {
        isBroken = false;
        currentHealth = objectMaxHealth;
        _animator = GetComponent<Animator>();
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
        
        _animator.SetTrigger("Break");
        _dropContext.position = transform.position;
        StageManager.Instance.ItemDropManager.HandleDrop(_dropContext);
        SoundManager.Instance.Play("InGame_ObjectField_DestroySFX");
        StageManager.Instance.mapObjectCount--;
    }
    
    public void ObjectBreakAnimationEnd()
    {
        ObjectPoolManager.Instance.ReturnToPool(PoolType.FieldObject, gameObject);
    }

}
