using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LordingUI : MonoBehaviour
{
    public static int NextScene; //�U�ӳ����s��
    private bool Cannextscene; //�O�_�i�i�J�U�ӳ���
    private bool UseCheatTime; //�@���X��J�ɶ�
    private AsyncOperation async; //���BŪ������

    public GameObject BlackMask;
    public GameObject NextLevel;
    public GameObject NextLevelName;
    public GameObject LordingCircle;
    private float LordingTimer; //�p�ɾ�

    public Texture2D[] lordingbacks;
    public RawImage LordingBackGround;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager_instance.audioManager.StopClip();
        Time.timeScale = 1;
        LordingCircle.SetActive(false);
        UseCheatTime = false;
        Cannextscene = false;
        LordingTimer = 0;
        NextLevel.SetActive(false);
        NextLevelName.SetActive(false);
        LordingBackChange();
        NextLevelText();
        NextLevelNameText();
        StartCoroutine(LordScene());
    }

    // Update is called once per frame
    void Update()
    {
        LordingTimer += Time.deltaTime;
        if (/*LordingTimer >= 0.5f &&*/ Cannextscene == false)
        {
            LordingCircle.SetActive(true);
            if (NextScene != 1)
            {
                NextLevel.SetActive(true);
                NextLevelName.SetActive(true);
            }
        }
        if (LordingTimer >= 2 && !UseCheatTime)
        {
            Cannextscene = true;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UseCheatTime = !UseCheatTime;
        }
    }
    private void LordingBackChange()
    {
        if (GameManager.gameManager_instance.LevelNumber != 6)
        {
            LordingBackGround.texture = lordingbacks[0];
        }
        else
        {
            LordingBackGround.texture = lordingbacks[1];
        }
    }
    private void NextLevelText()
    {
        if (GameManager.gameManager_instance.LevelNumber == 0)
        {
            GameManager.gameManager_instance.audioInt = 0;
            NextLevel.GetComponent<TextMeshProUGUI>().text = "�о���";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 5)
        {
            GameManager.gameManager_instance.audioInt = 2;
            NextLevel.GetComponent<TextMeshProUGUI>().text = "��" + GameManager.gameManager_instance.LevelNumber.ToString() + "��";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 6)
        {
            GameManager.gameManager_instance.audioInt = 3;
            NextLevel.GetComponent<TextMeshProUGUI>().text = "�̲���";
        }
        else
        {
            GameManager.gameManager_instance.audioInt = 1;
            NextLevel.GetComponent<TextMeshProUGUI>().text = "��" + GameManager.gameManager_instance.LevelNumber.ToString() + "��";
        }
    }
    private void NextLevelNameText()
    {
        if (GameManager.gameManager_instance.LevelNumber == 0)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "����";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 1)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "ı��";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 2)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "�θs";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 3)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "�H��";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 4)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "�]��";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 5)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "����";
        }
        else if (GameManager.gameManager_instance.LevelNumber == 6)
        {
            NextLevelName.GetComponent<TextMeshProUGUI>().text = "���㫰�����D";
        }
    }
    public IEnumerator LordScene()
    {
        BlackMask.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        async = SceneManager.LoadSceneAsync(NextScene);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (Cannextscene == true)
            {
                BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                yield return new WaitForSeconds(0.5f);
                LordingCircle.SetActive(false);
                NextLevel.SetActive(false);
                NextLevelName.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
