using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    public GameObject DuelUIEnd;
    public GameObject BattleStateManager;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.W))
        {
            PlayerUIManager.GetInstance().isDuelEnd = true;
            DuelUIManager.isPlayerWin = true;
            DuelUIManager.BattleEnd = true;
            DuelUIEnd.SetActive(true);
            BattleStateManager.SetActive(false);
        }
        if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.L))
        {
            PlayerUIManager.GetInstance().isDuelEnd = true;
            DuelUIManager.isPlayerWin = false;
            DuelUIManager.BattleEnd = true;
            DuelUIEnd.SetActive(true);
            BattleStateManager.SetActive(false);
        }
    }
}
