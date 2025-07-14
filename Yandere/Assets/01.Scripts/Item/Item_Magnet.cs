using UnityEngine;

public class Item_Magnet : Item
{
    public override void Use(Player player)
    {
        Debug.Log("[Magnet] Magnet");

        // 플레이어에서 MagnetEffect를 가져와 발동
        Item_MagnetEffect magnet = player.GetComponent<Item_MagnetEffect>();
        if (magnet != null)
        {
            magnet.AttractAllItems();
        }
        else
        {
            Debug.Log("MagnetEffect 컴포넌트를 Player에서 찾을 수 없습니다!");
        }

        Destroy(gameObject); // 아이템 자체는 파괴
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            foreach (var exp in FindObjectsOfType<Experience>())
            {
                exp.StartMagnetEffect();
            }
            Destroy(gameObject);
        }
        */
    }
}
