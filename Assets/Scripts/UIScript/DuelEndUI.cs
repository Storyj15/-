using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class DuelEndUI : MonoBehaviour
{
    public GameObject BlackMask;
    [Header("UIs")]
    public GameObject WinUI;
    public GameObject LoseUI;
    public GameObject EvaluateUI;
    public GameObject UpgradeUI;

    [Header("UpgradePart")]
    public TextMeshProUGUI Upgrade1Text;
    public TextMeshProUGUI Upgrade2Text;
    public TextMeshProUGUI Upgrade3Text;
    public GameObject[] InteractableImage;

    [Header("EvaluateUIPart")]
    public ScrollRect scrollRect;
    public GameObject NextLevelButton;
    public TextMeshProUGUI EvaluateText;
    public TextMeshProUGUI NextUpgradeScoreText;

    [Header("WinUIPart")]
    public GameObject WinText;
    public GameObject WinReasonText;
    public GameObject WinButton;

    [Header("LoseUIPart")]
    public GameObject LoseText;
    public GameObject LoseReasonText;
    public GameObject LoseButton;

    public AudioClip[] audioClips;
    private bool canUpgrade;
    private void OnEnable()
    {
        GameManager.gameManager_instance.audioManager.StopClip();
        GameManager.gameManager_instance.GameSpeed = 1;
        Time.timeScale = GameManager.gameManager_instance.GameSpeed;
        canUpgrade = false;
        for (int i = 0; i < InteractableImage.Length; i++)
        {
            if (GameManager.gameManager_instance.PlayerUpgrade[i] == 1)
            {
                InteractableImage[i].SetActive(true);
            }
            else
            {
                InteractableImage[i].SetActive(false);
            }
        }
        StartCoroutine(Ending());
    }
    private void Update()
    {
        if (!canUpgrade && EvaluateUI.activeInHierarchy && GameManager.gameManager_instance.PlayerLevel <= 2)
        {
            NextUpgradeScoreText.text = "距離升級還差 " + GameManager.gameManager_instance.PlayerUpgradeScore.ToString();
        }
        else if (!canUpgrade && EvaluateUI.activeInHierarchy && GameManager.gameManager_instance.PlayerLevel > 2)
        {
            NextUpgradeScoreText.text = "已經無法繼續升級";
        }
        else if (canUpgrade)
        {
            NextUpgradeScoreText.text = "概念升級!!!";
        }
    }
    public void ChooseUpgrade1()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        GameManager.gameManager_instance.PlayerUpgrade[0] = 1;
        canUpgrade = false;
    }
    public void ChooseUpgrade2()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        GameManager.gameManager_instance.PlayerUpgrade[1] = 1;
        canUpgrade = false;
    }
    public void ChooseUpgrade3()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        GameManager.gameManager_instance.PlayerUpgrade[2] = 1;
        canUpgrade = false;
    }
    public void ContinueToEvaluateUI()
    {
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            GameManager.gameManager_instance.isPracticeDuel = false;
            NextLevelBut();
        }
        else
        {
            if (GameManager.gameManager_instance.LevelNumber == 6)
            {
                NextLevelBut();
            }
            else
            {
                NextUpgradeScoreText.text = "距離升級還差 " + GameManager.gameManager_instance.PlayerUpgradeScore.ToString();
                EvaluateUI.SetActive(true);
                EvaluateUI.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetUpdate(true);
                StartCoroutine(EvaluateResult());
            }
        }
    }
    public void NextLevelBut()
    {
        StartCoroutine(NextLevel());
    }
    public void RestartBut()
    {
        StartCoroutine(Restart());
    }
    public void ScrollToDown()
    {
        scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public IEnumerator NextLevel()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        GameManager.gameManager_instance.LevelNumber += 1;
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        if (GameManager.gameManager_instance.LevelNumber == 4)
        {
            LordingUI.NextScene = 5;
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(4);
        }
    }
    public IEnumerator Restart()
    {
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        LordingUI.NextScene = 5;
        SceneManager.LoadScene(2);
    }
    public IEnumerator Ending()
    {
        if (DuelUIManager.isPlayerWin == true)
        {
            yield return new WaitForSeconds(0.5f);
            GameManager.gameManager_instance.audioManager.PlayClip(0, audioClips[0], false);
            yield return new WaitForSeconds(0.5f);
            WinUI.SetActive(true);
            WinText.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(2f);
            var back = WinUI.GetComponent<Image>();
            back.DOFade(1, 1f);
            yield return new WaitUntil(() => { return back.color.a == 1; });
            WinReasonText.GetComponent<TextMeshProUGUI>().text = "擊敗所有敵人";
            WinReasonText.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(0.5f);
            WinButton.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            GameManager.gameManager_instance.audioManager.PlayClip(0, audioClips[1], false);
            yield return new WaitForSeconds(1f);
            LoseUI.SetActive(true);
            LoseText.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(2f);
            var back = LoseUI.GetComponent<Image>();
            back.DOFade(1, 1f);
            yield return new WaitUntil(() => { return back.color.a == 1; });
            if (GameManager.gameManager_instance.isPracticeDuel)
            {
                LoseReasonText.GetComponent<TextMeshProUGUI>().text = "未擊敗鮮血伯爵";
            }
            else
            {
                LoseReasonText.GetComponent<TextMeshProUGUI>().text = "玩家生命值歸0";
            }
            LoseReasonText.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(0.5f);
            LoseButton.SetActive(true);
            Time.timeScale = 0;
        }
        yield return null;
    }
    public IEnumerator EvaluateResult()
    {
        var Evaluate = new List<string>();
        var playerdata = PlayerUIManager.GetInstance().PlayerData;
        var score = new List<int>();
        GameManager.gameManager_instance.audioManager.StopClip();
        GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[4], false);
        yield return new WaitForSecondsRealtime(0.5f);
        Evaluate.Add("橫掃千軍+100");
        score.Add(100);
        if (playerdata.notHurt)
        {
            Evaluate.Add("毫髮無傷+300");
            score.Add(300);
        }
        else if (playerdata.HP > 6 && !playerdata.notHurt)
        {
            Evaluate.Add("輕而易舉+100");
            score.Add(100);
        }
        else if (playerdata.HP < 5 && playerdata.HP > 0)
        {
            Evaluate.Add("極限通關+100");
            score.Add(100);
        }
        if (playerdata.Defense > 0)
        {
            Evaluate.Add("固守陣地+100");
            score.Add(100);
        }
        if (DuelUIManager.RoundInt <= 10)
        {
            if (DuelUIManager.RoundInt <= 5)
            {
                Evaluate.Add("神速通關+300");
                score.Add(300);
            }
            else if (DuelUIManager.RoundInt > 5 && DuelUIManager.RoundInt <= 7)
            {
                Evaluate.Add("高速通關+200");
                score.Add(200);
            }
            else
            {
                Evaluate.Add("迅速通關+100");
                score.Add(100);
            }
        }
        if (DuelUIManager.RoundInt > 20)
        {
            Evaluate.Add("神仙打架+200");
            score.Add(200);
        }
        if (PlayerUIManager.GetInstance().PlayerHandCardZone.transform.childCount > 1)
        {
            Evaluate.Add("運籌帷幄+100");
            score.Add(100);
        }
        for (int i = 0; i < Evaluate.Count; i++)
        {
            EvaluateText.text += Evaluate[i] + "\n";
            ScrollToDown();
            GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[2], false);
            yield return StartCoroutine(EvaluateScoreChange(score[i]));
            yield return new WaitForSecondsRealtime(0.5f);
        }
        NextLevelButton.SetActive(true);
    }
    public IEnumerator EvaluateScoreChange(int score)
    {
        for (int i = 0; i < (score);i++)
        {
            GameManager.gameManager_instance.PlayerUpgradeScore -= 1;
            if (GameManager.gameManager_instance.PlayerUpgradeScore == 0)
            {
                GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[3], false);
                canUpgrade = true;
                GameManager.gameManager_instance.PlayerLevel += 1;
                yield return new WaitForSecondsRealtime(0.3f);
                if (GameManager.gameManager_instance.PlayerLevel == 1)
                {
                    GameManager.gameManager_instance.PlayerUpgradeScore = 1000;
                }
                else if (GameManager.gameManager_instance.PlayerLevel == 2)
                {
                    GameManager.gameManager_instance.PlayerUpgradeScore = 2000;
                }
                yield return new WaitForSecondsRealtime(0.5f);
                yield return StartCoroutine(ChooseUpgrade());
                yield return new WaitForSecondsRealtime(0.5f);
                if (i < score - 1)
                {
                    GameManager.gameManager_instance.audioManager.PlayClip(1, audioClips[2], false);
                }
            }
            yield return new WaitForSecondsRealtime(1 / score);
        }
        yield return null;
    }
    public IEnumerator ChooseUpgrade()
    {
        Upgrade1Text.text = PlayerUIManager.GetInstance().PlayerData.Upgrage1Text;
        Upgrade2Text.text = PlayerUIManager.GetInstance().PlayerData.Upgrage2Text;
        Upgrade3Text.text = PlayerUIManager.GetInstance().PlayerData.Upgrage3Text;
        UpgradeUI.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetUpdate(true);
        yield return new WaitUntil(() => { return canUpgrade == false; });
        UpgradeUI.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetUpdate(true);
    }
}
