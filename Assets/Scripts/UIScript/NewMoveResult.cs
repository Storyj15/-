using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class NewMoveResult : MonoBehaviour,IPointerDownHandler
{
    public AudioClip[] audioClips;
    private bool GamePause;
    private bool CoinPlay;
    public GameObject CoinAniBack;
    public GameObject CoinEndEffect;
    public GameObject Coin;
    public VideoClip[] CoinAniClip;
    private int CoinValue;
    // Start is called before the first frame update
    private void OnEnable()
    {
        CoinAniBack.SetActive(false);
        CoinEndEffect.SetActive(false);
        GamePause = false;
        CoinPlay = false;
        Coin.SetActive(false);
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            CoinValue = 1;
        }
        else if (!GameManager.gameManager_instance.isPracticeDuel)
        {
            CoinValue = Random.Range(0, 2);
        }
    }
    private void Update()
    {
        Coin.GetComponent<VideoPlayer>().playbackSpeed = GameManager.gameManager_instance.GameSpeed;
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (Input.GetMouseButtonDown(0) && CoinPlay && Coin.GetComponent<VideoPlayer>().time < (int)(Coin.GetComponent<VideoPlayer>().frameCount / Coin.GetComponent<VideoPlayer>().frameRate + 0.5f))
        {
            CoinPlay = false;
            Coin.GetComponent<VideoPlayer>().time = (int)(Coin.GetComponent<VideoPlayer>().frameCount / Coin.GetComponent<VideoPlayer>().frameRate + 0.5f);
            Coin.GetComponent<VideoPlayer>().Play();
            CoinEndEffect.SetActive(true);
        }
    }
    public IEnumerator MoveResult()
    {
        DuelUIManager.Information = "移動中";
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[0], false);
        yield return new WaitForSeconds(0.4f);
        PlayerUIManager.GetInstance().MovePiece();
        yield return new WaitForSeconds(0.8f);
        DuelUIManager.Information = "決定先後攻";
        CoinPlay = true;
        CoinAniBack.SetActive(true);
        if (CoinValue == 0)
        {
            yield return StartCoroutine(PlayCoinAni());
        }
        else
        {
            yield return StartCoroutine(PlayCoinAni());
        }
        yield return new WaitForSeconds(0.5f);
        if (CoinValue == 0)
        {
            PlayerUIManager.GetInstance().isFirstATK = true;
            DuelUIManager.Information = "先攻";
        }
        else
        {
            PlayerUIManager.GetInstance().isFirstATK = false;
            DuelUIManager.Information = "後攻";
        }
        yield return new WaitForSeconds(1f);
        CoinPlay = false;
        Coin.SetActive(false);
        CoinAniBack.SetActive(false);
        DuelUIManager.showInformationText = false;
        yield return null;
    }
    public IEnumerator PlayCoinAni()
    {
        Coin.SetActive(true);
        Coin.GetComponent<VideoPlayer>().clip = CoinAniClip[CoinValue];
        //var videotime = (int)(Coin.GetComponent<VideoPlayer>().frameCount / Coin.GetComponent<VideoPlayer>().frameRate + 0.5f);
        Coin.GetComponent<VideoPlayer>().Play();
        yield return new WaitUntil(() => { return !Coin.GetComponent<VideoPlayer>().isPlaying; });
        CoinEndEffect.SetActive(true);
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[1], false);
    }
}
