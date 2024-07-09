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
        {//��H���I�쪫�~
            Debug.Log("IN");
            AddNewItem();
            Destroy(gameObject);
        }
        if (other.CompareTag("Bag"))
        {//���~�I��I�]
            Debug.Log("IN");
            AddNewItem();
            Destroy(gameObject);
        }
    }
    public void AddNewItem()
    {
        if (!PlayerInventery.ItemList.Contains(ThisItem))
        {//�p�G�I�]�C���]�t�o�Ӫ��~
            Debug.Log("Add");
            PlayerInventery.ItemList.Add(ThisItem);
            InventoryManager.CreateNewItem(ThisItem);
        }
        else
        {//�p�G�I�]���w�g���o���~
            Debug.Log("+1");
            ThisItem.ItemHeid += 1;
        }
    }
}
