using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("觸發按鈕")]
    public Button NPCTouch;
    public float fadeDuration = 1f; // 按鈕淡入時間
    public TextMeshProUGUI ButtonText;
    CanvasGroup NPCButtonCanvasGroup;

    private void Start()
    {
        NPCButtonCanvasGroup = NPCTouch.GetComponent<CanvasGroup>();
        NPCButtonCanvasGroup.alpha = 0f;
        NPCTouch.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player In");
            StartCoroutine(FadeInButton());
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player Out");
            NPCTouch.gameObject.SetActive(false);
        }
    }
    private IEnumerator FadeInButton()
    {
        float elapsedTime = 0f;
        NPCTouch.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            NPCButtonCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            NPCButtonCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        NPCButtonCanvasGroup.alpha = 1f; // 確保最終 alpha 為 1
    }

    private void Update()
    {

    }
}
