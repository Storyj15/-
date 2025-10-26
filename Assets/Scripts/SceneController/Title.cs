using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Title : MonoBehaviour
{
    public AudioClip menuButton;
    private AsyncOperation async;
    private bool pressAnyKey;
    public GameObject GameTitleText;
    public TextMeshProUGUI PressAnyKeyText;
    public GameObject LordingCircle;
    // Start is called before the first frame update
    void Start()
    {
        LordingCircle.SetActive(false);
        pressAnyKey = true;
        InvokeRepeating(nameof(ShowText),0.5f,1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && pressAnyKey)
        {
            pressAnyKey = false;
            GameManager.gameManager_instance.audioManager.PlayClip(1,menuButton,false);
            EnterMenu();
        }
    }
    private void ShowText()
    {
        if (PressAnyKeyText.text == "")
        {
            PressAnyKeyText.text = "ÂIÀ»·Æ¹«¥ªÁäÄ~Äò";
        }
        else
        {
            PressAnyKeyText.text = "";
        }
    }
    private void EnterMenu()
    {
        StartCoroutine(LordScene());
    }
    public IEnumerator LordScene()
    {
        GameTitleText.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        CancelInvoke(nameof(ShowText));
        PressAnyKeyText.text = "";
        LordingCircle.SetActive(true);
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(0.5f);
        while (!async.isDone)
        {
            async.allowSceneActivation = true;
            yield return null;
        }
    }
}
