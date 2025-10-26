using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DuelUIManager : MonoBehaviour
{
    //DuelScene全域廣播
    public static int RoundInt; //回合數
    public static bool showStateText; //顯示階段文字
    public static bool showInformationText; //顯示訊息文字
    public static string Information; //訊息文字內容
    public static bool stateEventStart; //階段事件執行
    public static bool BattleEnd;
    public static bool isPlayerWin;

    public Button PauseButton;
    public Button SpeedButton;
    public TextMeshProUGUI StateText;
    public TextMeshProUGUI SpeedText;

    public GameObject CameraManager;
    public GameObject BattleStateManager;
    public GameObject DuelState;
    public GameObject MoveResult;
    public GameObject PauseUI;
    public GameObject DuelEndUI;
    public GameObject PlayerJobUI;
    public GameObject InformationText;

    public GameObject Blackmask;
    public AudioClip[] EffectClips;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager_instance.GameSpeed = 1;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            GameManager.gameManager_instance.nextState = false;
        }
        RoundInt = 0;
        Blackmask.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        isPlayerWin = true;
        BattleEnd = false;
        showStateText = false;
        showInformationText = false;
        stateEventStart = false;

        PauseButton.interactable = false;
        BattleStateManager.SetActive(false);
        DuelState.SetActive(false);
        MoveResult.SetActive(false);
        PauseUI.SetActive(false);
        DuelEndUI.SetActive(false);
        InformationText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        InformationText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Information;
        if (showInformationText)
        {
            InformationText.SetActive(true);
        }
        else
        {
            InformationText.SetActive(false);
        }

        if (PlayerUIManager.GetInstance().readyToDuel && EnemyManager.GetInstance().readyToDuel)
        {
            if (GameManager.gameManager_instance.isPracticeDuel && GameManager.gameManager_instance.nextState)
            {
                StartCoroutine(StartDuel());
            }
            else if (!GameManager.gameManager_instance.isPracticeDuel)
            {
                StartCoroutine(StartDuel());
            }
        }

        if (showStateText)
        {
            StartCoroutine(ShowStateText());
        }

        if (stateEventStart)
        {
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Draw)
            {
                StartCoroutine(StartDrawStateResult());
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Move)
            {
                StartCoroutine(StartMoveStateResult());
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.MoveResult)
            {
                StartCoroutine(EndMoveStateResult());
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack)
            {
                StartCoroutine(StartAttackState());
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.AttackResult)
            {
                StartCoroutine(StartAttackStateResult());
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.End)
            {
                StartCoroutine(EndStateResult());
            }
        }
    }
    public void CheckJob()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            GameManager.gameManager_instance.audioManager.PauseBackSound();
            PlayerJobUI.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetUpdate(true);
        }
        else
        {
            Time.timeScale = GameManager.gameManager_instance.GameSpeed;
            GameManager.gameManager_instance.audioManager.ContinueBackSound();
            PlayerJobUI.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetUpdate(true);
        }
    }
    public void GameSpeed()
    {
        if (GameManager.gameManager_instance.GameSpeed != 1)
        {
            GameManager.gameManager_instance.GameSpeed = 1;
            Time.timeScale = GameManager.gameManager_instance.GameSpeed;
        }
        else
        {
            GameManager.gameManager_instance.GameSpeed = 2f;
            Time.timeScale = GameManager.gameManager_instance.GameSpeed;
        }
        SpeedText.text = "×" + GameManager.gameManager_instance.GameSpeed.ToString();
    }
    public void PauseOrStartGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            GameManager.gameManager_instance.audioManager.PauseBackSound();
            PauseUI.SetActive(true);
        }
        else
        {
            GameManager.gameManager_instance.audioManager.PlayClip(1, EffectClips[0], false);
            Time.timeScale = GameManager.gameManager_instance.GameSpeed;
            GameManager.gameManager_instance.audioManager.ContinueBackSound();
            PauseUI.SetActive(false);
        }
    }
    public void ReStartGame()
    {
        StartCoroutine(ReStart());
    }
    public void BackMainMenu()
    {
        StartCoroutine(BackMenu());
    }
    public IEnumerator ReStart()
    {
        Blackmask.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetUpdate(true);
        GameManager.gameManager_instance.audioManager.PlayClip(1, EffectClips[0], false);
        yield return new WaitForSecondsRealtime(0.5f);
        LordingUI.NextScene = 5;
        SceneManager.LoadScene(2);
    }
    public IEnumerator BackMenu()
    {
        Blackmask.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetUpdate(true);
        GameManager.gameManager_instance.audioManager.PlayClip(1, EffectClips[0], false);
        yield return new WaitForSecondsRealtime(0.5f);
        LordingUI.NextScene = 1;
        SceneManager.LoadScene(2);
    }
    public IEnumerator StartDuel()
    {
        PlayerUIManager.GetInstance().readyToDuel = false;
        EnemyManager.GetInstance().readyToDuel = false;
        yield return StartCoroutine(CameraManager.GetComponent<CameraManager>().ShowEnemyPassiveSkill());
        GameManager.gameManager_instance.Save();
        PauseButton.interactable = true;
        BattleStateManager.SetActive(true);
        yield return 0;
    }
    public IEnumerator ShowStateText()
    {
        showStateText = false;
        //StateText.GetComponent<TextMeshProUGUI>().text = DuelBattleManager.duelStateMode.ToString();
        DuelState.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        DuelState.SetActive(false);
    }
    public IEnumerator StartDrawStateResult()
    {
        stateEventStart = false;
        StateText.text = "抽牌階段";
        RoundInt += 1;
        showStateText = true;
        yield return new WaitForSeconds(1f);
        PlayerUIManager.GetInstance().NormalDrawCard();
    }
    public IEnumerator StartMoveStateResult()
    {
        stateEventStart = false;
        MoveResult.SetActive(true);
        StateText.text = "行動階段";
        showStateText = true;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(CameraManager.GetComponent<CameraManager>().MoveStateLook());
    }
    public IEnumerator EndMoveStateResult()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var enemys = EnemyManager.GetInstance();
        stateEventStart = false;
        yield return StartCoroutine(MoveResult.GetComponent<NewMoveResult>().MoveResult());
        MoveResult.SetActive(false);
        _player_data.canMove = false;
        _player_data.isReady = true;
        enemys.isReady = true;
    }
    public IEnumerator StartAttackState()
    {
        stateEventStart = false;
        StateText.text = "戰鬥階段";
        showStateText = true;
        yield return new WaitForSeconds(1f);
        PlayerUIManager.GetInstance().PlayerData.playerStateMode = NewGameState.NewPlayerStateMode.PlayerActivate;
        yield return null;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            DialogueUIController.openDialogueUI = true;
        }
    }
    public IEnumerator StartAttackStateResult()
    {
        showInformationText = true;
        Information = "卡牌結算中";
        stateEventStart = false;
        yield return StartCoroutine(PlayerUIManager.GetInstance().AttackReady());
        if (PlayerUIManager.GetInstance().isFirstATK)
        {
            yield return StartCoroutine(PlayerAttackResult());
            yield return 0;
            yield return StartCoroutine(EnemyAttackResult());
        }
        else
        {
            yield return StartCoroutine(EnemyAttackResult());
            yield return 0;
            yield return StartCoroutine(PlayerAttackResult());
        }
        showInformationText = false;
        yield return null;
    }
    public IEnumerator EndStateResult()
    {
        GameManager.gameManager_instance.nextState = false;
        stateEventStart = false;
        StateText.text = "結束階段";
        showStateText = true;
        yield return new WaitForSeconds(1f);
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            showInformationText = false;
            isPlayerWin = false;
            yield return StartCoroutine(DuelEnding());
            yield break;
        }
        PlayerUIManager.GetInstance().PlayerData.isReady = true;
        EnemyManager.GetInstance().isReady = true;
        yield return null;
    }
    public IEnumerator PlayerAttackResult()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        if (BattleEnd)
        {
            yield break;
        }
        if (_player_data.PlayerAction[1] == 0 && _player_data.PlayerAction[2] == 0)
        {
            Information = "此回合玩家未攻擊";
            yield return new WaitForSeconds(1f);
        }
        if (_player_data.PlayerAction[1] != 0)
        {
            Information = "結算玩家攻擊牌";
            if (_player_data.isNormalATKZones == true) 
            {
                var HitZone = new List<int[]>();
                for (int i = 0; i < _player_data.ATKZone; i++)
                {
                    HitZone.Add(new int[] { _player_data.PlayerLocation[0], (_player_data.PlayerLocation[1] + i + 1) });
                }
                _player_data.ATKHitZone = HitZone;
            }
            else if (_player_data.isNormalATKZones == false)
            {
                _player_data.ATKHitZone = _player_data.SpecialNormalZone;
            }
            CoordinatesEffectAni.HitAniIndex = 0;
            PlayerUIManager.GetInstance().PlayerAttackSound();
            LocationManager.showPlayerATKZone = true;
            yield return new WaitForSeconds(2 / 3f);
            yield return StartCoroutine(EnemyManager.GetInstance().EnemysHurt());
            EnemyManager.GetInstance().isEnemyDie = false;
            _player_data.PlayerAction[1] = 0;
            LocationManager.showPlayerATKZone = false;
        }
        if (BattleEnd)
        {
            yield return StartCoroutine(DuelEnding());
            yield break;
        }
        if (_player_data.PlayerAction[2] != 0)
        {
            yield return StartCoroutine(CameraManager.GetComponent<CameraManager>().ShowPlayerSkill());
        }
        if (BattleEnd)
        {
            yield return StartCoroutine(DuelEnding());
            yield break;
        }
        _player_data.isReady = true;
        yield return null;
    }
    public IEnumerator EnemyAttackResult()
    {
        var enemys = EnemyManager.GetInstance();
        if (BattleEnd)
        {
            yield break;
        }
        yield return StartCoroutine(CameraManager.GetComponent<CameraManager>().EnemyAttackResult());
        if (BattleEnd)
        {
            yield return StartCoroutine(DuelEnding());
            yield break;
        }
        enemys.isReady = true;
        yield return null;
    }
    public IEnumerator DuelEnding()
    {
        PlayerUIManager.GetInstance().isDuelEnd = true;
        DuelEndUI.SetActive(true);
        BattleStateManager.SetActive(false);
        yield return null;
    }
}
