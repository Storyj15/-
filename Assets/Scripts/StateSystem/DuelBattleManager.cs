using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelBattleManager : MonoBehaviour
{
    //���A������
    public static DuelBattleIState CurrentDuelState; //�ثe�M�����A(�i�J/����/�h�X)
    public static Dictionary<NewGameState.NewDuelStateMode, DuelBattleIState> DuelIState = new Dictionary<NewGameState.NewDuelStateMode, DuelBattleIState>();
    public static NewGameState.NewDuelStateMode duelStateMode;

    private void Awake()
    {
        DuelIState.Clear();
        DuelIState.Add(NewGameState.NewDuelStateMode.Draw, new NewDrawState());
        DuelIState.Add(NewGameState.NewDuelStateMode.Move, new NewMoveState());
        DuelIState.Add(NewGameState.NewDuelStateMode.MoveResult, new NewMoveResultState());
        DuelIState.Add(NewGameState.NewDuelStateMode.Attack, new NewAttackState());
        DuelIState.Add(NewGameState.NewDuelStateMode.AttackResult, new NewAttackResultState());
        DuelIState.Add(NewGameState.NewDuelStateMode.End, new NewEndState());
    }
    private void Start()
    {
        TranslateDuelState(duelStateMode);
        var clip = GameManager.gameManager_instance.audioClip[GameManager.gameManager_instance.audioInt];
        GameManager.gameManager_instance.audioManager.PlayClip(0, clip, true);
    }
    private void Update()
    {
        CurrentDuelState.UpdateState();
        if (Time.timeScale == 0 && GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Pause();
        }
        else if (Time.timeScale != 0 && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().UnPause();
        }
    }
    public static void TranslateDuelState(NewGameState.NewDuelStateMode type) //�����M�����q�ɩҰ���
    {
        CurrentDuelState = DuelIState[type];
        CurrentDuelState.EnterState();
    }
}
