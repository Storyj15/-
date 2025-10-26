using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public GameObject MenuContainer;
    public GameObject ContinueDisable;
    public GameObject BlackMask;
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("AudioCLip")]
    public AudioClip[] MenuClips;

    public Button ContinueGameButton;
    //////
    public void Start()
    {
        GameManager.gameManager_instance.Save();
        volumeSlider.value = GameManager.defaulVolume;
        volumeTextValue.text = GameManager.defaulVolume.ToString("0" + "%");
        if (GameManager.gameManager_instance.LevelNumber != 0)
        {
            ContinueGameButton.interactable = true;
            ContinueDisable.SetActive(false);
        }
        else
        {
            ContinueGameButton.interactable = false;
            ContinueDisable.SetActive(true);
        }
        GameManager.gameManager_instance.audioManager.PlayClip(0,MenuClips[0],true);
        StartCoroutine(EnterMenu());
    }
    public void ButtonSound()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, MenuClips[1], false);
    }
    //////
    public void NewGameDialogYes()
    {
        GameManager.gameManager_instance.StartNewGame();
        StartCoroutine(NewGame());
    }

    public void LoadGameDialogYes()
    {
        StartCoroutine(LoadDuel());
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        GameManager.defaulVolume = volume;
        AudioListener.volume = GameManager.defaulVolume;
        volumeTextValue.text = GameManager.defaulVolume.ToString("0" + "%");
    }
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = 0.5f;
            volumeSlider.value = 0.5f;
            GameManager.defaulVolume = 0.5f;
            volumeTextValue.text = GameManager.defaulVolume.ToString("0" + "%");
            //volumeApply();
        }
    }
    public IEnumerator NewGame()
    {
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(3);
        yield return null;
    }
    public IEnumerator LoadDuel()
    {
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        if (GameManager.gameManager_instance.LevelNumber == 4)
        {
            LordingUI.NextScene = 5;
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(4);
        }
        yield return null;
    }
    public IEnumerator EnterMenu()
    {
        MenuContainer.transform.DOScale(new Vector3(1,1,1),0.5f);
        yield return null;
    }
}
