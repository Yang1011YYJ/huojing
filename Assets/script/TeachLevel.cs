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
    [SerializeField] bool InTeaching;

    [Header("UI組件")]//美化inspector用，會出現UI組件區域並將下面的眶進去
    //顯示對話的文字框
    public TextMeshProUGUI TextLabel;
    //文本組件
    public GameObject TextPanel;
    //角色頭像顯示
    public UnityEngine.UI.Image FaceImage;
    public Sprite NPCNone;
    public Sprite NPCAngry;
    //角色名
    public TextMeshProUGUI Name;
    //顯示名稱的變數
    string[] nameset;

    [Header("腳本")]
    AnimatorControll animatorControll;
    DialogueLab dialogueLab;

    [Header("文本文件")]
    //對話文本
    public TextAsset TextFileTeaching;
    [SerializeField] TextAsset CurrentText;
    //文本行數
    [SerializeField]private int index;
    //文字輸出速度
    public float TextSpeed = 0.1f;
    //待輸出的文字
    [SerializeField] private List<string> TextList = new List<string>();

    [Header("文本判斷")]
    //當行是否輸出完畢
    [SerializeField] private bool TextFinished = false;
    //取消輸出文本(跳過判斷)
    [SerializeField] private bool CancelText = false;
    ////進入關卡環節
    //public bool InLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        //啟用遮罩
        MaskOverLay.SetActive(false);
        InTeaching = false;

        animatorControll = gameObject.GetComponent<AnimatorControll>();
        dialogueLab = gameObject.GetComponent<DialogueLab>();

        //隱藏文本框
        TextPanel.SetActive(false);
    }


    private void Update()
    {
        if(dialogueLab.InLevel && !InTeaching)
        {//如果對話結束 進入關卡環節且尚未觸發教學
            InTeaching = true;//啟用引導教學
            dialogueLab.InLevel = false;//進入教學對話所以還沒開始關卡
            //指定讀取的文件為實驗室文本
            CurrentText = TextFileTeaching;
            //讀取文件
            GetTextFromFile(CurrentText);
            //開始打字
            TextFinished = true;
            //開始輸入文字
            StartCoroutine(SetTextUI());
            string[] nameset = TextList[index].Split('&');
            Name.text = nameset[1].ToString();
            TextPanel.SetActive(true);//前導對話
        }

        if ((Input.GetKeyDown(KeyCode.Q)) && TextPanel.activeSelf && InTeaching)
        {//如果想要觸發對話繼續的Q鍵
            //if (dialogueLab.InLevel)//如果進入引導步驟
            //{
            //    TextPanel.SetActive(false);//關掉對話框
            //    MaskOverLay.SetActive(true);//顯示遮罩
            //}
            if(index == TextList.Count)
            {//文件結束

            }
            else if (InTeaching && TextFinished)
            {
                StartCoroutine(SetTextUI());
            }
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
            case '5'://旁白說的話
                FaceImage.color = new Color(255, 255, 255, 0);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '6'://空白處==執行引導部分
                FaceImage.sprite = null;
                index = index + 1;
                dialogueLab.InLevel = true;
                break;

            case '8'://NPC生氣表情
                index = index + 1;
                FaceImage.sprite = NPCAngry;
                FaceImage.color = new Color(255, 255, 255, 255);
                index = index + 1;
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
    public void EnableMask()
    {
        MaskOverLay.SetActive(true);
    }

    public void DisableMask()
    {
        MaskOverLay.SetActive(false);
    }
}
