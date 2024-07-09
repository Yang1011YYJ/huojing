using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("設定")]
    public GameObject Player;//玩家角色
    public GameObject SettingIcon;//設定圖示
    public GameObject SettingPanel;//設定面板
    public GameObject UIAct;//互動物件
    public bool SettingPanelActive;//設定面板顯示
    public GameObject BackToMenuPanel;//確認框
    [SerializeField]private UnityEngine.UI.Button SettingIconClick;
    

    void Start()
    {
        //設定面板不開啟
        SettingPanelActive = false;
        SettingPanel.SetActive(SettingPanelActive);

        //回到首頁面板關閉
        BackToMenuPanel.SetActive(false);

        //指定按鈕對應
        SettingIconClick = SettingIcon.GetComponent<UnityEngine.UI.Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))//按下X
        {
            //如果設定面板是關閉狀態
            //開啟設定面板
            //如果設定面板是開啟狀態
            //關閉設定面板
            ToggleSettingPanel();
        }
        if (Input.GetMouseButtonDown(0))
        {//如果滑鼠按下左鍵
            if(!IsPointerOverUIElement())
            {//如果滑鼠不在panel上方 或 不在互動按鍵上方
                //關閉設定面板
                CloseSettingPanel();
            }
            else
            {
                //Debug.Log("Pointer is over UI element.");
            }


        }
    }

    public void ToggleSettingPanel()//轉換成相反形式
    {
        SettingPanelActive = !SettingPanelActive;
        SettingPanel.SetActive(SettingPanelActive);
    }

    private void CloseSettingPanel()
    {
        SettingPanelActive = false;
        SettingPanel.SetActive(SettingPanelActive);
    }//關閉設定面板
    private bool IsPointerOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            //Debug.Log("Hit: " + result.gameObject.name);
            if (IsUIElement(result.gameObject))//如果點選任意空白處
            {
                if (result.gameObject.transform.IsChildOf(SettingPanel.transform))//如果點選的空白處是SettingPanel的子物件
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsUIElement(GameObject go)//點選處判定
    {//如果點選處的父物件可以追朔到護動物件則true
        Transform current = go.transform;
        while (current != null)
        {
            if (current.GetComponent<Slider>() != null ||
                current.GetComponent<Toggle>() != null ||
                current.GetComponent<UnityEngine.UI.Button>() != null ||
                current.GetComponent<Dropdown>() != null)
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }


    public void SettingIconOnClick()
    {
        ToggleSettingPanel();
    }
    public void BackToMenu()//點擊回到首頁的按鈕後
    {
        BackToMenuPanel.SetActive(true);//彈出確認框
    }

    public void BackToMenu_Yes()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BackToMenu_No()
    {
        BackToMenuPanel.SetActive(false);
    }
}
