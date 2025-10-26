using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DialogueController : MonoBehaviour
{
    public AudioClip DialogueClip;
    public GameObject SkipButton;
    public GameObject BlackMask;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager_instance.Save();
        GameManager.gameManager_instance.audioManager.PlayClip(0, DialogueClip, true);
        Time.timeScale = 1;
        DialogueUIController.textAssetIndex = (GameManager.gameManager_instance.LevelNumber - 1);
        SkipButton.SetActive(false);
        SkipButton.SetActive(false);
        Invoke(nameof(StartDialogue), 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (DialogueUIController.DialogueEnd)
        {
            SkipButton.SetActive(false);
            DialogueUIController.DialogueEnd = false;
            StartCoroutine(GoNext());
        }
    }
    private void StartDialogue()
    {
        DialogueUIController.openDialogueUI = true;
        SkipButton.SetActive(true);
    }
    public IEnumerator GoNext()
    {
        if (GameManager.gameManager_instance.LevelNumber == 7)
        {
            BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(0.5f);
            SkipButton.SetActive(false);
            SceneManager.LoadScene(6);
        }
        else
        {
            BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(0.5f);
            SkipButton.SetActive(false);
            LordingUI.NextScene = 5;
            SceneManager.LoadScene(2);
        }
    }
}
