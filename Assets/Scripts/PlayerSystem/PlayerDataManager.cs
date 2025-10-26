using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
    public NewGameState.NewPlayerStateMode playerStateMode;

    //��¦�ƭ�
    public int JobIndex; //Job�N��
    public string DefenseText;
    public string ATKText;
    public string SkillText;
    public string SkillName;
    public string Upgrage1Text;
    public string Upgrage2Text;
    public string Upgrage3Text;
    public int HP; 
    public int MoveValue; 
    public int ATKValue; 
    public int ATKZone;
    public int[] BuffValue;

    //�S��ƭ�
    public List<int[]> SpecialNormalZone;
    public bool isNormalATKZones;
    public int SkillValue;
    public List<int[]> SkillZones;

    //�H�M���ܤƼƭ�
    public bool notHurt;
    public int DrawAmount;
    public int Defense; //���m��
    public int[] PlayerLocation = new int[2]; //���a�Ҧb��m
    public int[] MoveToLocation = new int[2]; //���a�N�n���ʨ쪺��m
    public List<int[]> ATKHitZone;
    public int[] PlayerAction; //���a�欰(���m�P/�����P/�ޯ�P)

    //���a���A
    public bool canMove; //��ܲ��ʦܭ���
    public bool isReady; //���a�O�_�w�ǳƧ���
    public bool isFirstATK = false; //�O�_����
    public void SettingValue()
    {
        notHurt = true;
        playerStateMode = NewGameState.NewPlayerStateMode.PlayerDeactivate;
        PlayerAction = new int[3] { 0, 0, 0 };
        JobIndex = GameManager.gameManager_instance.PlayerJob;
        GameManager.gameManager_instance.Jobs[JobIndex].Setting();
        HP = GameManager.gameManager_instance.Jobs[JobIndex].HP;
        MoveValue = GameManager.gameManager_instance.Jobs[JobIndex].MoveValue;
        ATKValue = GameManager.gameManager_instance.Jobs[JobIndex].ATKValue;
        ATKZone = GameManager.gameManager_instance.Jobs[JobIndex].ATKZone;
        Defense = 0;
        BuffValue = GameManager.gameManager_instance.Jobs[JobIndex].BuffValue;
        SkillName = GameManager.gameManager_instance.Jobs[JobIndex].SkillName;
        Upgrage1Text = GameManager.gameManager_instance.Jobs[JobIndex].Upgrage1Text;
        Upgrage2Text = GameManager.gameManager_instance.Jobs[JobIndex].Upgrage2Text;
        Upgrage3Text = GameManager.gameManager_instance.Jobs[JobIndex].Upgrage3Text;
        DefenseText = GameManager.gameManager_instance.Jobs[JobIndex].DefenseText;
        ATKText = GameManager.gameManager_instance.Jobs[JobIndex].ATKText;
        SkillText = GameManager.gameManager_instance.Jobs[JobIndex].SkillText;
        isNormalATKZones = GameManager.gameManager_instance.Jobs[JobIndex].isNormalATKZone;
        SpecialNormalZone = GameManager.gameManager_instance.Jobs[JobIndex].SpecialNormalZone;
    }
    public void Skill()
    {
        GameManager.gameManager_instance.Jobs[JobIndex].Skill();
        SkillValue = GameManager.gameManager_instance.Jobs[JobIndex].SkillValue;
        SkillZones = GameManager.gameManager_instance.Jobs[JobIndex].SkillZones;
    }
}
