using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private bool showskip;
    public GameObject SkipPanel;
    public GameObject BlackMask;
    public TextAsset EndTextFile;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI Text;

    private List<string> SentenceList = new List<string>();
    private List<string[]> TextList = new List<string[]>();
    // Start is called before the first frame update
    void Start()
    {
        showskip = false;
        SkipPanel.SetActive(false);
        GameManager.gameManager_instance.LevelNumber = 0;
        GameManager.gameManager_instance.PlayerUpgrade = new int[3] { 0, 0, 0 };
        GameManager.gameManager_instance.PlayerUpgradeScore = 500;
        GameManager.gameManager_instance.PlayerLevel = 0;
        ReadText(EndTextFile);
        ReadyText();
        StartCoroutine(EndingList());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !showskip)
        {
            showskip = true;
            SkipPanel.SetActive(true);
        }
    }
    public void ReadText(TextAsset _textAsset)
    {
        var dialogRows = _textAsset.text.Split('\n');
        var List = new List<string>();
        foreach (var Line in dialogRows)
        {
            List.Add(Line);
        }
        SentenceList = List;
        Debug.Log("Ending SentenceText Add Complete");
    }
    public void ReadyText()
    {
        foreach (var row in SentenceList)
        {
            string[] cell = row.Split(',');
            TextList.Add(new string[] { cell[0], cell[1] });
        }
    }
    public void ShowText(int i)
    {
        TitleText.text = TextList[i][0];
        Text.text = TextList[i][1];
    }
    public IEnumerator EndingList()
    {
        for (int i = 0; i < TextList.Count; i++)
        {
            ShowText(i);
            TitleText.DOFade(1, 1);
            Text.DOFade(1, 1);
            yield return new WaitForSeconds(1.5f);
            TitleText.DOFade(0, 1);
            Text.DOFade(0, 1);
            yield return new WaitForSeconds(1.5f);
        }
        Text.text = "感謝您的遊玩~";
        Text.DOFade(1, 2);
        yield return new WaitForSeconds(3f);
        TitleText.DOFade(0, 1);
        Text.DOFade(0, 1);
        yield return new WaitForSeconds(1.5f);
        GameManager.gameManager_instance.Save();
        SceneManager.LoadScene(1);
    }
    public void Skip()
    {
        StartCoroutine(SkipEnding());
    }
    public IEnumerator SkipEnding()
    {
        SkipPanel.SetActive(false);
        BlackMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager_instance.Save();
        SceneManager.LoadScene(1);
    }
}
