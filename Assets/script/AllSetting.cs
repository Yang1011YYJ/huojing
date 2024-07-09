using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllSetting : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] Graphic[] allGraphics;
    public GameObject ExitPanel;
    public bool ExitChoose;
    public GameObject Player;

    [Header("腳本")]
    [SerializeField] AnimatorControll AnimatorControll;
    [SerializeField] DialogueLab DialogueLab;
    [SerializeField] Setting Setting;

    private void Start()
    {
        // 獲取所有UI元素的Graphic組件
        allGraphics = FindObjectsOfType<Graphic>();

        //退出遊戲
        ExitPanel.SetActive(false);
        ExitChoose = true;

        //播過的眨眼動畫次數
        AnimatorControll = gameObject.GetComponent<AnimatorControll>();
        //眨眼動畫是否播放完畢
        DialogueLab = gameObject.GetComponent<DialogueLab>();
        //眨眼動畫是否播放完畢
        Setting = gameObject.GetComponent<Setting>();
    }

    private void Update()
    {
        //如果開場眨眼動畫撥完就顯示文本框
        if (AnimatorControll.IsAnimatorFinished && AnimatorControll.AnimationTimes == 1)
        {
            DialogueLab.TextPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(true);
            Debug.Log("Esc");
            ExitChoose = true;
        }//退出遊戲確認框

        if (ExitPanel.activeSelf && ExitChoose)
        {//當退出遊戲的確認框出現
            // 禁用所有UI元素的Raycast
            SetAllRaycastTargets(false);
            // 啟用彈出框內UI元素的Raycast
            SetRaycastTarget(ExitPanel, true);
        }//退出遊戲

        if (Setting.SettingPanel.activeSelf)
        {
            Player.SetActive(false);
        }//顯示設定框就隱藏玩家

        if (DialogueLab.TextPanel.activeSelf)
        {//對話框啟用表示沒有在關卡
            DialogueLab.InLevel = false;
            Player.SetActive(false);
        }
        else if (!DialogueLab.TextPanel.activeSelf && AnimatorControll.AnimationTimes > 1)//Inlevel表示在關卡場景(沒有對話或關卡啟動)
            //大於一表示開場劇情已經播完了
        { 
            DialogueLab.InLevel = true;
        }

        if (DialogueLab.InLevel) { Player.SetActive(true); }
    }
    // 隱藏彈出框
    public void HidePopup()
    {

        ExitPanel.SetActive(false);
        // 啟用所有UI元素的Raycast
        SetAllRaycastTargets(true);
    }
    // 設置所有UI元素的Raycast Target
    void SetAllRaycastTargets(bool status)
    {
        foreach (Graphic graphic in allGraphics)
        {
            Debug.Log($"Setting {graphic.gameObject.name} raycastTarget to {status}");
            graphic.raycastTarget = status;
        }
        // 单独处理对话框的Graphic组件
        if (DialogueLab.TextPanel != null)
        {
            SetRaycastTarget(DialogueLab.TextPanel, status);
        }
    }
    // 設置特定GameObject的所有子UI元素的Raycast Target
    void SetRaycastTarget(GameObject parent, bool status)
    {
        Graphic[] graphics = parent.GetComponentsInChildren<Graphic>();
        foreach (Graphic graphic in graphics)
        {
            graphic.raycastTarget = status;
        }
    }
    // 當選擇完成時呼叫此方法
    public void OnChoiceMade()
    {
        HidePopup();
    }

    public void ExitYes()
    {
        ExitChoose = true;
        Application.Quit();
    }
    public void ExitNo()
    {
        ExitChoose = true;
        OnChoiceMade();
    }
}
