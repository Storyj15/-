using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FirstDialogueController : MonoBehaviour
{
    public AudioClip FirstDialogueClips;
    public GameObject ChoosePracticeUI;
    public GameObject SkipButton;
    public GameObject BlackMask;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager_instance.Save();
        GameManager.gameManager_instance.audioManager.PlayClip(0, FirstDialogueClips, true);
        Time.timeScale = 1;
        DialogueUIController.textAssetIndex = 0;
        SkipButton.SetActive(false);
        ChoosePracticeUI.SetActive(false);
        Invoke(nameof(StartFirstDialogue),1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (DialogueUIController.DialogueEnd)
        {
            DialogueUIController.DialogueEnd = false;
            ChoosePracticeUI.SetActive(true);
            SkipButton.SetActive(false);
        }
    }
    private void StartFirstDialogue()
    {
        DialogueUIController.openDialogueUI = true;
        SkipButton.SetActive(true);
    }
    public void StartPracticeDuel()
    {
        StartCoroutine(GoPractice());
    }
    public void StartNormalDuel()
    {
        StartCoroutine(GoNormal());
    }
    public IEnumerator GoPractice()
    {
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager_instance.LevelNumber = 0;
        LordingUI.NextScene = 5;
        GameManager.gameManager_instance.isPracticeDuel = true;
        SceneManager.LoadScene(2);
    }
    public IEnumerator GoNormal()
    {
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager_instance.LevelNumber = 1;
        GameManager.gameManager_instance.isPracticeDuel = false;
        SceneManager.LoadScene(4);
    }
}
