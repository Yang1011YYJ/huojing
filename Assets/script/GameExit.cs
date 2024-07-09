using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameExit : MonoBehaviour
{
    [Header("感應卡物件")]
    public GameObject Card;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "感應卡")
        {
            Debug.Log("card in");
            Card.GetComponent<SpriteRenderer>().sortingLayerName = "Canvas";
            Card.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

}
