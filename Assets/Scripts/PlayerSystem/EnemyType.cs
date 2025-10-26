using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyType
{
    //°òÂ¦¼Æ­È
    public string Name;
    public string ActionName;
    public float BodyShape;
    public int HP; //¦å¶q
    public int Defense;
    public int ATKValue; //§ðÀ»¼Æ­È
    public List<int[]> ActionZone;
    public int[] ActionType;
    public int ChooseActionType; //(0:ATK/1:move/2:TrapATK)
    public bool havePassiveSkill;

    //¯S®í¼Æ­È
    public int UniqueValue;
    public abstract void Setting();
    public abstract void PassiveSkill(int HP);
    public abstract void Action(int type,int[] location);
}
public class Tomato : EnemyType
{
    public override void Setting()
    {
        Name = "ÂA¦å§BÀï";
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            HP = 3;
            Defense = 1;
            ActionType = new int[] { 1 };
        }
        else
        {
            HP = Random.Range(2, 9);
            Defense = Random.Range(0, 3);
            ActionType = new int[] { 0, 1 };
        }
        UniqueValue = 0;
        havePassiveSkill = false;
        if (HP <= 4)
        {
            ATKValue = 3;
            BodyShape = 0.6f;
        }
        else if(HP > 4 && HP <= 6)
        {
            ATKValue = 4;
            BodyShape = 0.8f;
        }
        else
        {
            ATKValue = 5;
            BodyShape = 1f;
        }
    }
    public override void PassiveSkill(int none)
    {
        
    }
    public override void Action(int type,int[] location)
    {
        if (type == 0)
        {
            Skill1();
        }
        if (type == 1)
        {
            Skill2();
        }
    }
    public void Skill1()
    {
        ActionName = "´c·N¤¤¶Ë";
        ChooseActionType = 0;
        var Zone = new List<int[]>();
        for (int i = 0; i < 3; i++)
        {
            Zone.Add(new int[2] {i,3});
        }
        ActionZone = Zone;
    }
    public void Skill2()
    {
        ActionName = "¦å²½¸tÂ§";
        ChooseActionType = 0;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            var TargetZone = new List<int[]>();
            TargetZone.Add(new int[] { 1, 1 });
            TargetZone.Add(new int[] { 1, 3 });
            ActionZone = TargetZone;
        }
        else
        {
            var Zone = new List<int[]>();
            var TargetZone = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Zone.Add(new int[2] { i, j });
                }
            }
            for (int k = 0; k < 3; k++)
            {
                var Location = Random.Range(0, Zone.Count);
                TargetZone.Add(Zone[Location]);
                Zone.RemoveAt(Location);
            }
            ActionZone = TargetZone;
        }
    }
    /*public void Move()
    {
        ChooseActionType = 1;
        EnemySkillValue = 1;
    }*/
}
public class Doll : EnemyType
{
    public override void Setting()
    {
        Name = "©G¿£¤H³È";
        HP = 10;
        Defense = 1;
        ActionType = new int[] {0};
        UniqueValue = 0;
        ATKValue = 2;
        BodyShape = 1.5f;
        havePassiveSkill = true;
        ActionName = "©G¿£-µ²¦L";
    }
    public override void PassiveSkill(int HP)
    {
        if (HP > 0)
        {
            PlayerUIManager.GetInstance().PlayerData.BuffValue[1] -= 1;
            CoordinatesEffectAni.EnemyPassiveDeBuff = true;
            EnemyManager.GetInstance().EnemyATKZone = new List<int[]> { PlayerUIManager.GetInstance().PlayerData.PlayerLocation };
        }
        else
        {
            PlayerUIManager.GetInstance().PlayerData.BuffValue[1] += 1;
        }
    }
    public override void Action(int type, int[] location)
    {
        if (type == 0)
        {
            Skill1(location);
        }
    }
    public void Skill1(int[] location)
    {
        ActionName = "©G¿£-¶Â·t¦¬³Î";
        ChooseActionType = 0;
        var Zone = new List<int[]>();
        for (int i = 0; i < 3; i++)
        {
            Zone.Add(new int[2] { location[0], i });
        }
        ActionZone = Zone;
    }
    /*public void Move()
    {
        ChooseActionType = 1;
        EnemySkillValue = 1;
    }*/
}
public class Wise : EnemyType
{
    public override void Setting()
    {
        Name = "Ãh´µ";
        HP = 20;
        Defense = 4;
        ActionType = new int[] { 0, 1 };
        UniqueValue = 0;
        ATKValue = 4;
        BodyShape = 1.4f;
        havePassiveSkill = false;
    }
    public override void PassiveSkill(int HP)
    {

    }
    public override void Action(int type, int[] location)
    {
        if (type == 0)
        {
            Skill1(location);
        }
        if (type == 1)
        {
            Skill2(location);
        }
    }
    public void Skill1(int[] location)
    {
        ActionName = "©]¾~¨µÂ§";
        ChooseActionType = 0;
        var Zone = new List<int[]>();
        var TargetZone = new List<int[]>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Zone.Add(new int[2] { i, j });
            }
        }
        for (int k = 0; k < 4; k++)
        {
            var Location = Random.Range(0, Zone.Count);
            TargetZone.Add(Zone[Location]);
            Zone.RemoveAt(Location);
        }
        ActionZone = TargetZone;
    }
    public void Skill2(int[] location)
    {
        ActionName = "¸s¾~²±®b";
        ChooseActionType = 0;
        var Zone = new List<int[]>();
        for (int i = 0; i < 3; i++)
        {
            Zone.Add(new int[2] { 1, (i + 1) });
        }
        Zone.Add(new int[2] { 0, 2 });
        Zone.Add(new int[2] { 2, 2 });
        ActionZone = Zone;
    }
}
public class Abi : EnemyType
{
    private List<int[]> PlayerLocation;
    private List<int[]> TrapLocation;
    public override void Setting()
    {
        Name = "¦ã¤ñ";
        HP = 40;
        Defense = 5;
        ActionType = new int[] { 0,1 };
        UniqueValue = 0;
        ATKValue = 5;
        BodyShape = 1.4f;
        havePassiveSkill = false;
        PlayerLocation = new List<int[]>();
        TrapLocation = new List<int[]>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                PlayerLocation.Add(new int[2] { i, j });
            }
        }
    }
    public override void PassiveSkill(int HP)
    {
        
    }
    public override void Action(int type, int[] location)
    {
        if (type == 0)
        {
            Skill1(location);
        }
        if (type == 1)
        {
            Skill2(location);
        }
    }
    public void Skill1(int[] location)
    {
        ActionName = "®£Äß¨ã²{";
        ChooseActionType = 2;
        if (TrapLocation.Count < 12)
        {
            for (int i = 0; i < 3; i++)
            {
                var newtrap = PlayerLocation[Random.Range(0, PlayerLocation.Count)];
                TrapLocation.Add(newtrap);
                EnemyManager.GetInstance().SetTrap(newtrap);
                PlayerLocation.Remove(newtrap);
            }
        }
        ActionZone = TrapLocation;
        UniqueValue = 3;
    }
    public void Skill2(int[] location)
    {
        var Zone = new List<int[]>();
        var TargetZone = new List<int[]>();
        ActionName = "§L¼ìÃ~´²";
        ChooseActionType = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Zone.Add(new int[2] { i, j });
            }
        }
        for (int k = 0; k < 5; k++)
        {
            var Location = Random.Range(0, Zone.Count);
            TargetZone.Add(Zone[Location]);
            Zone.RemoveAt(Location);
        }
        ActionZone = TargetZone;
    }
    /*public void Move()
    {
        ChooseActionType = 1;
        UniqueValue = 1;
    }*/
}
