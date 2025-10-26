using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobData
{
    //��¦�ƭ�
    public string JobName;
    public string SkillName;
    public string DefenseText;
    public string ATKText;
    public string SkillText;
    public string Upgrage1Text;
    public string Upgrage2Text;
    public string Upgrage3Text;
    public int HP = 10; 
    public int MoveValue = 1; 
    public int ATKValue; 
    public int ATKZone;
    public int[] BuffValue;
    //�S��ƭ�
    public List<int[]> SpecialNormalZone;
    public bool isNormalATKZone = true;
    public int SkillValue;
    public List<int[]> SkillZones;
    public abstract void Setting();     //¾�~�ƭȳ]�w
    public abstract void Upgrade();    //�d�P�ɯų]�w
    public abstract void Skill();     //�ޯ�ĪG
    public abstract void SpecialProcess();    //�S��B�z
}
public class Warrior : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "�i��";
        SkillName = "�t�C";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillText = "���e��Ҧ��ĤH\n�ˮ`2";
        Upgrage1Text = "�אּ�@�ҭ�+2";
        Upgrage2Text = "�ˮ`+2";
        Upgrage3Text = "�t�C�ˮ`+2";
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = 1;
            DefenseText = "�@�ҭ�+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 2;
            ATKText = "�����Z��4\n�ˮ`4";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "���a���e��Ҧ��ĤH\n�ˮ`4";
        }
    }
    public override void Skill()
    {
        SkillValue = 2 + BuffValue[2];
        var SkillZone = new List<int[]>();
        for (int i = 0; i < 8; i++)
        {
            SkillZone.Add(new int[2] {PlayerUIManager.GetInstance().PlayerData.PlayerLocation[0],i});
        }
        for (int j = 0; j < PlayerUIManager.GetInstance().PlayerData.PlayerLocation[1] + 1; j++)
        {
            SkillZone.RemoveAt(0);
        }
        SkillZones = SkillZone;
    }
    public override void SpecialProcess()
    {
        
    }
}
public class Magician : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "�۷Q";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillName = "�]�k�z�}";
        SkillText = "�H��3��\n5�I�ˮ`";
        Upgrage1Text = "�אּ�@�ҭ�+3\n�^�X�������k0";
        Upgrage2Text = "�����Z��+1";
        Upgrage3Text = "�R����+2";
        //HP = 10;
        //MoveValue = 1;
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = 2;
            DefenseText = "�@�ҭ�+3\n�^�X�������k0";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            ATKZone += 1;
            ATKText = "�����Z��5\n�ˮ`2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "�H��5��\n5�I�ˮ`";
        }
    }
    public override void Skill()
    {
        SkillValue = 5;
        var SkillZone = new List<int[]>();
        var TargetZone = new List<int[]>();
        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 4; b++)
            {
                TargetZone.Add(new int[] {a,b +4});
            }
        }
        for (int i = 0; i < (3 + BuffValue[2]); i++)
        {
            var RandomInt = Random.Range(0, TargetZone.Count);
            SkillZone.Add(TargetZone[RandomInt]);
            TargetZone.Remove(TargetZone[RandomInt]);
        }
        SkillZones = SkillZone;
    }
    public override void SpecialProcess()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.End)
            {
                PlayerUIManager.GetInstance().PlayerData.Defense = 0;
            }
        }
    }
}
public class Archer : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "����";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillName = "�b�B";
        SkillText = "�H��2��\n4�I�ˮ`";
        Upgrage1Text = "�אּ�@�ҭ�+2";
        Upgrage2Text = "�����Z��+2";
        Upgrage3Text = "�R����+2";
        //HP = 10;
        //MoveValue = 1;
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = 1;
            DefenseText = "�@�ҭ�+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            ATKZone += 2;
            ATKText = "�����Z��6\n�ˮ`2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "�H��4��\n4�I�ˮ`";
        }
    }
    public override void Skill()
    {
        SkillValue = 4;
        var SkillZone = new List<int[]>();
        var TargetZone = new List<int[]>();
        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 4; b++)
            {
                TargetZone.Add(new int[] { a, b + 4 });
            }
        }
        for (int i = 0; i < (2 + BuffValue[2]); i++)
        {
            var RandomInt = Random.Range(0, TargetZone.Count);
            SkillZone.Add(TargetZone[RandomInt]);
            TargetZone.Remove(TargetZone[RandomInt]);
        }
        SkillZones = SkillZone;
    }
    public override void SpecialProcess()
    {
       
    }
}
public class Gambler : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "�R�B";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillName = "�R�w������m";
        SkillText = "�e��2��ˮ`10\n������ˮ`�h0";
        Upgrage1Text = "�אּ�@�ҭ�\n�W�[0~3";
        Upgrage2Text = "�ˮ`+1";
        Upgrage3Text = "�ˮ`+10\n������ˮ`�h0";
        //HP = 10;
        //MoveValue = 1;
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            DefenseText = "�@�ҭ�+0~3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 1;
            ATKText = "�����Z��4\n�ˮ`3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 10;
            SkillText = "�e��2��ˮ`20\n������ˮ`�h0";
        }
    }
    public override void Skill()
    {
        var SkillZone = new List<int[]>();
        for (int i = 0; i < 2; i++)
        {
            SkillZone.Add(new int[2] { PlayerUIManager.GetInstance().PlayerData.PlayerLocation[0], PlayerUIManager.GetInstance().PlayerData.PlayerLocation[1] + (i + 1) });
        }
        SkillZones = SkillZone;
    }
    public override void SpecialProcess()
    {
        if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.AttackResult && !PlayerUIManager.GetInstance().isFirstATK)
        {
            SkillValue = 0;
        }
        else if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.AttackResult && PlayerUIManager.GetInstance().isFirstATK)
        {
            SkillValue = 10 + BuffValue[2];
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = Random.Range(-1, 3);
        }
    }
}
public class Foodie : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "���j";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillName = "�ɭ�";
        SkillText = "�ͩR��+1";
        Upgrage1Text = "�אּ�@�ҭ�+2";
        Upgrage2Text = "�ˮ`+1";
        Upgrage3Text = "�אּ�ͩR��+2";
        //HP = 10;
        //MoveValue = 1;
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = 1;
            DefenseText = "�@�ҭ�+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 1;
            ATKText = "�����Z��4\n�ˮ`3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 1;
            SkillText = "�ͩR��+2";
        }
    }
    public override void Skill()
    {
        CoordinatesEffectAni.isBuffSkill = true;
        SkillValue = 1 + BuffValue[2];
        SkillZones = new List<int[]> { PlayerUIManager.GetInstance().PlayerData.PlayerLocation };
        PlayerUIManager.GetInstance().PlayerData.HP += (SkillValue * PlayerUIManager.GetInstance().PlayerData.PlayerAction[2]);
    }
    public override void SpecialProcess()
    {

    }
}
public class Musician : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "�۫�";
        DefenseText = "�@�ҭ�+1";
        ATKText = "�����Z��4\n�ˮ`2";
        SkillName = "�ܫ�";
        SkillText = "�o���M��\n�������O�ˮ`+1";
        Upgrage1Text = "�אּ�@�ҭ�+2";
        Upgrage2Text = "�����d���ܥ���\n�ˮ`1";
        Upgrage3Text = "�אּ�o���M��\n�������O�ˮ`+2";
        //HP = 10;
        //MoveValue = 1;
        ATKValue = 2;
        ATKZone = 4;
        BuffValue = new int[3] { 0, 0, 0 }; //(Defense,ATKValue,SkillValue)
        Upgrade();
    }
    public override void Upgrade()
    {
        if (GameManager.gameManager_instance.PlayerUpgrade[0] == 1)
        {
            BuffValue[0] = 1;
            DefenseText = "�@�ҭ�+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            isNormalATKZone = false;
            SpecialNormalZone = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    SpecialNormalZone.Add(new int[] { i, (j + 4) });
                }
            }
            BuffValue[1] = -1;
            ATKText = "��������\n�ˮ`1";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 1;
            SkillText = "�o���M��\n�������O�ˮ`+2";
        }
    }
    public override void Skill()
    {
        CoordinatesEffectAni.isBuffSkill = true;
        SkillValue = 1 + BuffValue[2];
        SkillZones = new List<int[]> { PlayerUIManager.GetInstance().PlayerData.PlayerLocation };
        PlayerUIManager.GetInstance().PlayerData.ATKValue += (SkillValue * PlayerUIManager.GetInstance().PlayerData.PlayerAction[2]);
    }
    public override void SpecialProcess()
    {
        
    }
}