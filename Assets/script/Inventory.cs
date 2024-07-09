using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/ New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item/*寫有背包系統裡的物件屬性的那個腳本名稱*/> ItemList = new List<Item>();//各背包系統的屬性
}
