using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MagnetEffect : MonoBehaviour
{
    public float magnetSpeed = 1f;

    public void AttractAllItems()
    {
        GameObject[] dropItems = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in dropItems)
        {
            Item_ItemDropEffect effect = item.GetComponent<Item_ItemDropEffect>();
            if (effect != null)
            {
                effect.MoveToPlayerInstantly(magnetSpeed);
            }
        }
    }
}
