using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobData
{
    //基礎數值
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
    //特殊數值
    public List<int[]> SpecialNormalZone;
    public bool isNormalATKZone = true;
    public int SkillValue;
    public List<int[]> SkillZones;
    public abstract void Setting();     //職業數值設定
    public abstract void Upgrade();    //卡牌升級設定
    public abstract void Skill();     //技能效果
    public abstract void SpecialProcess();    //特殊處理
}
public class Warrior : JobData
{
    public override void Setting()
    {
        isNormalATKZone = true;
        JobName = "勇氣";
        SkillName = "聖劍";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillText = "正前方所有敵人\n傷害2";
        Upgrage1Text = "改為護甲值+2";
        Upgrage2Text = "傷害+2";
        Upgrage3Text = "聖劍傷害+2";
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
            DefenseText = "護甲值+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 2;
            ATKText = "攻擊距離4\n傷害4";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "玩家正前方所有敵人\n傷害4";
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
        JobName = "幻想";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillName = "魔法爆破";
        SkillText = "隨機3格\n5點傷害";
        Upgrage1Text = "改為護甲值+3\n回合結束時歸0";
        Upgrage2Text = "攻擊距離+1";
        Upgrage3Text = "命中格+2";
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
            DefenseText = "護甲值+3\n回合結束時歸0";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            ATKZone += 1;
            ATKText = "攻擊距離5\n傷害2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "隨機5格\n5點傷害";
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
        JobName = "野心";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillName = "箭雨";
        SkillText = "隨機2格\n4點傷害";
        Upgrage1Text = "改為護甲值+2";
        Upgrage2Text = "攻擊距離+2";
        Upgrage3Text = "命中格+2";
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
            DefenseText = "護甲值+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            ATKZone += 2;
            ATKText = "攻擊距離6\n傷害2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 2;
            SkillText = "隨機4格\n4點傷害";
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
        JobName = "命運";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillName = "命定的正位置";
        SkillText = "前方2格傷害10\n拿到後攻傷害則0";
        Upgrage1Text = "改為護甲值\n增加0~3";
        Upgrage2Text = "傷害+1";
        Upgrage3Text = "傷害+10\n拿到後攻傷害則0";
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
            DefenseText = "護甲值+0~3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 1;
            ATKText = "攻擊距離4\n傷害3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 10;
            SkillText = "前方2格傷害20\n拿到後攻傷害則0";
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
        JobName = "飢餓";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillName = "暴食";
        SkillText = "生命值+1";
        Upgrage1Text = "改為護甲值+2";
        Upgrage2Text = "傷害+1";
        Upgrage3Text = "改為生命值+2";
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
            DefenseText = "護甲值+2";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[1] == 1)
        {
            BuffValue[1] = 1;
            ATKText = "攻擊距離4\n傷害3";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 1;
            SkillText = "生命值+2";
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
        JobName = "旋律";
        DefenseText = "護甲值+1";
        ATKText = "攻擊距離4\n傷害2";
        SkillName = "變奏";
        SkillText = "這場決鬥\n攻擊指令傷害+1";
        Upgrage1Text = "改為護甲值+2";
        Upgrage2Text = "攻擊範圍變全場\n傷害1";
        Upgrage3Text = "改為這場決鬥\n攻擊指令傷害+2";
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
            DefenseText = "護甲值+2";
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
            ATKText = "攻擊全場\n傷害1";
        }
        if (GameManager.gameManager_instance.PlayerUpgrade[2] == 1)
        {
            BuffValue[2] = 1;
            SkillText = "這場決鬥\n攻擊指令傷害+2";
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