using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LabLevel : MonoBehaviour
{
    [Header("UI�ե�")]//����inspector�ΡA�|�X�{UI�ե�ϰ�ñN�U�������i�h
    //��ܹ�ܪ���r��
    public TextMeshProUGUI TextLabel;
    //�奻�ե�
    public GameObject TextPanel;
    //�����Y�����
    public Image FaceImage;
    public Sprite PlayerFace;
    //����W
    public TextMeshProUGUI Name;
    //��ܦW�٪��ܼ�
    string[] nameset;
    //���L�@��
    public GameObject JumpDia;

    [Header("�奻���")]
    //��ܤ奻
    public TextAsset TextFileArm;
    public TextAsset TextFileAlready;
    public TextAsset TextFileBox;
    TextAsset CurrentText;
    //�奻���
    public int index;
    //��r��X�t��
    public float TextSpeed = 0.1f;
    //�ݿ�X����r
    private List<string> TextList = new List<string>();

    [Header("�奻�P�_")]
    //���O�_��X����
    public bool TextFinished = false;
    public GameObject eventSystem;
    //������X�奻(���L�P�_)
    public bool CancelText = false;

    [Header("���d")]
    bool InLevel;
    //�������u
    public int MachineTimes = 0;
    public GameObject Machine;
    Button MachineButton;
    //�ưʤ��(�P���d)
    public GameObject Box;
    public bool BoxTimes;
    Button BoxButton;

    [Header("�Ƥ��")]
    public GameObject BoxAll;
    public GameObject Card;
    Animator CardAni;
    public GameObject SquareExit;
    public bool ItemIn;

    [Header("���a")]
    public GameObject Player;

    [Header("�I�]�t��")]
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
        //�ʵe�����~�i�H�ҥ����dĲ�o
        if (InLevel && !TextPanel.activeSelf)
        {//�p�G�i�J���d���`�B��Ĳ�o��ܼҦ�
            MachineButton.interactable = true;
            BoxButton.interactable = true;
        }
        else if (InLevel && TextPanel.activeSelf)
        {//�p�GĲ�o��ܼҦ�
            //��Ĳ�o����ȮɵL�k�Q�A���I��
            MachineButton.interactable= false;
            BoxButton.interactable= false;
        }
        //�奻����
        if ((Input.GetKeyDown(KeyCode.Q)) && TextPanel.activeSelf && MachineButton.interactable && !InLevel)
        {
            //�p�G�奻����
            if (index == TextList.Count)
            {
                //�p�Gindex�w�g��̫�@�� == ��ܵ���(Count:�p����)
                //������ܮ�
                TextPanel.SetActive(false);
                Player.SetActive(true);
                //���s���
                index = 0;
                return;
            }
            //�p�G���UR��ɤ�r�w�g��X���� = �i�H��X�U�@��
            else if (!TextFinished && !CancelText)//�p�G��r�٨S��X��(TextFished == false)�BCancalText = false
                                                  //�p�G���U���ɭ��٨S��X���ӥBcancelText�O�_ = �n���L
            {
                CancelText = true;
            }
        }
        if(Card.activeSelf && !TextPanel.activeSelf)
        {
            //CardAni.SetTrigger("GetCard");//����ʵe
            StartCoroutine(Wait(0.35f));
        }
        ////�p�G�P���d��renderer�h���� = ��o�P���d
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
    public void ArmOnclick()//�������uĲ�o����
    {
        if (!TextPanel.activeSelf && !BoxTimes)
        {//�p�G�w�g�}�ҹ�ܴN����A�}�@��
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
    //���lĲ�o����
    public void BoxOnClick()
    {
        if(!TextPanel.activeSelf && !BoxTimes)
        {
            BoxTimes = false;
            CurrentText = TextFileBox;
            GetTextFromFile(CurrentText);
            StartCoroutine(SetTextUI());
            TextPanel.SetActive(true);//�ҥι�ܮ�
            //�����O0 canvas�O2
            //sorting Layer�Ocamera�˵����󪺶���
            //Layer�i�H�ΨӤ������� �Ҧp�n�I��������
            //BoxAll.SetActive(true);//��ܹC������
            Card.SetActive(true);
            Player.GetComponent<Player>().BagIcon.SetActive(true);
        }

    }

    public void PanelOnClick()
    {
        if (!eventSystem.GetComponent<DialogueLab>().InLevel && eventSystem.GetComponent<DialogueLab>().ChoiceEnd)
        {//���b���d������(����ܩάO���b����)
            //�p�G�奻����
            if (index == TextList.Count)
            {
                //�p�Gindex�w�g��̫�@�� == ��ܵ���(Count:�p����)
                //������ܮ�
                TextPanel.SetActive(false);
                //���s���
                index = 0;
                return;
            }
            //�p�G���UR��ɤ�r�w�g��X���� = �i�H��X�U�@��
            else if (TextFinished)
            {
                StartCoroutine(SetTextUI());//��X�U�@��
            }
            //�p�G���UR��ɤ�r�w�g��X���� = �i�H��X�U�@��
            else if (!TextFinished && !CancelText)//�p�G��r�٨S��X��(TextFished == false)�BCancalText = false
                                                  //�p�G���U���ɭ��٨S��X���ӥBcancelText�O�_ = �n���L
            {
                CancelText = true;
            }
        }
    }

    IEnumerator SetTextUI()
    {//���ͥ��r�ĪG
        //�}�l���r
        TextFinished = false;
        //�N��r��M��
        TextLabel.text = "";

        //�P�_�A��
        switch (TextList[index][0])
        {
            case '1'://���a�W
                FaceImage.sprite = PlayerFace;
                FaceImage.color = new Color(255, 255, 255, 255);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '2'://�Ƥ�����d
                index++;
                TextPanel.SetActive(false);//������ܮ�
                BoxTimes = true;
                eventSystem.GetComponent<DialogueLab>().JumpDia.SetActive(false);
                break;

            default:
                break;

        }

        //�P�_���UCancelText���ɭԤ�r��X���F�S
        int Letter = 0;
        //CancelText�O�_�B��r����X��
        while (!CancelText && Letter < TextList[index].Length - 1)
        {
            //�p�GCancelText�O�_�B�S��X���N�~���X
            //�p�GCancelText�O���N���X�j��
            //���r�ĪG��X
            TextLabel.text += TextList[index][Letter];
            Letter++;
            yield return new WaitForSeconds(TextSpeed);
        }
        //�p�G���X�j��N������Ӧ���ܥX��
        TextLabel.text = TextList[index];
        CancelText = false;//CancelText�אּ�_(���n������r��X)
        //�Ӧ��r���w�g�����F
        TextFinished = true;
        //�i�H�U�@��F�Aindex�[�@
        index++;

    }

    void GetTextFromFile(TextAsset file)
    {
        //�C�����n�M��
        TextList.Clear();
        //�Ǹ��k�s
        index = 0;

        //���F�N���Φn��X���r����w��List ���w�@���ܶq
        string[] LineData = file.text/*�N�奻�ഫ���r��*/.Split('\n');//�N�ഫ�����r��δ���Ÿ�����
        //�o����n��string�|���o�ˡG[�J��,�o�O����,�J��,�ڦn��,...]

        //�o��foreach�����|���o��
        foreach (var line in LineData)//�C�@��
        {
            TextList.Add(line);//��Ū���쪺�y�l�[�J��X
        }
    }

    //���L�@��
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
