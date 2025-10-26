using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InformationManager : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [Header("InformationType")]
    public GameObject CardInformation;
    public GameObject PlayerStateInformation;

    [Header("InformationPart")]
    public TextMeshProUGUI CardValue1Value;
    public TextMeshProUGUI CardValue2Value;
    public Image CardIcon1;
    public Image CardIcon2;
    public Sprite[] Icons;
    //public static int[] AttackZone;

    [Header("InformationExplain")]
    //public GameObject[] Explain1 = new GameObject[2];
    public GameObject Explain1TargetLocation;
    public TextMeshProUGUI Explain1TypeText;
    public Image Explain1Icon;
    //public GameObject[] Explain1ATKZone;
    public Image Explain2Icon;
    public TextMeshProUGUI Explain2TypeText;

    public static int Information1Value;
    public static int Information2Value;
    public static int Information1Type;
    public static int Information2Type;

    public static bool isopenInformationUI;
    public static bool readInformation;
    public static bool isCardInformation;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-300, 0);
        readInformation = false;
        isopenInformationUI = false;
        isCardInformation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCardInformation)
        {
            CardInformation.SetActive(true);
            PlayerStateInformation.SetActive(false);
        }
        if (!isCardInformation)
        {
            CardInformation.SetActive(false);
            PlayerStateInformation.SetActive(true);
        }
        if (readInformation && rectTransform.anchoredPosition.x != 300)
        {
            rectTransform.anchoredPosition = new Vector2(300, 0);
        }
        if (!readInformation && rectTransform.anchoredPosition.x != -300)
        {
            rectTransform.anchoredPosition = new Vector2(-300, 0);
        }
        if (PlayerUIManager.GetInstance().PlayerData.playerStateMode == NewGameState.NewPlayerStateMode.PlayerDeactivate)
        {
            isopenInformationUI = false;
            readInformation = false;
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (isopenInformationUI)
            {
                readInformation = true;
            }
            else
            {
                readInformation = false;
            }
        }
        Informations();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isopenInformationUI = true;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isopenInformationUI = false;
    }
    public void Informations()
    {
        CardValue1Value.text = Information1Value.ToString();
        CardValue2Value.text = Information2Value.ToString();
        CardIcon1.sprite = Icons[Information1Type];
        CardIcon2.sprite = Icons[Information2Type];
        Explain1TypeText.text = "Draw:" + Information1Value;
        if (Information2Type == 0)
        {
            Explain2TypeText.text = "Move:" + Information2Value;
        }
        else if (Information2Type == 3)
        {
            Explain2TypeText.text = "Star:" + Information2Value;
        }
        else if (Information2Type == 4)
        {
            Explain2TypeText.text = "Draw:" + Information2Value;
        }
        ATKZoneInformation();
    }
    public void ATKZoneInformation()
    {
        /*for (int a = 0; a < Explain1ATKZone.Length; a++)
        {
            Explain1ATKZone[a].gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 100);
        }*/

        /*if (Information1Type == 1 || Information1Type == 2)
        {
            if (Information1Type == 1)
            {
                Explain1TargetLocation.SetActive(true);
            }
            else
            {
                Explain1TargetLocation.SetActive(false);
            }
            Explain1Icon.sprite = CardIcon1.sprite;
            for (int i = 0; i < AttackZone.Length; i++)
            {
                if (Information1Type == 1)
                {
                    Explain1ATKZone[2 + AttackZone[i]].gameObject.GetComponent<Image>().color = new Color32(180, 18, 18, 200);
                }
                else
                {
                    Explain1ATKZone[AttackZone[i]].gameObject.GetComponent<Image>().color = new Color32(18, 100, 180, 200);
                }
                Explain1[0].SetActive(false);
                Explain1[1].SetActive(true);
            }
        }
        else
        {
            Explain1[0].SetActive(true);
            Explain1[1].SetActive(false);
        }*/
    }
}
