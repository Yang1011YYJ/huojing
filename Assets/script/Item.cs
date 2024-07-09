using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New File"/*命名*/,
    menuName = "Inventory/New Item"/*右鍵菜單叫什麼名字*/)]//創建新物件的菜單(在創建新物件的菜單設定自定義選項)
public class Item : ScriptableObject
{
    public string ItemName;//物品名稱
    public Sprite ItemImage;//物品圖案
    public int ItemHeid;//持有物品數量
    [TextArea]//宣告這是一個文字輸入區域(沒寫就是一行)
    public string ItemIntro;//物件描述
}