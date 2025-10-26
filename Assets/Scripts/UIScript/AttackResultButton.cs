using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackResultButton : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }
    // Update is called once per frame
    void Update()
    {
        var Player = PlayerUIManager.GetInstance().PlayerData;
        if (Player.playerStateMode == NewGameState.NewPlayerStateMode.PlayerActivate)
        {
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack)
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
