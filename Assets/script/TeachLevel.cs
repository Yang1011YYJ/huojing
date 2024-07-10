using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TeachLevel : MonoBehaviour
{
    [Header("引導關卡")]
    public GameObject MaskOverLay; // 遮罩對象
    //跳過引導
    public GameObject JumpTeach;
    [SerializeField]public bool InTeaching;

    void Start()
    {
        //啟用遮罩
        MaskOverLay.SetActive(false);
        InTeaching = false;
    }
    private void Update()
    {
        //if()
    }




    public void EnableMask()
    {
        MaskOverLay.SetActive(true);
    }

    public void DisableMask()
    {
        MaskOverLay.SetActive(false);
    }
}
