using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EnemyManager : Singleton<EnemyManager>
{   
    public GameObject TrapObject; 
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyPiece; //場景中敵人
    public List<int> EnemysIDs;


    public GameObject PlayerPieceTrapLocation; //玩家的區域
    public GameObject EnemyPieceLocation; //敵人區域
    public List<int[]> EnemyATKZone;

    public bool isEnemyDie;
    public bool readyToDuel;
    public bool isReady;

    public List<int[]> LocationList;
    public AudioClip[] WiseSound;
    public AudioClip[] AbiSound;
    public List<AudioClip[]> EnemyAudios;
    protected override void Awake()
    {
        base.Awake();
        EnemyAudios = new List<AudioClip[]>();
        EnemyAudios.Add(WiseSound);
        EnemyAudios.Add(AbiSound);
    }
    // Start is called before the first frame update
    void Start()
    {
        isEnemyDie = false;
        isReady = true;
        LocationList = new List<int[]>();
        ResetLocationList();
        StartCoroutine(StartDuel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetLocationList()
    {
        var list = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    list.Add(new int[] { i, j });
                }
            }
        LocationList = list;
    }
    public void SetTrap(int[] TargetLocation)
    {
        Instantiate(TrapObject, PlayerPieceTrapLocation.transform.GetChild(TargetLocation[0]).transform.GetChild(TargetLocation[1] + 4));
    }
    public void EnemyHurtSound(int enemyID)
    {
        var hurtclip = EnemyAudios[enemyID][Random.Range(0, 2)];
        GameManager.gameManager_instance.audioManager.PlayClip(2, hurtclip, false);
    }
    /*public IEnumerator EnemyMove(int enemyInt)
    {
        var SkillZoneList = new List<int[]>();
        var MoveZoneList = new List<int[]>();
        var enemylocation = EnemyPiece[enemyInt].GetComponent<EnemyData>().EnemyLocation;
        var enemymovelocation = EnemyPiece[enemyInt].GetComponent<EnemyData>().MoveToLocation;
        var enemyvalue = EnemyPiece[enemyInt].GetComponent<EnemyData>().EnemyUniqueValue;
        SkillZoneList.Add(new int[2] { 0, (enemylocation[1] - 4) });
        SkillZoneList.Add(new int[2] { 1, (enemylocation[1] - 4) });
        SkillZoneList.Add(new int[2] { 2, (enemylocation[1] - 4) });
        for (int a = 0; a < EnemyPiece[enemyInt].GetComponent<EnemyData>().EnemyUniqueValue; a++)
        {
            if (enemylocation[1] + (enemyvalue + 1) <= 7)
            {
                SkillZoneList.Add(new int[2] { enemylocation[0], enemylocation[1] + (a + 1) - 4 });
            }
            if (enemylocation[1] - (enemyvalue + 1) >= 0)
            {
                SkillZoneList.Add(new int[2] { enemylocation[0], enemylocation[1] - (a + 1) - 4 });
            }
        }
        for (int b = 0; b < SkillZoneList.Count; b++)
        {
            var MoveZone = LocationList.Any(arr => arr.GetType() == SkillZoneList[b].GetType() && arr.SequenceEqual(SkillZoneList[b]));
            if (MoveZone)
            {
                MoveZoneList.Add(SkillZoneList[b]);
            }
        }
        enemymovelocation = MoveZoneList[Random.Range(0, MoveZoneList.Count)];
        LocationList.Add(new int[] {enemylocation[0],enemylocation[1] - 4 });
        EnemyPiece[enemyInt].transform.parent = EnemyPieceLocation.transform.GetChild(enemymovelocation[0]).transform.GetChild(enemymovelocation[1]);
        EnemyPiece[enemyInt].transform.DOMove(EnemyPieceLocation.transform.GetChild(enemymovelocation[0]).transform.GetChild(enemymovelocation[1]).transform.position, 0.5f);
        EnemyPiece[enemyInt].GetComponent<EnemyData>().EnemyLocation[0] = enemymovelocation[0];
        EnemyPiece[enemyInt].GetComponent<EnemyData>().EnemyLocation[1] = (enemymovelocation[1] + 4);
        LocationList.Remove(enemymovelocation);
        yield return null;
    }*/
    public IEnumerator EnemysHurt()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        for (int i = 0; i < EnemyPiece.Count; i++)
        {
            var hurt = _player_data.ATKHitZone.Any(arr => arr.GetType() == EnemyPiece[i].GetComponent<EnemyData>().EnemyLocation.GetType() && arr.SequenceEqual(EnemyPiece[i].GetComponent<EnemyData>().EnemyLocation));
            if (hurt)
            {
                if (GameManager.gameManager_instance.LevelNumber >= 5 && EnemyPiece[i].GetComponent<EnemyData>().EnemyID >= 2)
                {
                    EnemyHurtSound(EnemyPiece[i].GetComponent<EnemyData>().EnemyID - 2);
                }
                EnemyPiece[i].transform.DOPunchPosition(new Vector3(10,0,0),0.5f,10);
                if (EnemyPiece[i].GetComponent<EnemyData>().Defense > 0 && EnemyPiece[i].GetComponent<EnemyData>().Defense >= ((_player_data.ATKValue + _player_data.BuffValue[1]) * _player_data.PlayerAction[1]))
                {
                    EnemyPiece[i].GetComponent<EnemyData>().Defense -= ((_player_data.ATKValue + _player_data.BuffValue[1]) * _player_data.PlayerAction[1]);
                }
                else
                {
                    EnemyPiece[i].GetComponent<EnemyData>().HP -= (((_player_data.ATKValue + _player_data.BuffValue[1]) * _player_data.PlayerAction[1]) - EnemyPiece[i].GetComponent<EnemyData>().Defense);
                    EnemyPiece[i].GetComponent<EnemyData>().Defense = 0;
                }
            }
            if (EnemyPiece[i].GetComponent<EnemyData>().HP <= 0)
            {
                EnemyPiece[i].GetComponent<EnemyData>().PassiveSkill();
            }
        }
        yield return new WaitForSeconds(0.6f);
        EnemyPiece.RemoveAll(x => x.GetComponent<EnemyData>().HP <= 0);
        isEnemyDie = true;
        yield return new WaitForSeconds(0.4f);
        if (EnemyPiece.Count == 0)
        {
            yield return StartCoroutine(PlayerWin());
            yield return null;
        }
    }
    public IEnumerator EnemysSkillHurt()
    {
        var _player_data = PlayerUIManager.GetInstance().PlayerData;
        for (int i = 0; i < EnemyPiece.Count; i++)
        {
            var hurt = _player_data.ATKHitZone.Any(arr => arr.GetType() == EnemyPiece[i].GetComponent<EnemyData>().EnemyLocation.GetType() && arr.SequenceEqual(EnemyPiece[i].GetComponent<EnemyData>().EnemyLocation));
            if (hurt)
            {
                if (GameManager.gameManager_instance.LevelNumber >= 5 && EnemyPiece[i].GetComponent<EnemyData>().EnemyID >= 2)
                {
                    EnemyHurtSound(EnemyPiece[i].GetComponent<EnemyData>().EnemyID - 2);
                }
                EnemyPiece[i].transform.DOPunchPosition(new Vector3(15, 0, 0), 0.5f, 10);
                if (EnemyPiece[i].GetComponent<EnemyData>().Defense > 0 && EnemyPiece[i].GetComponent<EnemyData>().Defense >= (_player_data.SkillValue * _player_data.PlayerAction[2]))
                {
                    EnemyPiece[i].GetComponent<EnemyData>().Defense -= (_player_data.SkillValue * _player_data.PlayerAction[2]);
                }
                else
                {
                    EnemyPiece[i].GetComponent<EnemyData>().HP -= ((_player_data.SkillValue * _player_data.PlayerAction[2]) - EnemyPiece[i].GetComponent<EnemyData>().Defense);
                    EnemyPiece[i].GetComponent<EnemyData>().Defense = 0;
                }
            }
            if (EnemyPiece[i].GetComponent<EnemyData>().HP <= 0)
            {
                EnemyPiece[i].GetComponent<EnemyData>().PassiveSkill();
            }
        }
        yield return new WaitForSeconds(0.6f);
        EnemyPiece.RemoveAll(x => x.GetComponent<EnemyData>().HP <= 0);
        isEnemyDie = true;
        yield return new WaitForSeconds(0.4f);
        if (EnemyPiece.Count == 0)
        {
            yield return StartCoroutine(PlayerWin());
        }
    }
    public IEnumerator StartDuel()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(EnemysIDsSetting());
        var enemys = EnemysIDs.Count;
        if (!GameManager.gameManager_instance.isPracticeDuel)
        {
            if (GameManager.gameManager_instance.LevelNumber >= 5)
            {
                var _bossPrefab = Instantiate(EnemyPrefab, EnemyPieceLocation.transform);
                var bosslocation = new int[] { 0, 1, 4, 5, 8, 9 };
                var target = Random.Range(0, bosslocation.Length);
                _bossPrefab.GetComponent<EnemyData>().EnemyLocation = LocationList[bosslocation[target]];
                LocationList.Remove(LocationList[bosslocation[target]]);
                _bossPrefab.transform.parent = EnemyPieceLocation.transform.GetChild(_bossPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_bossPrefab.GetComponent<EnemyData>().EnemyLocation[1]);
                _bossPrefab.transform.position = EnemyPieceLocation.transform.GetChild(_bossPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_bossPrefab.GetComponent<EnemyData>().EnemyLocation[1]).transform.position;
                _bossPrefab.GetComponent<EnemyData>().EnemyLocation[1] += 4;
                EnemyPiece.Add(_bossPrefab);
                yield return null;
                Debug.Log(LocationList.Count);
                for (int i = 0; i < (enemys - 1); i++)
                {
                    var _enemyPrefab = Instantiate(EnemyPrefab, EnemyPieceLocation.transform);
                    var enemylocation = Random.Range(0, LocationList.Count);
                    _enemyPrefab.GetComponent<EnemyData>().EnemyLocation = LocationList[enemylocation];
                    LocationList.Remove(LocationList[enemylocation]);
                    _enemyPrefab.transform.parent = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]);
                    _enemyPrefab.transform.position = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]).transform.position;
                    _enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1] += 4;
                    EnemyPiece.Add(_enemyPrefab);
                    yield return null;
                }
            }
            else
            {
                for (int i = 0; i < enemys; i++)
                {
                    var _enemyPrefab = Instantiate(EnemyPrefab, EnemyPieceLocation.transform);
                    var enemylocation = Random.Range(0, LocationList.Count);
                    _enemyPrefab.GetComponent<EnemyData>().EnemyLocation = LocationList[enemylocation];
                    LocationList.Remove(LocationList[enemylocation]);
                    _enemyPrefab.transform.parent = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]);
                    _enemyPrefab.transform.position = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]).transform.position;
                    _enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1] += 4;
                    EnemyPiece.Add(_enemyPrefab);
                    yield return null;
                }
            }
        }
        else
        {
            for (int i = 0; i < enemys; i++)
            {
                var _enemyPrefab = Instantiate(EnemyPrefab, EnemyPieceLocation.transform);
                _enemyPrefab.GetComponent<EnemyData>().EnemyLocation = new int[] {1,2};
                _enemyPrefab.transform.parent = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]);
                _enemyPrefab.transform.position = EnemyPieceLocation.transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[0]).transform.GetChild(_enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1]).transform.position;
                _enemyPrefab.GetComponent<EnemyData>().EnemyLocation[1] += 4;
                EnemyPiece.Add(_enemyPrefab);
                yield return null;
            }
        }
        readyToDuel = true;
    }
    public IEnumerator EnemysIDsSetting()
    {
        var enemy = new List<int>();
        if (GameManager.gameManager_instance.LevelNumber == 0)
        {
            var enemystype = new int[] { 0 };
            enemy.Add(enemystype[0]);
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 1)
        {
            var enemystype = new int[] { 0 };
            for (int i = 0; i < 2; i++)
            {
                var enemyint = Random.Range(0, enemystype.Length);
                enemy.Add(enemystype[enemyint]);
            }
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 2)
        {
            var enemystype = new int[] { 0 };
            for (int i = 0; i < 3; i++)
            {
                var enemyint = Random.Range(0, enemystype.Length);
                enemy.Add(enemystype[enemyint]);
            }
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 3)
        {
            var enemystype = new int[] { 0, 1 };
            for (int i = 0; i < (3 - 1); i++)
            {
                enemy.Add(enemystype[0]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[1]);
            }
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 4)
        {
            var enemystype = new int[] { 0, 1 };
            for (int i = 0; i < (4 - 1); i++)
            {
                enemy.Add(enemystype[0]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[1]);
            }
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 5)
        {
            var enemystype = new int[] { 0, 1 ,2 };
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[2]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[0]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[1]);
            }
            EnemysIDs = enemy;
        }
        else if (GameManager.gameManager_instance.LevelNumber == 6)
        {
            var enemystype = new int[] { 0, 1, 3 };
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[2]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[0]);
            }
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(enemystype[1]);
            }
            EnemysIDs = enemy;
        }
        yield return null;
    }
    public IEnumerator PlayerWin()
    {
        DuelUIManager.isPlayerWin = true;
        DuelUIManager.BattleEnd = true;
        yield return 0;
    }
}
