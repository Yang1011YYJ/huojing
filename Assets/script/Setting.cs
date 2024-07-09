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

    [Header("�]�w")]
    public GameObject Player;//���a����
    public GameObject SettingIcon;//�]�w�ϥ�
    public GameObject SettingPanel;//�]�w���O
    public GameObject UIAct;//���ʪ���
    public bool SettingPanelActive;//�]�w���O���
    public GameObject BackToMenuPanel;//�T�{��
    [SerializeField]private UnityEngine.UI.Button SettingIconClick;
    

    void Start()
    {
        //�]�w���O���}��
        SettingPanelActive = false;
        SettingPanel.SetActive(SettingPanelActive);

        //�^�쭺�����O����
        BackToMenuPanel.SetActive(false);

        //���w���s����
        SettingIconClick = SettingIcon.GetComponent<UnityEngine.UI.Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))//���UX
        {
            //�p�G�]�w���O�O�������A
            //�}�ҳ]�w���O
            //�p�G�]�w���O�O�}�Ҫ��A
            //�����]�w���O
            ToggleSettingPanel();
        }
        if (Input.GetMouseButtonDown(0))
        {//�p�G�ƹ����U����
            if(!IsPointerOverUIElement())
            {//�p�G�ƹ����bpanel�W�� �� ���b���ʫ���W��
                //�����]�w���O
                CloseSettingPanel();
            }
            else
            {
                //Debug.Log("Pointer is over UI element.");
            }


        }
    }

    public void ToggleSettingPanel()//�ഫ���ۤϧΦ�
    {
        SettingPanelActive = !SettingPanelActive;
        SettingPanel.SetActive(SettingPanelActive);
    }

    private void CloseSettingPanel()
    {
        SettingPanelActive = false;
        SettingPanel.SetActive(SettingPanelActive);
    }//�����]�w���O
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
            if (IsUIElement(result.gameObject))//�p�G�I����N�ťճB
            {
                if (result.gameObject.transform.IsChildOf(SettingPanel.transform))//�p�G�I�諸�ťճB�OSettingPanel���l����
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsUIElement(GameObject go)//�I��B�P�w
    {//�p�G�I��B��������i�H�l�Ҩ��@�ʪ���htrue
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
    public void BackToMenu()//�I���^�쭺�������s��
    {
        BackToMenuPanel.SetActive(true);//�u�X�T�{��
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
