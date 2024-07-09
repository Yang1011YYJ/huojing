using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TeachLevel : MonoBehaviour
{
    [Header("�޾����d")]
    public GameObject MaskOverLay; // �B�n��H
    //���L�޾�
    public GameObject JumpTeach;
    [SerializeField] bool InTeaching;

    [Header("UI�ե�")]//����inspector�ΡA�|�X�{UI�ե�ϰ�ñN�U�������i�h
    //��ܹ�ܪ���r��
    public TextMeshProUGUI TextLabel;
    //�奻�ե�
    public GameObject TextPanel;
    //�����Y�����
    public UnityEngine.UI.Image FaceImage;
    public Sprite NPCNone;
    public Sprite NPCAngry;
    //����W
    public TextMeshProUGUI Name;
    //��ܦW�٪��ܼ�
    string[] nameset;

    [Header("�}��")]
    AnimatorControll animatorControll;
    DialogueLab dialogueLab;

    [Header("�奻���")]
    //��ܤ奻
    public TextAsset TextFileTeaching;
    [SerializeField] TextAsset CurrentText;
    //�奻���
    [SerializeField]private int index;
    //��r��X�t��
    public float TextSpeed = 0.1f;
    //�ݿ�X����r
    [SerializeField] private List<string> TextList = new List<string>();

    [Header("�奻�P�_")]
    //���O�_��X����
    [SerializeField] private bool TextFinished = false;
    //������X�奻(���L�P�_)
    [SerializeField] private bool CancelText = false;
    ////�i�J���d���`
    //public bool InLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        //�ҥξB�n
        MaskOverLay.SetActive(false);
        InTeaching = false;

        animatorControll = gameObject.GetComponent<AnimatorControll>();
        dialogueLab = gameObject.GetComponent<DialogueLab>();

        //���ä奻��
        TextPanel.SetActive(false);
    }


    private void Update()
    {
        if(dialogueLab.InLevel && !InTeaching)
        {//�p�G��ܵ��� �i�J���d���`�B�|��Ĳ�o�о�
            InTeaching = true;//�ҥΤ޾ɱо�
            dialogueLab.InLevel = false;//�i�J�оǹ�ܩҥH�٨S�}�l���d
            //���wŪ������󬰹���Ǥ奻
            CurrentText = TextFileTeaching;
            //Ū�����
            GetTextFromFile(CurrentText);
            //�}�l���r
            TextFinished = true;
            //�}�l��J��r
            StartCoroutine(SetTextUI());
            string[] nameset = TextList[index].Split('&');
            Name.text = nameset[1].ToString();
            TextPanel.SetActive(true);//�e�ɹ��
        }

        if ((Input.GetKeyDown(KeyCode.Q)) && TextPanel.activeSelf && InTeaching)
        {//�p�G�Q�nĲ�o����~��Q��
            //if (dialogueLab.InLevel)//�p�G�i�J�޾ɨB�J
            //{
            //    TextPanel.SetActive(false);//������ܮ�
            //    MaskOverLay.SetActive(true);//��ܾB�n
            //}
            if(index == TextList.Count)
            {//��󵲧�

            }
            else if (InTeaching && TextFinished)
            {
                StartCoroutine(SetTextUI());
            }
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
            case '5'://�ǥջ�����
                FaceImage.color = new Color(255, 255, 255, 0);
                nameset = TextList[index].Split('&');
                Name.text = nameset[1].ToString();
                index++;
                break;

            case '6'://�ťճB==����޾ɳ���
                FaceImage.sprite = null;
                index = index + 1;
                dialogueLab.InLevel = true;
                break;

            case '8'://NPC�ͮ��
                index = index + 1;
                FaceImage.sprite = NPCAngry;
                FaceImage.color = new Color(255, 255, 255, 255);
                index = index + 1;
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
    public void EnableMask()
    {
        MaskOverLay.SetActive(true);
    }

    public void DisableMask()
    {
        MaskOverLay.SetActive(false);
    }
}
