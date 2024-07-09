using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Ĳ�o���s")]
    public Button NPCTouch;
    public float fadeDuration = 1f; // ���s�H�J�ɶ�
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

        NPCButtonCanvasGroup.alpha = 1f; // �T�O�̲� alpha �� 1
    }

    private void Update()
    {

    }
}
