using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LabLevel : MonoBehaviour
{
    [Header("UI組件")]//美化inspector用，會出現UI組件區域並將下面的眶進去
    //顯示對話的文字框
    public TextMeshProUGUI TextLabel;
    //文本組件
    public GameObject TextPanel;
    //角色頭像顯示
    public Image FaceImage;
    public Sprite PlayerFace;
    //角色名
    public TextMeshProUGUI Name;
    //顯示名稱的變數
    string[] nameset;
    //跳過劇情
    public GameObject JumpDia;

    [Header("文本文件")]
    //對話文本
    public TextAsset TextFileArm;
    public TextAsset TextFileAlready;
    public TextAsset TextFileBox;
    TextAsset CurrentText;
    //文本行數
    public int index;
    //文字輸出速度
    public float TextSpeed = 0.1f;
    //待輸出的文字
    private List<string> TextList = new List<string>();

    [Header("文本判斷")]
    //當行是否輸出完畢
    public bool TextFinished = false;
    public GameObject eventSystem;
    //取消輸出文本(跳過判斷)
    public bool CancelText = false;

    [Header("關卡")]
    bool InLevel;
    //機器手臂
    public int MachineTimes = 0;
    public GameObject Machine;
    Button MachineButton;
    //滑動方塊(感應卡)
    public GameObject Box;
    public bool BoxTimes;
    Button BoxButton;

    [Header("滑方塊")]
    public GameObject BoxAll;
    public GameObject Card;
    Animator CardAni;
    public GameObject SquareExit;
    public bool ItemIn;

    [Header("玩家")]
    public GameObject Player;

    [Header("背包系統")]
    public Inventory MyBag;


    public Camera MainCamera;

    void Awake()
    {
        MachineButton = Machine.GetComponent<Button>();
        MachineButton.interactable = false;
        BoxButton = Box.GetComponent<Button>();
        BoxButton.interactable = false;
        BoxAll.SetActive(false);
        Card.SetActive(false);
        CardAni = Card.GetComponent<Animator>();
        ItemIn = false;

        InLevel = eventSystem.GetComponent<DialogueLab>().InLevel;
    }

    private void Update()
    {
        //動畫撥完才可以啟用關卡觸發
        if (InLevel && !TextPanel.activeSelf)
        {//如果進入關卡環節且未觸發對話模式
            MachineButton.interactable = true;
            BoxButton.interactable = true;
        }
        else if (InLevel && TextPanel.activeSelf)
        {//如果觸發對話模式
            //該觸發機制暫時無法被再次點擊
            MachineButton.interactable= false;
            BoxButton.interactable= false;
        }
        //文本控制
        if ((Input.GetKeyDown(KeyCode.Q)) && TextPanel.activeSelf && MachineButton.interactable && !InLevel)
        {
            //如果文本結束
            if (index == TextList.Count)
            {
                //如果index已經到最後一行 == 對話結束(Count:計算行數)
                //關閉對話框
                TextPanel.SetActive(false);
                Player.SetActive(true);
                //重製行數
                index = 0;
                return;
            }
            //如果按下R鍵時文字已經輸出完畢 = 可以輸出下一行
            else if (!TextFinished && !CancelText)//如果文字還沒輸出完(TextFished == false)且CancalText = false
                                                  //如果按下的時候還沒輸出完而且cancelText是否 = 要跳過
            {
                CancelText = true;
            }
        }
        if(Card.activeSelf && !TextPanel.activeSelf)
        {
            //CardAni.SetTrigger("GetCard");//撥放動畫
            StartCoroutine(Wait(0.35f));
        }
        ////如果感應卡的renderer層改變 = 獲得感應卡
        //if(Card.GetComponent<SpriteRenderer>().sortingLayerName == "GameSquare" && !TextPanel.activeSelf)
        //{
        //    index = 5;
        //    TextPanel.SetActive(true);
        //    StartCoroutine(SetTextUI());
        //}
    }
    IEnumerator Wait(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Destroy(Card);
        MyBag.ItemList.Add(Card.GetComponent<ItemOnWorld>().ThisItem);
        Player.SetActive(true);
    }
    public void ArmOnclick()//機器手臂觸發機制
    {
        if (!TextPanel.activeSelf && !BoxTimes)
        {//如果已經開啟對話就不能再開一次
            MachineTimes += 1;
            if(MachineTimes < 5)
            {
                CurrentText = TextFileArm;
            }
            else if(MachineTimes >= 5)
            {
                CurrentText = TextFileAlready;
            }
            GetTextFromFile(CurrentText);
            TextPanel.SetActive(true);
            StartCoroutine(SetTextUI());
        }
    }
    //盒子觸發機制
    public void BoxOnClick()
    {
        if(!TextPanel.activeSelf && !BoxTimes)
        {
            BoxTimes = false;
            CurrentText = TextFileBox;
            GetTextFromFile(CurrentText);
            StartCoroutine(SetTextUI());
            TextPanel.SetActive(true);//啟用對話框
            //場景是0 canvas是2
            //sorting Layer是camera檢視物件的順序
            //Layer可以用來分類物件 例如要碰撞的物件
            //BoxAll.SetActive(true);//顯示遊戲介面
            Card.SetActive(true);
            Player.GetComponent<Player>().BagIcon.SetActive(true);
        }

    }

    public void PanelOnClick()
    {
        if (!eventSystem.GetComponent<DialogueLab>().InLevel && eventSystem.GetComponent<DialogueLab>().ChoiceEnd)
        {//不在關卡場景內(有對話或是正在闖關)
            //如果文本結束
            if (index == TextList.Count)
            {
                //如果index已經到最後一行 == 對話結束(Count:計算行數)
                //關閉對話框
                TextPanel.SetActive(false);
                //重製行數
                index = 0;
                return;
            }
            //如果按下R鍵時文字已經輸出完畢 = 可以輸出下一行
            else if (TextFinished)
            {
                StartCoroutine(SetTextUI());//輸出下一行
            }
            //如果按下R鍵時文字已經輸出完畢 = 可以輸出下一行
            else if (!TextFinished && !CancelText)//如果文字還沒輸出完(TextFished == false)且CancalText = false
                                                  //如果按下的時候還沒輸出完而且cancelText是否 = 要跳過
            {
                CancelText = true;
            }
        }
    }

    IEnumerator SetTextUI()
    {//產生打字效果
        //開始打字
        TextFinished = false;
        //將文字欄清空
        TextLabel.text = "";

        //判斷括號
        switch (TextList[index][0])
        {
            case '1'://玩家名
                FaceImage.sprite = PlayerFace;
                FaceImage.color = new Color(255, 255, 255, 255);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '2'://滑方塊關卡
                index++;
                TextPanel.SetActive(false);//關閉對話框
                BoxTimes = true;
                eventSystem.GetComponent<DialogueLab>().JumpDia.SetActive(false);
                break;

            default:
                break;

        }

        //判斷按下CancelText的時候文字輸出完了沒
        int Letter = 0;
        //CancelText是否且文字未輸出完
        while (!CancelText && Letter < TextList[index].Length - 1)
        {
            //如果CancelText是否且沒輸出完就繼續輸出
            //如果CancelText是正就跳出迴圈
            //打字效果輸出
            TextLabel.text += TextList[index][Letter];
            Letter++;
            yield return new WaitForSeconds(TextSpeed);
        }
        //如果跳出迴圈就直接把該行顯示出來
        TextLabel.text = TextList[index];
        CancelText = false;//CancelText改為否(不要取消文字輸出)
        //該行文字都已經打完了
        TextFinished = true;
        //可以下一行了，index加一
        index++;

    }

    void GetTextFromFile(TextAsset file)
    {
        //每次都要清空
        TextList.Clear();
        //序號歸零
        index = 0;

        //為了將切割好輸出的字串指定給List 指定一個變量
        string[] LineData = file.text/*將文本轉換為字串*/.Split('\n');//將轉換完的字串用換行符號切割
        //這邊切好的string會長這樣：[胡金,這是哪裡,胡金,我好渴,...]

        //這裡foreach分完會長這樣
        foreach (var line in LineData)//每一行
        {
            TextList.Add(line);//把讀取到的句子加入輸出
        }
    }

    //跳過劇情
    public void JumpDialogue()
    {
        if (eventSystem.GetComponent<DialogueLab>().InLevel)
        {
            index = TextList.Count - 2;
            try
            {
                StartCoroutine(SetTextUI());
            }
            catch
            {
                Debug.Log("Error;");
            }
        }
    }
}
