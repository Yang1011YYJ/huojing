using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TeachLevel : MonoBehaviour
{
    [Header("�޾����d")]
    public GameObject MaskOverLay; // �B�n��H
    //���L�޾�
    public GameObject JumpTeach;
    [SerializeField]public bool InTeaching;

    void Start()
    {
        //�ҥξB�n
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
