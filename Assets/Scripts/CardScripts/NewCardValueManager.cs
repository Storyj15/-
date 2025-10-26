using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class NewCardValueManager : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{ 
    [Header("CardPart")]
    public GameObject CardTop;
    public GameObject CardBottom;
    public GameObject CardResult;
    public Image Type1Image;
    public Image Type2Image;
    public TextMeshProUGUI CardUpValueText;
    public TextMeshProUGUI CardDownValueText;
    public Image ResultImage;
    public Image InteractableImage;

    [Header("Card")]
    public Sprite[] Icon;
    public AudioClip[] audioClip;

    [Header("MainValue")]
    public int MainType;

    private int Type1;
    private int Type2;
    private int[] weight = new int[3] { 20, 60, 20 }; //權重各機率

    [Header("CardState")]
    public bool isCardUp; //卡片是否正位置
    public bool usecard; //選擇使用該卡片
    public bool resultCard;
    public bool changeCardUpOrDown;
    public PlayerDataManager playerData;
    // Start is called before the first frame update
    void Start()
    {
        InteractableImage.enabled = false;
        CardResult.SetActive(false);
        usecard = false;
        isCardUp = true;
        resultCard = false;
        changeCardUpOrDown = true;
        CardValueSetting();
    }

    // Update is called once per frame
    void Update()
    {
        playerData = PlayerUIManager.GetInstance().PlayerData;
        var typevalue = new int[] { 1, playerData.ATKValue, 1 };
        if (Type1 == 2)
        {
            CardUpValueText.text = typevalue[Type1].ToString();
        }
        else
        {
            CardUpValueText.text = (typevalue[Type1] + playerData.BuffValue[Type1]).ToString();
        }
        if (Type2 == 2)
        {
            CardDownValueText.text = typevalue[Type2].ToString();
        }
        else
        {
            CardDownValueText.text = (typevalue[Type2] + playerData.BuffValue[Type2]).ToString();
        }
        if (resultCard)
        {
            ResultImage.sprite = Icon[MainType];
            CardResult.SetActive(true);
            CardTop.SetActive(false);
            CardBottom.SetActive(false);
        }
        if ((DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Move) || (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.MoveResult))
        {
            InteractableImage.enabled = true;
        }
        else
        {
            InteractableImage.enabled = false;
        }
    }
    private int GetTypeWeight(int[] array, int Totalweight) //機率權重
    {
        int rand = Random.Range(1, Totalweight + 1);
        int tmp = 0;
        for (int w = 0; w < array.Length; w++)
        {
            tmp += array[w];
            if (rand < tmp)
            {
                return w;
            }
        }
        return 0;
    }

    private void CardValueSetting()
    {
        playerData = PlayerUIManager.GetInstance().PlayerData;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            Type1 = PlayerUIManager.GetInstance().PracticeType1;
            Type2 = PlayerUIManager.GetInstance().PracticeType2;
            Type1Image.sprite = Icon[Type1];
            Type2Image.sprite = Icon[Type2];
        }
        else if (!GameManager.gameManager_instance.isPracticeDuel)
        {
            Type1 = GetTypeWeight(weight, 100);
            Type2 = GetTypeWeight(weight, 100);
            Type1Image.sprite = Icon[Type1];
            Type2Image.sprite = Icon[Type2];
        }
        ChangeCardValue();
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if ((playerData.playerStateMode == NewGameState.NewPlayerStateMode.PlayerActivate) && (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack))
        {
            //滑鼠左鍵卡片時
            if (pointerEventData.button == PointerEventData.InputButton.Left && !NewCardTurnTopOrBottom.istheChangeUpOrDown)
            {
                GameManager.gameManager_instance.audioManager.PlayClip(1, audioClip[0], false);
                ChooseUseCard();
            }
            if (pointerEventData.button == PointerEventData.InputButton.Right && changeCardUpOrDown) //滑鼠右鍵卡片時卡片翻轉+更換資料
            {
                GameManager.gameManager_instance.audioManager.PlayClip(1, audioClip[1], false);
                if (isCardUp == true)
                {
                    isCardUp = false;
                    ChangeCardValue();
                    ShowATKZone();
                    gameObject.GetComponent<NewCardTurnTopOrBottom>().CardStartDown();

                }
                else
                {
                    isCardUp = true;
                    ChangeCardValue();
                    ShowATKZone();
                    gameObject.GetComponent<NewCardTurnTopOrBottom>().CardStartUp();
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //跳出效果UI
        if ((playerData.playerStateMode == NewGameState.NewPlayerStateMode.PlayerActivate) && (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack))
        {
            ShowATKZone();
            StartCoroutine(OpenCardInformation());
            transform.DOScale(1.3f, 0.2f);
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //關閉效果UI
        SmallInformationUI.readCardInformation = false;
        LocationManager.showPlayerATKZone = false;
        transform.DOScale(1, 0.2f);
    }
    public void ChooseUseCard()
    {
        usecard = !usecard;
        UseCard();
    }
    private void UseCard() //卡片被使用時
    {
        if (usecard)
        {
            transform.position += new Vector3(0, 50, 0);
            SmallInformationUI.UIPos = new Vector3(transform.position.x, transform.position.y, 0);
            changeCardUpOrDown = false;
        }
        else
        {
            transform.position -= new Vector3(0, 50, 0);
            SmallInformationUI.UIPos = new Vector3(transform.position.x, transform.position.y, 0);
            changeCardUpOrDown = true;
        }
    }
    public void ShowATKZone()
    {
        LocationManager.ActionType = gameObject.GetComponent<NewCardValueManager>().MainType;
        if (MainType == 1)
        {
            LocationManager.showPlayerATKZone = true;
        }
        else
        {
            LocationManager.showPlayerATKZone = false;
        }
    }
    public void ChangeCardValue()
    {
        if (isCardUp)
        {
            MainType = Type1;
        }
        else
        {
            MainType = Type2;
        }
    }
    public IEnumerator OpenCardInformation()
    {
        SmallInformationUI.CardType1 = Type1;
        SmallInformationUI.CardType2 = Type2;
        SmallInformationUI.CardUp = isCardUp;
        SmallInformationUI.UIPos = new Vector3(transform.position.x, transform.position.y, 0);
        SmallInformationUI.readCardInformation = true;
        yield return null;
    }
}
