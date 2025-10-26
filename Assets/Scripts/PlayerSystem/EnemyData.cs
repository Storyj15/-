using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyData : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI DefenseText;
    public GameObject EnemyImage;
    
    //AnimationPart
    public Animator animator;
    public AnimatorOverrideController[] animatorOverride;
    public bool isdie;

    //基礎數值
    public float BodyShape;
    public int EnemyID;
    public int HP;
    public int Defense;
    public int ATKValue;
    public List<int[]> ActionZone;
    public int[] ActionTypes;

    //public int[] ThisLocation; //該所在位置
    public int[] EnemyLocation; //敵人所在位置
    public int AllDamaged; //受傷害總數值
    public string ActionNameText;
    public int ChooseActionType;
    public bool havePassiveSkill;

    //特殊數值
    public int EnemyUniqueValue;
    public int[] MoveToLocation = new int[2]; //將要移動到的位置
    public void Start()
    {
        isdie = false;
        SettingValue();
        animator = EnemyImage.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorOverride[EnemyID];
    }
    void Update()
    {
        if (HP < 0)
        {
            HPText.text = "0";

        }
        else
        {
            HPText.text = HP.ToString();
        }
        if (Defense < 0)
        {
            DefenseText.text = "0";
        }
        else
        {
           DefenseText.text = Defense.ToString();
        }
        if (EnemyManager.GetInstance().isEnemyDie && HP <= 0)
        {
            isdie = true;
        }
        if (isdie)
        {
            isdie = false;
            animator.SetBool("Die", true);
            Invoke(nameof(Die), 0.4f);
        }
    }
    public void SettingValue()
    {
        var EnemySetting = GameManager.gameManager_instance.Enemys;
        EnemyID = EnemyManager.GetInstance().EnemysIDs[0];
        EnemyManager.GetInstance().EnemysIDs.RemoveAt(0);
        EnemySetting[EnemyID].Setting();
        gameObject.name = EnemySetting[EnemyID].Name;
        HP = EnemySetting[EnemyID].HP;
        Defense = EnemySetting[EnemyID].Defense;
        ATKValue = EnemySetting[EnemyID].ATKValue;
        BodyShape = EnemySetting[EnemyID].BodyShape;
        EnemyImage.GetComponent<RectTransform>().localScale = new Vector2(BodyShape,BodyShape);
        havePassiveSkill = EnemySetting[EnemyID].havePassiveSkill;
        ActionNameText = EnemySetting[EnemyID].ActionName;
        ActionTypes = EnemySetting[EnemyID].ActionType;
    }
    public void PassiveSkill()
    {
        var EnemySetting = GameManager.gameManager_instance.Enemys;
        EnemySetting[EnemyID].PassiveSkill(HP);
    }
    public void EnemyAction()
    {
        var EnemySetting = GameManager.gameManager_instance.Enemys;
        var actiontype = ActionTypes[Random.Range(0, ActionTypes.Length)];
        EnemySetting[EnemyID].Action(actiontype,EnemyLocation);
        ActionNameText = EnemySetting[EnemyID].ActionName;
        ChooseActionType = EnemySetting[EnemyID].ChooseActionType;
        EnemyUniqueValue = EnemySetting[EnemyID].UniqueValue;
        ActionZone = EnemySetting[EnemyID].ActionZone;
        EnemyManager.GetInstance().EnemyATKZone = ActionZone;
    }
    public void Die()
    {
        animator.SetBool("Die", false);
        Destroy(gameObject);
    }
}
