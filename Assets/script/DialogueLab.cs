using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueLab : MonoBehaviour
{
    [Header("UI組件")]//美化inspector用，會出現UI組件區域並將下面的眶進去
    //顯示對話的文字框
    public TextMeshProUGUI TextLabel;
    //文本組件
    public GameObject TextPanel;
    //角色頭像顯示
    public Image FaceImage;
    public Sprite PlayerFace;
    public Sprite NPCNone;
    public Sprite NPCAngry;
    public Sprite NPCHow;
    //角色名
    public TextMeshProUGUI Name;
    //顯示名稱的變數
    string[] nameset;
    //跳過劇情
    public GameObject JumpDia;

    [Header("文本文件")]
    //對話文本
    public TextAsset TextFile1;
    public TextAsset TextFileNPCTalk;
    public TextAsset TextFileExit;
    public TextAsset TextFileTeaching;
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
    //取消輸出文本(跳過判斷)
    public bool CancelText = false;
    //進入關卡環節
    public bool InLevel = false;

    [Header("腳本")]
    AnimatorControll AnimatorControll;
    NPC NPCScript;

    [Header("選擇")]
    public GameObject Ignore;//無視驅趕
    public GameObject Ask;//詢問原因
    public GameObject Stay;//繼續探索
    public GameObject Leave;//離開
    public int Choice1 = -1;
    public bool ChoiceEnd;

    [Header("玩家資訊")]
    public bool SetPlayerInfor = false;
    public GameObject Player;

    [Header("NPC相關")]
    public GameObject NPC;
    public bool interactable;


    void Awake()//在OnEnable前面執行(Start在OnEnable後面)
    {
        ////未進入關卡
        //InLevel = false;
        AnimatorControll = gameObject.GetComponent<AnimatorControll>();
        //NPC可否互動
        NPCScript = NPC.GetComponent<NPC>();

        //指定讀取的文件為實驗室文本
        CurrentText = TextFile1;
        //讀取文件
        GetTextFromFile(CurrentText);
        //隱藏文本框
        TextPanel.SetActive(false);
        Ignore.SetActive(false);//無視選項
        Ask.SetActive(false);//詢問原因選項
        Leave.SetActive(false);//無視選項
        Stay.SetActive(false);//詢問原因選項
        ChoiceEnd = true;//選擇完畢布林值
        CancelText = false;//是否取消輸入
        SetPlayerInfor = false;//未需要更新資料
    }
    private void OnEnable()//一開始直接顯示第一行，然後index成為1
    {
        //開始打字
        TextFinished = true;
        //開始輸入文字
        StartCoroutine(SetTextUI());
        string[] nameset = TextList[index].Split('&');
        Name.text = nameset[1].ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Q)) && TextPanel.activeSelf && !InLevel)
        {//如果想要觸發對話繼續的Q鍵
            //如果文本結束
            if (index == TextList.Count)
            {
                if(CurrentText == TextFile1)
                {
                    //如果index已經到最後一行 == 對話結束(Count:計算行數)
                    //對話結束 == 開場對話劇情結束 == 進入關卡 == 遊戲開始
                    //關閉對話框
                    TextPanel.SetActive(false);
                    AnimatorControll.AnimationTimes++;//已經播過一次眨眼動畫 == 避免進入第二次開頭劇情
                    //淡出場景
                    StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
                    StartCoroutine(StartFind());
                }
                else
                {
                    TextPanel.SetActive(false);
                    Player.SetActive(true);
                    //重製行數
                    index = 0;
                    //歸零目前文本(置空)
                    CurrentText = null;
                }

                return;
            }
            //如果按下R鍵時文字已經輸出完畢 = 可以輸出下一行
            else if (TextFinished &&
                ChoiceEnd)
            {
                StartCoroutine(SetTextUI());//輸出下一行
            }
            else if (!TextFinished && !CancelText)//如果文字還沒輸出完(TextFished == false)且CancalText = false
                                                    //如果按下的時候還沒輸出完而且cancelText是否 = 要跳過
            {
                CancelText = true;
            }
        }
        //Ignore選項
        if (!ChoiceEnd)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChoiceEnd = true;
                Ignore.SetActive(false);//選項消失
                TextPanel.SetActive(false);
                gameObject.GetComponent<AnimatorControll>().AnimationTimes++;//已經播過一次
                                                                              //淡出場景
                StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
                StartCoroutine(StartFind());
                //重製行數
                index = 0;
            }
            else if (Input.GetKeyDown(KeyCode.R))//對話選項
            {
                ChoiceEnd = true;
                Ignore.SetActive(false);//選項消失
                Ask.SetActive(false);//選項消失
                StartCoroutine(SetTextUI());
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {//繼續探索
                ChoiceEnd = true;
                Stay.SetActive(false);//選項消失
                Leave.SetActive(false);//選項消失
                TextPanel.SetActive(false);
                Player.SetActive(true);
                //重製行數
                index = 0;
            }
            else if(Input.GetKeyDown(KeyCode.Y))
            {//離開
                ChoiceEnd = true;
                Leave.SetActive(false);//選項消失
                Stay.SetActive(false);//選項消失
                TextPanel.SetActive(false);
                gameObject.GetComponent<AnimatorControll>().AnimationTimes++;//已經播過一次
                                                                              //淡出場景
                StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
                StartCoroutine(NextScene());
                //重製行數
                index = 0;
            }
        }
        if (interactable && !TextPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                CurrentText = TextFileNPCTalk;
                GetTextFromFile(TextFileNPCTalk);
                TextPanel.SetActive(true);
                StartCoroutine(SetTextUI());
            }
        }

        
    }

    IEnumerator StartFind()
    {
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<AnimatorControll>().FadeOutPanel.SetActive(false);
        Player.SetActive(true);
        InLevel = true;
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2.5f);
        TextPanel.SetActive(false);
        SceneManager.LoadScene("City");
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
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
            case '1'://玩家說的話
                FaceImage.sprite = PlayerFace;
                FaceImage.color = new Color(255, 255, 255, 255);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '2'://NPC驅趕玩家 選項
                index++;
                ChoiceEnd = false;
                Name.text = " ";
                Ignore.SetActive(true);
                Ask.SetActive(true);
                break;

            case '3'://NPC說的話
                FaceImage.sprite = NPCNone;
                FaceImage.color = new Color(255, 255, 255, 255);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index ++;
                break;

            case '4'://玩家資訊更新
                SetPlayerInfor = true;
                index++;
                break;

            case '5'://旁白說的話
                FaceImage.color = new Color(255, 255, 255, 0);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '7'://逃出的門
                index++;
                ChoiceEnd = false;
                Name.text = " ";
                Stay.SetActive(true);//留下再探索一下
                Leave.SetActive(true);//離開實驗室
                break;

            case '8'://NPC生氣表情
                index = index + 1;
                FaceImage.sprite = NPCAngry;
                FaceImage.color = new Color(255, 255, 255, 255);
                index = index + 1;
                break;

            case '9'://NPC疑惑表情
                index = index + 1;
                FaceImage.sprite = NPCHow;
                FaceImage.color = new Color(255, 255, 255, 255);
                index = index + 1;
                break;

            case '('://說明
                index = index+1;
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index = index + 1;
                break;

            default:
                break;

        }

        //判斷按下CancelText的時候文字輸出完了沒
        int Letter = 0;
        //CancelText是否且文字未輸出完
        while(!CancelText && Letter < TextList[index].Length - 1)
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

    public void ChoiceIgnore()//忽略NPC的驅趕 選項
    {
        ChoiceEnd = true;
        Ignore.SetActive(false);//選項消失
        Ask.SetActive(false);//選項消失
        TextPanel.SetActive(false);
        gameObject.GetComponent<AnimatorControll>().AnimationTimes++;//已經播過一次
        //淡出場景
        StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
        StartCoroutine(StartFind());
        //重製行數
        index = 0;
    }
    public void ChoiceAsk()//詢問為什麼不能出現 選項
    {
        ChoiceEnd = true;
        Ignore.SetActive(false);//選項消失
        Ask.SetActive(false);//選項消失
        StartCoroutine(SetTextUI());
    }
    public void ChoiceLeave()//離開詢問 離開實驗室 選項
    {
        ChoiceEnd = true;
        Leave.SetActive(false);//選項消失
        Stay.SetActive(false);//選項消失
        TextPanel.SetActive(false);
        //淡出場景
        StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
        StartCoroutine(NextScene());
        //重製行數
        index = 0;
    }
    public void ChoiceStay()//離開詢問 繼續探索 選項
    {
        Player.transform.position = new Vector3(2323f, -142.8f, 17.16908f);
        ChoiceEnd = true;
        Leave.SetActive(false);//選項消失
        Stay.SetActive(false);//選項消失
        TextPanel.SetActive (false);
        Player.SetActive(true);
    }

    public void ONCLICKset()//點對話框可繼續對話
    {
        if (TextPanel.activeSelf && !InLevel && ChoiceEnd)
        {
            //如果文本結束
            if (index == TextList.Count)
            {
                //如果index已經到最後一行 == 對話結束(Count:計算行數)
                //關閉對話框
                TextPanel.SetActive(false);
                gameObject.GetComponent<AnimatorControll>().AnimationTimes++;//已經播過一次
                //淡出場景
                StartCoroutine(gameObject.GetComponent<AnimatorControll>().FadeOutCoroutine());
                StartCoroutine(StartFind());
                //重製行數
                index = 0;

                return;
            }
            else//文本還沒結束
            {
                //如果按下R鍵時文字已經輸出完畢 = 可以輸出下一行
                if (TextFinished &&
                    (Ignore.activeSelf == false &&
                    Ask.activeSelf == false/* &&
                    Feeding.activeSelf == false &&
                    Reject.activeSelf == false*/))
                {
                    StartCoroutine(SetTextUI());//輸出下一行
                }
                else if (!TextFinished && !CancelText)//如果文字還沒輸出完(TextFished == false)且CancalText = false
                                                      //如果按下的時候還沒輸出完而且cancelText是否 = 要跳過
                {
                    CancelText = true;
                }
            }
        }
        else if (TextPanel.activeSelf && InLevel)//關卡階段
        {

        }
    }

    public void JumpDialogue()//跳過劇情
    {
        if (!InLevel)
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
