using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("角色")]
    //public GameObject PlayerSet;
    public float MoveSpeed = 30.0f;
    public float MaxMoveSpeed = 500f;
    Animator animator;
    Rigidbody rb;
    public int PlayerFace;
    public int PlayerFaceUD;
    Vector3 MoveAmount;//實際移動
    public int Player0;

    [Header("重力影響")]
    public float Gravity = 10f;

    [Header("背包UI")]
    public GameObject Bag;
    public GameObject BagIcon;
    //是否開啟布林值
    public bool IsOpen = false;

    [Header("關卡機制")]
    public bool ExitDoor = false;

    [Header("設定")]
    public GameObject SettingPanel;

    public EventSystem eventSystem;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Application.targetFrameRate = 60;
        IsOpen = false;//初始狀態為關閉
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        Bag.SetActive(false);
        BagIcon.SetActive(false);
        ExitDoor = false;
    }
    public void Update()
    {
        

        if (eventSystem.GetComponent<DialogueLab>().TextPanel.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else if (!eventSystem.GetComponent<DialogueLab>().TextPanel.activeSelf &&
            !eventSystem.GetComponent<AnimatorControll>().FadeOutPanel.activeSelf)
        {
            gameObject.SetActive(true);
        }
        // 移动方向检测
        Vector2 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        bool moveAxis = moveDirection != Vector2.zero;

        if (moveAxis)
        {
            // 获取当前速度
            Vector2 currentVelocity = rb.velocity;
            // 限制玩家的最大速度
            if (currentVelocity.magnitude > MaxMoveSpeed)
            {
                rb.velocity = rb.velocity.normalized * MaxMoveSpeed;
            }
            // 施加移动力
            rb.AddForce(moveDirection * MoveSpeed);

            //if (animator.GetBool("Fly") == true)//飛行時走路禁用
            //{
            //    animator.SetBool("Walk", false);
            //}
            if(gameObject.transform.position.y > Player0)
            {
                animator.SetBool("Fly", true);
            }
            if (moveDirection.y != 0)
            {//在飛
                animator.SetBool("Fly", true);
                if (moveDirection.x != 0)
                {//原本在走路
                    animator.SetBool("Walk", false);
                }
            }
            else if (moveDirection.y == 0)
            {//沒有在飛
                animator.SetBool("Fly", false);
                animator.SetBool("Walk", true);
            }
            //面向
            if (moveDirection.x > 0)//向右移動
            {
                if (transform.localScale.x < 0)
                {//原本朝左
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (moveDirection.x < 0)//向左移動
            {
                if (transform.localScale.x > 0)//原本朝右
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        else if(!moveAxis && gameObject.transform.position.y > Player0)//還沒回到地面
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Fly", true);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Fly", false);
        }

        //if (eventSystem.GetComponent<LabLevel>().TextPanel.activeSelf)
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    gameObject.SetActive(true);
        //}

        OpenBag();


    }
    private void FixedUpdate()
    {
        // 添加重力影响
        rb.AddForce(Vector2.down * Gravity * Time.deltaTime * 30);
    }


    void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))//按下按鍵
        {
            IsOpen = !IsOpen;//設為相反(開啟則關閉、關閉則開啟)
            Bag.SetActive(IsOpen);//直接設為對應狀態就好
        }

        if(Bag.activeSelf == false)
        {
            IsOpen = false;
        }
    }

    public void BagOnClick()
    {
        IsOpen = !IsOpen;
        Bag.SetActive(IsOpen);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Door")
        {
            ExitDoor = true;
        }
    }
}
