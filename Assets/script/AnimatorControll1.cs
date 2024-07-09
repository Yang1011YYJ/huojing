using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class AnimatorControll1 : MonoBehaviour
{
    // Start is called before the first frame update
    //播放前等待秒數
    public float InSceneWaitSeconds = 3.0f;

    [Header("淡出場景")]
    public GameObject FadeOutPanel; //場景淡出
    public float FadeOutTime = 2f;//淡出速度
    CanvasGroup FadeOutPanelCanvasGroup;

    [Header("開始")]
    public GameObject Start;
    public float StartSpeed = 0.5f;
    public bool StartStartBlink = false;
    //[Header("提示動畫效果")]
    CanvasGroup StartCanvasGroup;
    public float StartFadeSpeed = 0.1f;

    void Awake()//在OnEnable前面執行(Start在OnEnable後面)
    {
        StartStartBlink = false;//開始閃爍

        FadeOutPanelCanvasGroup = FadeOutPanel.GetComponent<CanvasGroup>();
        FadeOutPanelCanvasGroup.alpha = 0f;
        FadeOutPanel.SetActive(false);
    }
    public void Update()
    {
        //提示動畫:閃爍
        if (!StartStartBlink)
        {
            StartCoroutine(StartBlink(Start));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void StartOnClick()
    {
        StartCoroutine(FadeOutCoroutine());

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
        SceneManager.LoadScene("Lab");
    }
    public IEnumerator StartBlink(GameObject GameObjectSet)
    {//提示閃爍動畫
        StartStartBlink = true;
        StartCanvasGroup = GameObjectSet.GetComponent<CanvasGroup>();//要閃爍的物件

        //FadeIn
        StartCanvasGroup.alpha = 0.0f;
        while (StartCanvasGroup.alpha < 1.0f)
        {
            StartCanvasGroup.alpha += Time.deltaTime * StartFadeSpeed;
        }
        StartCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(StartSpeed);
        while (StartCanvasGroup.alpha > 0f)
        {
            StartCanvasGroup.alpha -= Time.deltaTime * StartSpeed;
        }
        StartCanvasGroup.alpha = 0f;
        yield return new WaitForSeconds(StartSpeed);
        StartStartBlink = false;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
