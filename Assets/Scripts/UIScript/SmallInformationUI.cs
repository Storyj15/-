using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmallInformationUI : MonoBehaviour
{
    [Header("InformationType")]
    public GameObject CardInformation;

    [Header("CardInformationPart")]
    public Image CardIcon1;
    public Image CardIcon2;
    public Sprite[] Icons;
    public TextMeshProUGUI CardUpText;
    public TextMeshProUGUI CardDownText;
    public static int CardType1;
    public static int CardType2;
    public static bool CardUp;

    public static Vector3 UIPos;
    public static bool readCardInformation;
    // Start is called before the first frame update
    void Start()
    {
        CardInformation.SetActive(false);
        CardUp = true;
        readCardInformation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readCardInformation)
        {
            CardInformation.transform.position = UIPos;
            CardInformations();
            CardInformation.SetActive(true);
        }
        if(!readCardInformation)
        {
            CardInformation.SetActive(false);
        }
        if (PlayerUIManager.GetInstance().PlayerData.playerStateMode == NewGameState.NewPlayerStateMode.PlayerDeactivate)
        {
            readCardInformation = false;
        }

    }
    public void CardInformations()
    {
        var playerdata = PlayerUIManager.GetInstance().PlayerData;
        var typevalue = new int[] {1,playerdata.ATKValue,1};
        var typebuff = playerdata.BuffValue;
        if (CardUp)
        {
            CardIcon1.sprite = Icons[CardType1];
            CardIcon2.sprite = Icons[CardType2];
            if (CardType1 == 2)
            {
                CardUpText.text = (typevalue[CardType1]).ToString();
            }
            else
            {
                CardUpText.text = (typevalue[CardType1] + typebuff[CardType1]).ToString();
            }
            if (CardType2 == 2)
            {
                CardDownText.text = (typevalue[CardType2]).ToString();
            }
            else
            {
                CardDownText.text = (typevalue[CardType2] + typebuff[CardType2]).ToString();
            }
            if ((typevalue[CardType1] + typebuff[CardType1]) > typevalue[CardType1])
            {
                CardUpText.color = new Color32(0, 255, 0, 255);
            }
            else if ((typevalue[CardType1] + typebuff[CardType1]) < typevalue[CardType1])
            {
                CardUpText.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                CardUpText.color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            CardIcon1.sprite = Icons[CardType2];
            CardIcon2.sprite = Icons[CardType1];
            CardUpText.text = (typevalue[CardType2] + typebuff[CardType2]).ToString();
            CardDownText.text = (typevalue[CardType1] + typebuff[CardType1]).ToString();
            if (CardType1 == 2)
            {
                CardDownText.text = (typevalue[CardType1]).ToString();
            }
            else
            {
                CardDownText.text = (typevalue[CardType1] + typebuff[CardType1]).ToString();
            }
            if (CardType2 == 2)
            {
                CardUpText.text = (typevalue[CardType2]).ToString();
            }
            else
            {
                CardUpText.text = (typevalue[CardType2] + typebuff[CardType2]).ToString();
            }
            if ((typevalue[CardType2] + typebuff[CardType2]) > typevalue[CardType2])
            {
                CardUpText.color = new Color32(0, 255, 0, 255);
            }
            else if ((typevalue[CardType2] + typebuff[CardType2]) < typevalue[CardType2])
            {
                CardUpText.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                CardUpText.color = new Color32(255, 255, 255, 255);
            }
        }
        Debug.Log("Buff:" + typebuff[0] + "+" + typebuff[1] + "+" + typebuff[2]);
    }
}
