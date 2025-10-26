using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class dialog : MonoBehaviour,IPointerDownHandler
{
    public AudioClip SentenceChangeClip; //切換句子音效

    [Header("TextAssetPart")]
    public TextAsset dialogDataFile;
    public Sprite[] dialogueImageList;
    public Sprite[] dialogueBackList;
    private bool isTextFinished;
    private bool PassText;
    private bool isEvent;
    private List<string> SentenceList;
    public  int dialogIndex;
    private string SentenceText;

    public GameObject CharacterLeft;
    public GameObject CharacterRight;
    public GameObject DialogueBack;
    public GameObject NextText;
    public GameObject DialogObject;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public GameObject DialogueImage;

    public List<Sprite> Charactersprites = new List<Sprite>();
    public Dictionary<string, Sprite> CharacterImageDic = new Dictionary<string, Sprite>();
    private void Awake()
    {
        CharacterImageDic.Add("腦中的聲音", Charactersprites[0]);
        CharacterImageDic.Add("???", Charactersprites[0]);
        CharacterImageDic.Add("我",Charactersprites[1]);
        CharacterImageDic.Add("鮮血伯爵", Charactersprites[2]);
        CharacterImageDic.Add("懷斯", Charactersprites[3]);
        CharacterImageDic.Add("艾比", Charactersprites[4]);
        CharacterImageDic.Add("Doll",Charactersprites[5]);
    }
    private void OnEnable()
    {
        SentenceList = new List<string>();
        DialogueBack.SetActive(false);
        NextText.SetActive(false);
        isTextFinished = true;
        isEvent = false;
        ReadText(dialogDataFile);
        ShowDialogrow();
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //開始下一段對話/快速跑完該對話
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (isTextFinished && !PassText)
            {
                NextText.SetActive(false);
                ShowDialogrow();
            }
            else if (!isTextFinished && !PassText)
            {
                PassText = true;
            }
        }
    }
    public void UpdateText(string _name , string _text)
    {
        nameText.text = _name;
        SentenceText = _text;
    }
    public void UpdateImage(string _name, string _position)
    {
        if(_position == "左") 
        {
            CharacterLeft.GetComponent<Image>().sprite = CharacterImageDic[_name];
            CharacterLeft.SetActive(true);
            CharacterRight.SetActive(false);
        }
        else if(_position == "右")
        {
            CharacterRight.GetComponent<Image>().sprite = CharacterImageDic[_name];
            CharacterLeft.SetActive(false);
            CharacterRight.SetActive(true);
        }
        else if (_position == "左右")
        {
            CharacterLeft.GetComponent<Image>().sprite = CharacterImageDic[_name];
            CharacterLeft.SetActive(true);
            CharacterRight.SetActive(true);
        }
    }
    public void ReadText(TextAsset _textAsset)
    {
        var dialogRows = _textAsset.text.Split('\n');
        foreach (var Line in dialogRows)
        {
            SentenceList.Add(Line);
        }
        Debug.Log("SentenceText Add Complete");
    }
    public void ShowDialogrow()
    {
        foreach(var row in SentenceList)
        {
            string[] cell = row.Split(',');
            if (cell[0] == "#" && int.Parse(cell[1]) == dialogIndex)
            {
                dialogIndex = int.Parse(cell[5]);
                UpdateImage(cell[2], cell[3]);
                UpdateText(cell[2], cell[4]);
                break;
            }
        }
        StartCoroutine(ReadDialogueText());
    }
    public void OnClickNext()
    {
        ShowDialogrow();
        Debug.Log("55");
    }
    private IEnumerator ReadDialogueText()
    {
        isTextFinished = false;
        dialogText.text = "";
        switch (SentenceText)
        {
            case "Img1":
                DialogueImage.GetComponent<Image>().sprite = dialogueImageList[0];
                DialogueImage.SetActive(true);
                isEvent = true;
                break;
            case "Img2":
                DialogueImage.GetComponent<Image>().sprite = dialogueImageList[1];
                DialogueImage.SetActive(true);
                isEvent = true;
                break;
            case "Img3":
                DialogueImage.GetComponent<Image>().sprite = dialogueImageList[2];
                DialogueImage.SetActive(true);
                isEvent = true;
                break;
            case "ImgClose":
                DialogueImage.SetActive(false);
                isEvent = true;
                break;
            case "End":
                DialogueUIController.DialogueEnd = true;
                gameObject.SetActive(false);
                break;
            case "Pause":
                gameObject.SetActive(false);
                GameManager.gameManager_instance.nextState = true;
                break;
            case "ShowEnemy":
                CharacterRight.GetComponent<Image>().sprite = Charactersprites[3];
                CharacterRight.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                CharacterRight.GetComponent<Image>().DOFade(1, 1);
                yield return new WaitForSeconds(1f);
                isEvent = true;
                break;
            case "Next":
                isEvent = true;
                break;
        }
        if (isEvent)
        {
            isEvent = false;
            ShowDialogrow();
            yield break;
        }
        int letter = 0;
        GameManager.gameManager_instance.audioManager.PlayClip(1, SentenceChangeClip, false);
        while (!PassText && letter < SentenceText.Length - 1)
        {
            dialogText.text += SentenceText[letter];
            letter++;
            yield return new WaitForSeconds(0.08f);
        }
        dialogText.text = SentenceText;
        NextText.SetActive(true);
        PassText = false;
        isTextFinished = true;
    }
}
