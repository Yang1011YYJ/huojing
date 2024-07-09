using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class AnimatorControll : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("眨眼")]
    public GameObject UP;
    Animator UpAnimator;
    public GameObject DOWN;
    Animator DownAnimator;
    public GameObject Vague;
    Animator VagueAnimator;
    //是否播放完畢進幕動畫
    public bool IsAnimatorFinished = false;
    public float AnimationTimes = 0;
    //播放前等待秒數
    public float InSceneWaitSeconds = 3.0f;

    [Header("淡出場景")]
    public GameObject FadeOutPanel; //場景淡出
    public float FadeOutTime = 2f;//淡出速度
    CanvasGroup FadeOutPanelCanvasGroup;

    [Header("提示")]
    public GameObject hint;//對話繼續的提示
    public float hintSpeed = 0.5f;
    public bool hintStartBlink = false;
    //[Header("提示動畫效果")]
    CanvasGroup hintCanvasGroup;
    public float hintFadeSpeed = 0.1f;

    [Header("跳過劇情")]
    public GameObject JumpDia;
    public float JumpSpeed = 0.5f;
    public bool JumpStartBlink = false;
    //[Header("提示動畫效果")]
    CanvasGroup JumpCanvasGroup;
    public float JumpFadeSpeed = 0.1f;


    void Awake()//在OnEnable前面執行(Start在OnEnable後面)
    {
        hintStartBlink = false;//對話繼續的提示還沒開始閃爍
        JumpStartBlink = false;//跳過開頭對話還沒開始開始閃爍

        //開頭對話結束後的淡出動畫
        FadeOutPanelCanvasGroup = FadeOutPanel.GetComponent<CanvasGroup>();
        FadeOutPanelCanvasGroup.alpha = 0f;
        FadeOutPanel.SetActive(false);
    }

    void Start()
    {
        //眨眼動畫的物件對應
        UpAnimator = UP.GetComponent<Animator>();
        DownAnimator = DOWN.GetComponent<Animator>();
        VagueAnimator = Vague.GetComponent<Animator>();

        //撥放過動畫次數(眨眼動畫)
        AnimationTimes = 0;
        
        //不要立刻撥動畫
        //等3秒再撥放眨眼動畫
        StartCoroutine(WaitAndPlayAnimation());
    }
    public void Update()
    {


        //提示動畫:閃爍
        if (!hintStartBlink)
        {
            StartCoroutine(HintBlink(hint));
        }
        //提示動畫:閃爍
        if (!JumpStartBlink)
        {
            StartCoroutine(JumpBlink(JumpDia));
        }

    }
    public IEnumerator FadeOutCoroutine()
    {//淡出動畫
        FadeOutPanel.SetActive(true);
        float timer = 0f;
        while (timer < FadeOutTime)//當時間還沒到設定的(2秒)
        {
            FadeOutPanelCanvasGroup.alpha = Mathf.Lerp(0f/*開始*/, 1f/*結束*/, timer / FadeOutTime/*時間長度*/);
            timer += Time.deltaTime;
            yield return null;
        }

        // 確保最终透明度是 1
        FadeOutPanelCanvasGroup.alpha = 1f;
    }
    public IEnumerator HintBlink(GameObject GameObjectSet)
    {//提示閃爍動畫
        hintStartBlink = true;
        hintCanvasGroup = GameObjectSet.GetComponent<CanvasGroup>();//要閃爍的物件

        //FadeIn
        hintCanvasGroup.alpha = 0.0f;
        while (hintCanvasGroup.alpha < 1.0f)
        {
            hintCanvasGroup.alpha += Time.deltaTime * hintFadeSpeed;
        }
        hintCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(hintSpeed);
        while (hintCanvasGroup.alpha > 0f)
        {
            hintCanvasGroup.alpha -= Time.deltaTime * hintSpeed;
        }
        hintCanvasGroup.alpha = 0f;
        yield return new WaitForSeconds(hintSpeed);
        hintStartBlink = false;
    }
    public IEnumerator JumpBlink(GameObject GameObjectSet)
    {//提示閃爍動畫
        JumpStartBlink = true;
        JumpCanvasGroup = GameObjectSet.GetComponent<CanvasGroup>();//要閃爍的物件

        //FadeIn
        JumpCanvasGroup.alpha = 0.0f;
        while (JumpCanvasGroup.alpha < 1.0f)
        {
            JumpCanvasGroup.alpha += Time.deltaTime * JumpSpeed;
        }
        JumpCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(JumpSpeed);
        while (JumpCanvasGroup.alpha > 0f)
        {
            JumpCanvasGroup.alpha -= Time.deltaTime * JumpSpeed;
        }
        JumpCanvasGroup.alpha = 0f;
        yield return new WaitForSeconds(JumpSpeed);
        JumpStartBlink = false;
    }
    public IEnumerator WaitAndPlayAnimation()
    {//等待三秒後開始眨眼動畫
        IsAnimatorFinished = false;
        
        //等待三秒
        yield return new WaitForSeconds(InSceneWaitSeconds);

        //撥放動畫
        UpAnimator.SetTrigger("Blink");
        DownAnimator.SetTrigger("Blink");
        VagueAnimator.SetTrigger("Blink");

        //等待播放完畢
        yield return new WaitForSeconds(4.5f);
        //動畫播放完畢
        IsAnimatorFinished = true;
        UP.SetActive(false);
        DOWN.SetActive(false);
        Vague.SetActive(false);
        //播過眨眼動畫一次
        AnimationTimes++;

        //StartCoroutine(LookEnvironment());
    }
}
