using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    [Header("­I¥]")]
    public Inventory MyBag;
    public GameObject SlotGrid;
    public Slot SlotPrefeb;
    public TextMeshProUGUI ItemInformation;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public static void CreateNewItem(Item item)
    {
        Slot NewItem = Instantiate(instance.SlotPrefeb, instance.SlotGrid.transform.position, Quaternion.identity);
        NewItem.gameObject.transform.SetParent(instance.SlotGrid.transform);
        NewItem.SlotItem = item;
        NewItem.SlotImage.sprite = item.ItemImage;
        NewItem.SlotNum.text = item.ItemHeid.ToString();
    }
}
