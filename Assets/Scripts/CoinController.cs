using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinController : MonoBehaviour
{
    public static bool isCoinTop;

    public GameObject CoinTop;
    public GameObject CoinBottom;
    
    private float ChangeTime = 0.1f;

    private bool CoinMove = false;

    public void CoinInit()
    {
        CoinMove = false;
        CoinTop.transform.eulerAngles = new Vector3(0, 0, 0);
        CoinBottom.transform.eulerAngles = new Vector3(0, 90, 0);
        CoinTop.SetActive(true);
        CoinBottom.SetActive(false);
    }
    private void OnEnable()
    {
        CoinInit();
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            CoinInit();
            isCoinTop = false;
            StartCoroutine(CoinTopOrBottom());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CoinInit();
            isCoinTop = true;
            StartCoroutine(CoinTopOrBottom());
        }*/
        if ((gameObject.transform.localPosition.y != 350) && CoinMove)
        {
            CoinMove = false;
            gameObject.transform.DOLocalMoveY(350, 0.8f).SetEase(Ease.InSine);
        }
        if (gameObject.transform.localPosition.y == 350)
        {
            gameObject.transform.DOLocalMoveY(-250, 0.8f).SetEase(Ease.InSine);
        }
    }
    public IEnumerator CoinTopOrBottom()
    {
        CoinTop.transform.DORotate(new Vector3(0, 90, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }
        CoinBottom.SetActive(true);
        CoinBottom.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
        CoinTop.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        CoinBottom.transform.DORotate(new Vector3(0, 90, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }
        CoinTop.SetActive(true);
        CoinTop.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
        CoinBottom.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        CoinTop.transform.DORotate(new Vector3(-60, 0, 0), ChangeTime);
        CoinBottom.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        CoinTop.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
        for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }
        yield return null;
        CoinMove = true;
        if (isCoinTop)
        {
            for (int a = 0; a < 5; a++)
            {
                CoinTop.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
                for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
                {
                    yield return null;
                }
                CoinBottom.SetActive(true);
                CoinBottom.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
                CoinTop.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                CoinBottom.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
                for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
                {
                    yield return null;
                }
                CoinTop.SetActive(true);
                CoinTop.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
                CoinBottom.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int a = 0; a < 4; a++)
            {
                CoinTop.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
                for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
                {
                    yield return null;
                }
                CoinBottom.SetActive(true);
                CoinBottom.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
                CoinTop.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                CoinBottom.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
                for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
                {
                    yield return null;
                }
                CoinTop.SetActive(true);
                CoinTop.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
                CoinBottom.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
            CoinTop.transform.DORotate(new Vector3(90, 0, 0), ChangeTime);
            for (float i = ChangeTime; i > 0; i -= Time.deltaTime)
            {
                yield return null;
            }
            CoinBottom.SetActive(true);
            CoinBottom.transform.DORotate(new Vector3(0, 0, 0), ChangeTime);
            CoinTop.SetActive(false);
        }
        yield return null;
    }
}
