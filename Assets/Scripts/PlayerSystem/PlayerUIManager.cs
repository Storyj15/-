using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerUIManager : Singleton<PlayerUIManager>
{
    public GameObject PlayerPiece;
    public GameObject PlayerPieceLocation;

    public AudioClip[] audioClips;
    public AudioClip[] PlayerATKClips;
    public AudioClip[] PlayerHurtClips;

    public int HandCardAmount;

    public bool isDuelEnd = false;
    public bool readyToDuel;
    public bool isFirstATK;
    
    public GameObject PlayerHandCardZone;
    public GameObject PlayerDeck;
    public GameObject PlayerState;
    public PlayerDataManager PlayerData;
    
    public GameObject PlayerPrefab;
    public GameObject CardPrefab;

    private List<int[]> PracticeDuelCard = new List<int[]>();
    public int PracticeType1;
    public int PracticeType2;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        isDuelEnd = false;
        if (GameManager.gameManager_instance.isPracticeDuel)
        {
            PracticeDuelCard.Add(new int[] { 0, 1 });
            PracticeDuelCard.Add(new int[] { 1, 0 });
            PracticeDuelCard.Add(new int[] { 2, 1 });
            PracticeDuelCard.Add(new int[] { 0, 0 });
        }
        PlayerData = new PlayerDataManager();
        PlayerData.MoveToLocation[0] = 1;
        PlayerData.MoveToLocation[1] = 1;
        PlayerData.SettingValue();
        PlayerHandCardZone.SetActive(false);
        PlayerDeck.SetActive(false);  
        StartCoroutine(StartDuel());
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.isReady && (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Move || DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Attack))
        {
            PlayerData.playerStateMode = NewGameState.NewPlayerStateMode.PlayerReady;
        }
        HandCardAmount = PlayerHandCardZone.transform.childCount;
        GameManager.gameManager_instance.Jobs[PlayerData.JobIndex].SpecialProcess();
        if (isDuelEnd)
        {
            PlayerState.SetActive(false);
        }
    }
    public void MovePiece()
    {
        PlayerPiece.transform.parent = PlayerPieceLocation.transform.GetChild(PlayerData.MoveToLocation[0]).transform.GetChild(PlayerData.MoveToLocation[1]);
        //PlayerPiece.transform.DOMove(PlayerPieceLocation.transform.GetChild(PlayerData.MoveToLocation[0]).transform.GetChild(PlayerData.MoveToLocation[1]).transform.position, 1);
        PlayerPiece.transform.position = PlayerPieceLocation.transform.GetChild(PlayerData.MoveToLocation[0]).transform.GetChild(PlayerData.MoveToLocation[1]).transform.position;
        PlayerData.PlayerLocation = PlayerData.MoveToLocation;
    }
    public void PlayerAttackReady()
    {
        PlayerData.isReady = true;
    }
    public void PlayerAttackSound()
    {
        var attackclip = PlayerATKClips[Random.Range(0, PlayerATKClips.Length)];
        GameManager.gameManager_instance.audioManager.PlayClip(2, attackclip, false);
    }
    public void PlayerHurtSound()
    {
        var hurtclip = PlayerHurtClips[Random.Range(0, PlayerHurtClips.Length)];
        GameManager.gameManager_instance.audioManager.PlayClip(2, hurtclip, false);
    }
    public virtual IEnumerator AttackReady()
    {
        var targetlist = new List<Transform>();
        for (int i = 0; i < PlayerHandCardZone.transform.childCount; i++)
        {
            var targetcard = PlayerHandCardZone.transform.GetChild(i);
            if (targetcard.GetComponent<NewCardValueManager>().usecard == true)
            {
                targetcard.GetComponent<NewCardValueManager>().resultCard = true;
                targetlist.Add(targetcard);
                targetcard.transform.DOMove(Camera.main.WorldToScreenPoint(PlayerPiece.transform.position),0.5f);
                targetcard.transform.DOScale(0f, 1f);
                yield return new WaitForSeconds(0.4f);
                GameManager.gameManager_instance.audioManager.PlayClip(2, audioClips[1], false);
                yield return new WaitForSeconds(0.1f);
                targetcard.gameObject.SetActive(false);
                if (targetcard.GetComponent<NewCardValueManager>().MainType == 0)
                {
                    PlayerData.Defense += (1 + PlayerData.BuffValue[0]);
                }
                PlayerData.PlayerAction[targetcard.GetComponent<NewCardValueManager>().MainType] += 1;
            }
        }
        for (int j = 0; j < targetlist.Count; j++)
        {
            Destroy(targetlist[j].gameObject);
        }
        yield return new WaitForSeconds(0.5f);
        targetlist.Clear();
        PlayerData.PlayerAction[0] = 0;
        yield return null;
    }
    public void NormalDrawCard()
    {
        StartCoroutine(NormalDraw());
    }
    public virtual IEnumerator NormalDraw()
    {
        if (PlayerHandCardZone.transform.childCount >= 5)
        {
            DuelUIManager.showInformationText = true;
            DuelUIManager.Information = "手牌達到上限不抽牌";
            yield return new WaitForSeconds(1f);
            DuelUIManager.showInformationText = false;
            PlayerData.isReady = true;
        }
        else
        {
            for (int i = 0; i < PlayerData.DrawAmount; i++)
            {
                if (GameManager.gameManager_instance.isPracticeDuel)
                {
                    PracticeType1 = PracticeDuelCard[0][0];
                    PracticeType2 = PracticeDuelCard[0][1];
                    PracticeDuelCard.RemoveAt(0);
                }
                var newcard = Instantiate(CardPrefab, PlayerDeck.transform);
                GameManager.gameManager_instance.audioManager.PlayClip(2, audioClips[0], false);
                newcard.transform.DOMove(PlayerHandCardZone.transform.position, 0.25f);
                yield return new WaitForSeconds(0.25f);
                newcard.transform.parent = PlayerHandCardZone.transform; //卡片變成手牌子物件
                PlayerHandCardZone.transform.GetChild(index: PlayerHandCardZone.transform.childCount - 1).GetComponent<NewCardTurnTopOrBottom>().CardStartTop();
                yield return 0;
            }
            if (DuelBattleManager.duelStateMode == NewGameState.NewDuelStateMode.Draw)
            {
                yield return new WaitForSeconds(1f);
                PlayerData.isReady = true;
            }
        }
        PlayerData.DrawAmount = 0;
        yield return 0;
    }
    public virtual IEnumerator StartDuel()
    {
        DuelBattleManager.duelStateMode = NewGameState.NewDuelStateMode.Draw;
        yield return new WaitForSeconds(1f);
        PlayerDeck.SetActive(true);
        PlayerState.SetActive(true);
        PlayerHandCardZone.SetActive(true);
        yield return new WaitForSeconds(1f);
        PlayerPiece = Instantiate(PlayerPrefab, PlayerPieceLocation.transform.GetChild(PlayerData.MoveToLocation[0]).transform.GetChild(PlayerData.MoveToLocation[1])); 
        PlayerData.PlayerLocation = PlayerData.MoveToLocation;
        PlayerData.DrawAmount = 3;
        /*if (GameManager.gameManager_instance.ObstacleLocation != null)
        {
            for (int j = 0; j < GameManager.gameManager_instance.ObstacleLocation.Count; j++)
            {
                Instantiate(Obstacle, PlayerPieceLocation.transform.GetChild(GameManager.gameManager_instance.ObstacleLocation[j][0]).transform.GetChild(GameManager.gameManager_instance.ObstacleLocation[j][1]));
            }
        }*/
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(NormalDraw());
        readyToDuel = true;
        yield return 0;
        if (GameManager.gameManager_instance.isPracticeDuel && !GameManager.gameManager_instance.nextState)
        {
            DialogueUIController.openDialogueUI = true;
        }
    }
}
