using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item ThisItem;
    public Inventory PlayerInventery;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {//當人物碰到物品
            Debug.Log("IN");
            AddNewItem();
            Destroy(gameObject);
        }
        if (other.CompareTag("Bag"))
        {//當物品碰到背包
            Debug.Log("IN");
            AddNewItem();
            Destroy(gameObject);
        }
    }
    public void AddNewItem()
    {
        if (!PlayerInventery.ItemList.Contains(ThisItem))
        {//如果背包列表不包含這個物品
            Debug.Log("Add");
            PlayerInventery.ItemList.Add(ThisItem);
            InventoryManager.CreateNewItem(ThisItem);
        }
        else
        {//如果背包內已經有這物品
            Debug.Log("+1");
            ThisItem.ItemHeid += 1;
        }
    }
}
