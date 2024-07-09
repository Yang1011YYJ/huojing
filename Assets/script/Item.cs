using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New File"/*�R�W*/,
    menuName = "Inventory/New Item"/*�k����s����W�r*/)]//�Ыطs���󪺵��(�b�Ыطs���󪺵��]�w�۩w�q�ﶵ)
public class Item : ScriptableObject
{
    public string ItemName;//���~�W��
    public Sprite ItemImage;//���~�Ϯ�
    public int ItemHeid;//�������~�ƶq
    [TextArea]//�ŧi�o�O�@�Ӥ�r��J�ϰ�(�S�g�N�O�@��)
    public string ItemIntro;//����y�z
}