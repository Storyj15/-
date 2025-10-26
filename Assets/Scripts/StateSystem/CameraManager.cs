using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject EnemyCamera;
    public GameObject EnemyCam1;
    public GameObject FollowEnemy;
    public GameObject PlayerSkillAniPanel;
    public GameObject PlayerSkillAni;
    public AudioClip TrapSound;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.SetActive(true);
        EnemyCamera.SetActive(false);
    }

    // Update is called once per frame
    public void FollowAtEnemy()
    {
        EnemyCam1.GetComponent<CinemachineVirtualCamera>().Follow = FollowEnemy.transform;
        EnemyCam1.transform.position = FollowEnemy.transform.position;
        MainCamera.SetActive(false);
        EnemyCamera.SetActive(true);
    }
    public void LookScene()
    {
        MainCamera.SetActive(true);
        EnemyCamera.SetActive(false);
    }
    public IEnumerator MoveStateLook()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var enemys = EnemyManager.GetInstance();
        for (int i = 0; i < enemys.EnemyPiece.Count; i++)
        {
            DuelUIManager.showInformationText = true;
            FollowEnemy = enemys.EnemyPiece[i];
            enemys.EnemyPiece[i].GetComponent<EnemyData>().EnemyAction();
            if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 0) //攻擊行為
            {
                DuelUIManager.Information = enemys.EnemyPiece[i].name + "準備攻擊";
                FollowAtEnemy();
            }
            else if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 1) //移動行為
            {
                DuelUIManager.Information = enemys.EnemyPiece[i].name + "進行移動";
                FollowAtEnemy();
            }
            else if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 2) //放陷阱行為
            {
                DuelUIManager.Information = enemys.EnemyPiece[i].name + "陷阱攻擊";
                FollowAtEnemy();
            }
            yield return new WaitForSeconds(1f);
            LookScene();
            if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    DuelUIManager.Information = enemys.EnemyPiece[i].name + "攻擊區域";
                    LocationManager.showEnemyATKZone = true;
                    yield return new WaitForSeconds(0.25f);
                    LocationManager.showEnemyATKZone = false;
                    yield return new WaitForSeconds(0.25f);
                }
            }
            else if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 1)
            {
                //yield return StartCoroutine(enemys.EnemyMove(i));
                yield return new WaitForSeconds(1.5f);
            }
            else if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 2)
            {
                DuelUIManager.Information = enemys.EnemyPiece[i].name + "陷阱位置";
                yield return new WaitForSeconds(1.5f);
            }
        }
        yield return 0;
        _player_data.canMove = true;
        enemys.isReady = true;
        _player_data.playerStateMode = NewGameState.NewPlayerStateMode.PlayerActivate;
        DuelUIManager.showInformationText = true;
        DuelUIManager.Information = "選擇移動至哪裡";
        yield return 0;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            DialogueUIController.openDialogueUI = true;
        }
    }
    public IEnumerator EnemyAttackResult()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var enemys = EnemyManager.GetInstance();
        for (int i = 0; i < enemys.EnemyPiece.Count; i++)
        {
            if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 0) //攻擊行動
            {
                var atkhit = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionZone.Any(arr => arr.GetType() == _player_data.PlayerLocation.GetType() && arr.SequenceEqual(_player_data.PlayerLocation));
                DuelUIManager.Information = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionNameText;
                FollowEnemy = enemys.EnemyPiece[i];
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", true);
                FollowAtEnemy();
                yield return new WaitForSeconds(1f);
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", false);
                LookScene();
                enemys.EnemyATKZone = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionZone;
                CoordinatesEffectAni.HitAniIndex = enemys.EnemyPiece[i].GetComponent<EnemyData>().EnemyID;
                LocationManager.showEnemyATKZone = true;
                yield return new WaitForSeconds(2 / 3f);
                if (atkhit)
                {
                    if (_player_data.Defense > 0 && _player_data.Defense >= enemys.EnemyPiece[i].GetComponent<EnemyData>().ATKValue)
                    {
                        _player_data.Defense -= enemys.EnemyPiece[i].GetComponent<EnemyData>().ATKValue;
                    }
                    else
                    {
                        PlayerUIManager.GetInstance().PlayerData.notHurt = false;
                        _player_data.HP -= (enemys.EnemyPiece[i].GetComponent<EnemyData>().ATKValue - _player_data.Defense);
                        _player_data.Defense = 0;
                    }
                    DuelUIManager.Information = "攻擊命中";
                    PlayerUIManager.GetInstance().PlayerHurtSound();
                    PlayerUIManager.GetInstance().PlayerPiece.transform.DOPunchPosition(new Vector3(15, 0, 0), 0.5f, 10);
                    yield return new WaitForSeconds(0.8f);
                    if (_player_data.HP <= 0)
                    {
                        yield return StartCoroutine(PlayerLose());
                        yield break;
                    }
                }
                else
                {
                    DuelUIManager.Information = "攻擊未命中";
                    yield return new WaitForSeconds(0.8f);
                }
                LocationManager.showEnemyATKZone = false;
            }
            if (enemys.EnemyPiece[i].GetComponent<EnemyData>().ChooseActionType == 2) //陷阱攻擊行動
            {
                var traphit = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionZone.Any(arr => arr.GetType() == _player_data.PlayerLocation.GetType() && arr.SequenceEqual(_player_data.PlayerLocation));
                DuelUIManager.Information = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionNameText;
                FollowEnemy = enemys.EnemyPiece[i];
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", true);
                FollowAtEnemy();
                yield return new WaitForSeconds(1f);
                GameManager.gameManager_instance.audioManager.PlayClip(2, TrapSound, false);
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", false);
                LookScene();
                Trap.ActionPlay = true;
                yield return new WaitForSeconds(2 / 3f);
                Trap.ActionPlay = false;
                if (traphit)
                {
                    if (_player_data.Defense > 0 && _player_data.Defense >= enemys.EnemyPiece[i].GetComponent<EnemyData>().ATKValue)
                    {
                        _player_data.Defense -= enemys.EnemyPiece[i].GetComponent<EnemyData>().EnemyUniqueValue;
                    }
                    else
                    {
                        PlayerUIManager.GetInstance().PlayerData.notHurt = false;
                        _player_data.HP -= (enemys.EnemyPiece[i].GetComponent<EnemyData>().EnemyUniqueValue - _player_data.Defense);
                        _player_data.Defense = 0;
                    }
                    DuelUIManager.Information = "攻擊命中";
                    PlayerUIManager.GetInstance().PlayerHurtSound();
                    PlayerUIManager.GetInstance().PlayerPiece.transform.DOPunchPosition(new Vector3(15, 0, 0), 0.5f, 10);
                    yield return new WaitForSeconds(0.8f);
                    if (_player_data.HP <= 0)
                    {
                        yield return StartCoroutine(PlayerLose());
                        yield break;
                    }
                }
                else
                {
                    DuelUIManager.Information = "攻擊未命中";
                    yield return new WaitForSeconds(0.8f);
                }
            }
        }
        yield return 0;
    }
    public IEnumerator ShowEnemyPassiveSkill()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var enemys = EnemyManager.GetInstance();
        for (int i = 0; i < enemys.EnemyPiece.Count; i++)
        {
            if (enemys.EnemyPiece[i].GetComponent<EnemyData>().havePassiveSkill == true)
            {
                DuelUIManager.showInformationText = true;
                DuelUIManager.Information = enemys.EnemyPiece[i].GetComponent<EnemyData>().ActionNameText;
                FollowEnemy = enemys.EnemyPiece[i];
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", true);
                FollowAtEnemy();
                yield return new WaitForSeconds(1f);
                enemys.EnemyPiece[i].GetComponent<EnemyData>().animator.SetBool("EnemyAction", false);
                LookScene();
                DuelUIManager.Information = "玩家攻擊力下降了";
                enemys.EnemyPiece[i].GetComponent<EnemyData>().PassiveSkill();
                LocationManager.showEnemyPassiveSkillZone = true;
                yield return new WaitForSeconds(2 / 3f);
                CoordinatesEffectAni.EnemyPassiveDeBuff = false;
                yield return new WaitForSeconds(0.5f);
                LocationManager.showPlayerSkillZone = false;
                DuelUIManager.showInformationText = false;
                yield return null;
            }
        }
        yield return 0;
    }
    public IEnumerator ShowPlayerSkill()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        var HitZone = new List<int[]>();
        _player_data.Skill();
        DuelUIManager.Information = PlayerUIManager.GetInstance().PlayerData.SkillName;
        PlayerSkillAniPanel.SetActive(true);
        PlayerSkillAniPanel.GetComponent<Image>().DOFade(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        PlayerSkillAni.SetActive(true);
        yield return new WaitForSeconds(1f);
        PlayerSkillAni.SetActive(false);
        PlayerSkillAniPanel.GetComponent<Image>().DOFade(0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        PlayerSkillAniPanel.SetActive(false);
        LookScene();
        PlayerUIManager.GetInstance().PlayerAttackSound();
        LocationManager.showPlayerSkillZone = true;
        yield return new WaitForSeconds(2 / 3f);
        if (CoordinatesEffectAni.isBuffSkill)
        {
            CoordinatesEffectAni.isBuffSkill = false;
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            for (int i = 0; i < _player_data.SkillZones.Count; i++)
            {
                HitZone.Add(_player_data.SkillZones[i]);
            }
            _player_data.ATKHitZone = HitZone;
            yield return StartCoroutine(EnemyManager.GetInstance().EnemysSkillHurt());
            EnemyManager.GetInstance().isEnemyDie = false;
        }
        _player_data.PlayerAction[2] = 0;
        LocationManager.showPlayerSkillZone = false;
        yield return null;
    }
    public IEnumerator PlayerLose()
    {
        PlayerAnimator.playerdie = true;
        LocationManager.showEnemyATKZone = false;
        DuelUIManager.showInformationText = false;
        DuelUIManager.isPlayerWin = false;
        DuelUIManager.BattleEnd = true;
        yield return 0;
    }
}
