using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DuelBattleIState
{
    public abstract void EnterState();
    public abstract void UpdateState();
}
public class NewGameState
{
    public enum NewDuelStateMode
    {
        Draw,
        Move,
        MoveResult,
        Attack,
        AttackResult,
        End,
    }
    public enum NewPlayerStateMode //玩家狀態
    {
        PlayerActivate,
        PlayerReady,
        PlayerDeactivate,
    }
}
public class NewDrawState : DuelBattleIState
{
    public override void EnterState()
    {
        if (PlayerUIManager.GetInstance().HandCardAmount < 3)
        {
            PlayerUIManager.GetInstance().PlayerData.DrawAmount += (3 - PlayerUIManager.GetInstance().HandCardAmount);
        }
        else
        {
            PlayerUIManager.GetInstance().PlayerData.DrawAmount += 1;
        }
        PlayerUIManager.GetInstance().PlayerData.isReady = false;
        EnemyManager.GetInstance().isReady = true;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (GameManager.gameManager_instance.isPracticeDuel && GameManager.gameManager_instance.nextState)
        {
            if (_player_data.isReady && _enemyManager.isReady)
            {
                _player_data.isReady = false;
                _enemyManager.isReady = false;
                DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.Move;
                DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
            }
        }
        else if (!GameManager.gameManager_instance.isPracticeDuel)
        {
            if (_player_data.isReady && _enemyManager.isReady)
            {
                _player_data.isReady = false;
                _enemyManager.isReady = false;
                DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.Move;
                DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
            }
        }
    }
}
public class NewMoveState : DuelBattleIState //移動階段(引用IState的運行模式)
{
    public override void EnterState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        _player_data.isReady = false;
        _enemyManager.isReady = false;
        _player_data.playerStateMode = NewGameState.NewPlayerStateMode.PlayerDeactivate;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (_player_data.isReady && _enemyManager.isReady)
        {
            _player_data.canMove = false;
            _player_data.isReady = false;
            _enemyManager.isReady = false;
            DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.MoveResult;
            DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
        }
    }
}
public class NewMoveResultState : DuelBattleIState //移動階段(引用IState的運行模式)
{
    public override void EnterState()
    {
        PlayerUIManager.GetInstance().PlayerData.playerStateMode = NewGameState.NewPlayerStateMode.PlayerDeactivate;
        PlayerUIManager.GetInstance().PlayerData.isReady = false;
        EnemyManager.GetInstance().isReady = false;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {

        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (_player_data.isReady && _enemyManager.isReady)
        {
            _player_data.isReady = false;
            _enemyManager.isReady = false;
            DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.Attack;
            DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
        }
    }
}
public class NewAttackState : DuelBattleIState
{
    public override void EnterState()
    {
        PlayerUIManager.GetInstance().PlayerData.isReady = false;
        EnemyManager.GetInstance().isReady = true;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (_player_data.isReady && _enemyManager.isReady)
        {
            _player_data.isReady = false;
            _enemyManager.isReady = false;
            DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.AttackResult;
            DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
        }
    }
}
public class NewAttackResultState : DuelBattleIState //移動階段(引用IState的運行模式)
{
    public override void EnterState()
    {
        PlayerUIManager.GetInstance().PlayerData.playerStateMode = NewGameState.NewPlayerStateMode.PlayerDeactivate;
        PlayerUIManager.GetInstance().PlayerData.isReady = false;
        EnemyManager.GetInstance().isReady = false;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (_player_data.isReady && _enemyManager.isReady)
        {
            _player_data.isReady = false;
            _enemyManager.isReady = false;
            //DuelUIController.resultEnd = true;
            DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.End;
            DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
        }
    }
}
public class NewEndState : DuelBattleIState //移動階段(引用IState的運行模式)
{
    public override void EnterState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        //PracticeLimtedSetting.PracticeTurn += 1;
        _player_data.isReady = false;
        _enemyManager.isReady = false;
        DuelUIManager.stateEventStart = true;
    }
    public override void UpdateState()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var _enemyManager = EnemyManager.GetInstance();
        if (_player_data.isReady && _enemyManager.isReady)
        {
            _player_data.isReady = false;
            _enemyManager.isReady = false;
            DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.Draw;
            DuelBattleManager.TranslateDuelState(DuelBattleManager.duelStateMode);
        }
    }
}
