using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CoordinatesLocation : MonoBehaviour,IPointerDownHandler
{
    public int[] Coordinates = new int[2];

    //public bool alreadyStop; //已經有東西停留
    public bool isPlayerLocation; //屬於玩家的移動區
    
    private bool canStop; //此格可停留 
    private bool isPlayerATKZone; //在攻擊範圍內
    private Button button;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        canStop = false;
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.enabled = false;
        button.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Move)
        {
            if (LocationManager.showEnemyATKZone)
            {
                canStop = false;
                var atkzone = EnemyManager.GetInstance().EnemyATKZone.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
                if (atkzone)
                {
                    image.color = new Color32(255, 0, 0,255);
                    image.enabled = true;
                }
                else
                {
                    image.enabled = false;
                }
            }
            else
            {
                canStop = true;
                if (canStop && PlayerUIManager.GetInstance().PlayerData.canMove && !PlayerUIManager.GetInstance().PlayerData.isReady && isPlayerLocation && PlayerUIManager.GetInstance().PlayerData.MoveValue >= (Mathf.Abs(PlayerUIManager.GetInstance().PlayerData.PlayerLocation[0] - Coordinates[0]) + Mathf.Abs(PlayerUIManager.GetInstance().PlayerData.PlayerLocation[1] - Coordinates[1])))
                {
                    image.color = new Color32(0, 235, 255,255);
                    button.enabled = true;
                    image.enabled = true;
                }
                else
                {
                    button.enabled = false;
                    image.enabled = false;
                }
            }
        }
        else
        {
            canStop = false;
        }
        if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack)
        {
            var playerPhysicATKZone = PlayerUIManager.GetInstance().PlayerData.ATKZone;
            var playerLocation = PlayerUIManager.GetInstance().PlayerData.PlayerLocation;
            if (LocationManager.showPlayerATKZone)
            {
                if (PlayerUIManager.GetInstance().PlayerData.isNormalATKZones)
                {
                    if (playerLocation[0] == Coordinates[0] && (Coordinates[1] - playerLocation[1]) <= playerPhysicATKZone && (Coordinates[1] - playerLocation[1]) >= 0 && (playerLocation[1] != Coordinates[1]))
                    {
                        isPlayerATKZone = true;
                    }
                    else
                    {
                        isPlayerATKZone = false;
                    }
                }
                else if (!PlayerUIManager.GetInstance().PlayerData.isNormalATKZones)
                {
                    var canatkhit = PlayerUIManager.GetInstance().PlayerData.SpecialNormalZone.Any(arr => arr.GetType() == Coordinates.GetType() && arr.SequenceEqual(Coordinates));
                    if (canatkhit)
                    {
                        isPlayerATKZone = true;
                    }
                    else
                    {
                        isPlayerATKZone = false;
                    }
                }
            }
            else
            {
                isPlayerATKZone = false;
            }

            if (isPlayerATKZone)
            {
                image.color = new Color32(255, 0, 0, 255);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        MovePointClick();
    }
    public void MovePointClick()
    {
        if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Move && canStop && isPlayerLocation)
        {
            PlayerAnimator.isMove = true;
            PlayerUIManager.GetInstance().PlayerData.MoveToLocation = Coordinates;
            PlayerUIManager.GetInstance().PlayerData.isReady = true;
        }
    }
}
