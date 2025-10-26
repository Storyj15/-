using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    //泛型
    public static GameManager gameManager_instance = null;

    //職業與敵人種類程式資料庫
    public Dictionary<int, JobData> Jobs = new Dictionary<int, JobData>();
    public Dictionary<int, EnemyType> Enemys = new Dictionary<int, EnemyType>();

    [SerializeField] public static float defaulVolume = 0.5f;
    //可隨時調用
    [Header("AudioSouce")]
    public GameObject audioManagerPrefab;
    public AudioManager audioManager;
    public int audioInt;
    public AudioClip[] audioClip;

    [Header("DuelSetting")]
    public float GameSpeed;
    public bool isPracticeDuel = false;
    public bool nextState = false;

    private string datapath;

    [Header("PlayerSaveData")]
    public int PlayerJob;
    public int LevelNumber;
    public int[] PlayerUpgrade = new int[3] {0,0,0};
    public int PlayerUpgradeScore;
    public int PlayerLevel;


    // Start is called before the first frame update
    private void Awake()
    {
        AudioListener.volume = defaulVolume;
        datapath = Application.persistentDataPath + "/playersave.json";
        if (gameManager_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        gameManager_instance = this;
        GameSpeed = 1;
        DontDestroyOnLoad(gameObject);
        InitializePlayerJob();
        InitializeEnemy();
        LordingSave();
    }
    private void Start()
    {
        audioManager = Instantiate(audioManagerPrefab).GetComponent<AudioManager>();
    }
    private void Update() //Cheat
    {
        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.T))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.gameManager_instance.LevelNumber = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.gameManager_instance.LevelNumber = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.gameManager_instance.LevelNumber = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.gameManager_instance.LevelNumber = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameManager.gameManager_instance.LevelNumber = 5;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameManager.gameManager_instance.LevelNumber = 6;
            }
        }
        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.P))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                GameManager.gameManager_instance.PlayerJob = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.gameManager_instance.PlayerJob = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.gameManager_instance.PlayerJob = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.gameManager_instance.PlayerJob = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.gameManager_instance.PlayerJob = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameManager.gameManager_instance.PlayerJob = 5;
            }
        }
        if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.B))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                EnemyManager.GetInstance().EnemyPiece[0].GetComponent<EnemyData>().ActionTypes = new int[] { 0 };
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EnemyManager.GetInstance().EnemyPiece[0].GetComponent<EnemyData>().ActionTypes = new int[] { 1 };
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EnemyManager.GetInstance().EnemyPiece[0].GetComponent<EnemyData>().ActionTypes = new int[] { 0,1 };
            }
        }
    }
    private void InitializePlayerJob()
    {
        Jobs.Clear();
        Jobs.Add(0, new Warrior());
        Jobs.Add(1, new Magician());
        Jobs.Add(2, new Archer());
        Jobs.Add(3,new Gambler());
        Jobs.Add(4, new Foodie());
        Jobs.Add(5, new Musician());
    }
    private void InitializeEnemy()
    {
        Enemys.Clear();
        Enemys.Add(0, new Tomato());
        Enemys.Add(1, new Doll());
        Enemys.Add(2, new Wise());
        Enemys.Add(3, new Abi());
    }
    private void CreateNewSave()
    {
        LevelNumber = 0;
        PlayerUpgrade = new int[3] { 0, 0, 0 };
        SaveData savedata = new SaveData();
        savedata.Level = LevelNumber;
        savedata.PlayerUpgrade = PlayerUpgrade;
        savedata.PlayerUpgradeScore = 500;
        savedata.PlayerLevel = 0;
        string savejson = JsonConvert.SerializeObject(savedata);
        File.WriteAllText(datapath, savejson);
        Debug.Log("Create New Save");
    }
    public void LordingSave()
    {
        if (File.Exists(datapath))
        {
            string playersave = File.ReadAllText(datapath);
            SaveData savedata = JsonConvert.DeserializeObject<SaveData>(playersave);
            PlayerJob = savedata.PlayerJobID;
            LevelNumber = savedata.Level;
            PlayerUpgrade = savedata.PlayerUpgrade;
            PlayerUpgradeScore = savedata.PlayerUpgradeScore;
            PlayerLevel = savedata.PlayerLevel;
            Debug.Log("Lording Save");
        }
        else
        {
            CreateNewSave();
        }
    }
    public void Save()
    {
        string playersave = File.ReadAllText(datapath);
        SaveData savedata = JsonConvert.DeserializeObject<SaveData>(playersave);
        savedata.Level = LevelNumber;
        savedata.PlayerUpgrade = PlayerUpgrade;
        savedata.PlayerJobID = PlayerJob;
        savedata.PlayerUpgradeScore = PlayerUpgradeScore;
        savedata.PlayerLevel = PlayerLevel;
        string savejson = JsonConvert.SerializeObject(savedata);
        File.WriteAllText(datapath, savejson);
        Debug.Log("Save");
    }
    public void StartNewGame()
    {
        PlayerJob = Random.Range(0, Jobs.Count);
        PlayerUpgrade = new int[3] { 0, 0, 0 };
        PlayerUpgradeScore = 500;
        PlayerLevel = 0;
        Debug.Log("New Game");
    }
}
public class SaveData
{
    public int Level;
    public int PlayerJobID;
    public int[] PlayerUpgrade;
    public int PlayerLevel;
    public int PlayerUpgradeScore;
}
