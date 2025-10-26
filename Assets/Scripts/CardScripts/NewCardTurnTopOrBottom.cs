using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardTopOrBottomState
{ 
    Top,
    Bottom
}
public class NewCardTurnTopOrBottom : MonoBehaviour
{
    public CardTopOrBottomState cardTopOrBottomState;

    public GameObject CardTop;
    public GameObject CardBottom;
    private float ChangeTime = 0.3f;

    private bool isChangeTopOrBottom; //是否正在翻轉中
    public static bool istheChangeUpOrDown;

    public void CardInit()
    {
        if (cardTopOrBottomState == CardTopOrBottomState.Top)
        {
            CardTop.transform.eulerAngles = new Vector3(0, 0, 0);
            CardBottom.transform.eulerAngles = new Vector3(0, 90, 0);
            CardTop.SetActive(true);
            CardBottom.SetActive(false);
        }
        else
        {
            CardTop.transform.eulerAngles = new Vector3(0, 90, 0);
            CardBottom.transform.eulerAngles = new Vector3(0, 0, 0);
            CardTop.SetActive(false);
            CardBottom.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cardTopOrBottomState = CardTopOrBottomState.Bottom;
        isChangeTopOrBottom = false;
        istheChangeUpOrDown = false;
        CardInit();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CardStartTop();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CardStartBottom();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CardStartUp();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            CardStartDown();
        }
    }
    public void CardStartTop()
    {
        if (isChangeTopOrBottom)
        {
            return;
        }
        StartCoroutine(ToTop());
    }
    public void CardStartBottom()
    {
        if (isChangeTopOrBottom)
        {
            return;
        }
        StartCoroutine(ToBottom());
    }
    public void CardStartUp()
    {
        if (istheChangeUpOrDown)
        {
            return;
        }
        StartCoroutine(ToUp());
    }
    public void CardStartDown()
    {
        if (istheChangeUpOrDown)
        {
            return;
        }
        StartCoroutine(ToDown());
    }
    IEnumerator ToTop()
    {
        isChangeTopOrBottom = true;
        CardBottom.transform.DORotate(new Vector3(0, 90, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        CardTop.SetActive(true);
        CardTop.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
        cardTopOrBottomState = CardTopOrBottomState.Top;
        CardBottom.SetActive(false);
        isChangeTopOrBottom = false;
    }
    IEnumerator ToBottom()
    {
        isChangeTopOrBottom = true;
        CardTop.transform.DORotate(new Vector3(0, 90, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        CardBottom.SetActive(true);
        CardBottom.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
        cardTopOrBottomState = CardTopOrBottomState.Bottom;
        CardTop.SetActive(false);
        isChangeTopOrBottom = false;
    }
    IEnumerator ToUp()
    {
        istheChangeUpOrDown = true;
        gameObject.GetComponent<NewCardValueManager>().changeCardUpOrDown = false;
        SmallInformationUI.CardUp = true;
        CardTop.transform.DORotate(new Vector3(0, 0, 360), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        istheChangeUpOrDown = false;
        gameObject.GetComponent<NewCardValueManager>().changeCardUpOrDown = true;
    }
    IEnumerator ToDown()
    {
        istheChangeUpOrDown = true;
        gameObject.GetComponent<NewCardValueManager>().changeCardUpOrDown = false;
        SmallInformationUI.CardUp = false;
        CardTop.transform.DORotate(new Vector3(0, 0, 180), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        istheChangeUpOrDown = false;
        gameObject.GetComponent<NewCardValueManager>().changeCardUpOrDown = true;
    }
}
