using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/ New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item/*�g���I�]�t�θ̪������ݩʪ����Ӹ}���W��*/> ItemList = new List<Item>();//�U�I�]�t�Ϊ��ݩ�
}
