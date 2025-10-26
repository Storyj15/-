using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
    private List<List<GameObject>> AllLocations;
    private List<List<GameObject>> PlayerLocations;
    private List<List<GameObject>> EnemyLocations;
    private List<List<GameObject>> AllLocationsEffect;
    private List<GameObject> Locations;

    public GameObject PlayerPieceLocation;
    public GameObject EnemyPieceLocation;

    public static bool showPlayerATKZone;
    public static bool showPlayerSkillZone;
    public static bool showEnemyATKZone;
    public static bool showEnemyPassiveSkillZone;
    public static bool actionResult;
    public static int  ActionType;

    // Start is called before the first frame update
    void Start()
    {
        showPlayerSkillZone = false;
        showPlayerATKZone = false;
        showEnemyATKZone = false;
        showEnemyPassiveSkillZone = false;
        actionResult = false;
        AllLocations = new List<List<GameObject>>();
        AllLocationsEffect = new List<List<GameObject>>();
        PlayerLocations = new List<List<GameObject>>();
        EnemyLocations = new List<List<GameObject>>();
        for (int i = 0; i < PlayerPieceLocation.transform.childCount; i++)
        {
            Locations = new List<GameObject>();
            for (int j = 0; j < 4; j++)
            {
                Locations.Add(PlayerPieceLocation.transform.GetChild(i).transform.GetChild(j).gameObject);
            }
            PlayerLocations.Add(Locations);
            AllLocations.Add(Locations);
        }
        for (int k = 0; k < EnemyPieceLocation.transform.childCount; k++)
        {
            Locations = new List<GameObject>();
            for (int l = 0; l < 4; l++)
            {
                Locations.Add(EnemyPieceLocation.transform.GetChild(k).transform.GetChild(l).gameObject);
            }
            EnemyLocations.Add(Locations);
            AllLocations.Add(Locations);
        }
        for (int m = 0; m < PlayerPieceLocation.transform.childCount; m++)
        {
            Locations = new List<GameObject>();
            for (int n = 0; n < 4; n++)
            {
                Locations.Add(PlayerPieceLocation.transform.GetChild(m).transform.GetChild(n + 4).gameObject);
            }
            AllLocationsEffect.Add(Locations);
        }
        for (int o = 0; o < EnemyPieceLocation.transform.childCount; o++)
        {
            Locations = new List<GameObject>();
            for (int p = 0; p < 4; p++)
            {
                Locations.Add(EnemyPieceLocation.transform.GetChild(o).transform.GetChild(p + 4).gameObject);
            }
            AllLocationsEffect.Add(Locations);
        }
        for (int a = 0; a < AllLocations.Count; a++)
        {
            for (int b = 0; b < AllLocations[a].Count; b++)
            {
                if (a > 2)
                {
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().Coordinates[0] = a - 3;
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().Coordinates[1] = b + 4;
                }
                else
                {
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().Coordinates[0] = a;
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().Coordinates[1] = b;
                }
                if (AllLocations[a][b].GetComponent<CoordinatesLocation>().Coordinates[1] < 4)
                {
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().isPlayerLocation = true;
                }
                else
                {
                    AllLocations[a][b].GetComponent<CoordinatesLocation>().isPlayerLocation = false;
                }
            }
        }
        for (int c = 0; c < AllLocationsEffect.Count; c++)
        {
            for (int d = 0; d < AllLocationsEffect[c].Count; d++)
            {
                if (c > 2)
                {
                    AllLocationsEffect[c][d].GetComponent<CoordinatesEffectAni>().Coordinates[0] = c - 3;
                    AllLocationsEffect[c][d].GetComponent<CoordinatesEffectAni>().Coordinates[1] = d + 4;
                }
                else
                {
                    AllLocationsEffect[c][d].GetComponent<CoordinatesEffectAni>().Coordinates[0] = c;
                    AllLocationsEffect[c][d].GetComponent<CoordinatesEffectAni>().Coordinates[1] = d;
                }
            }
        }
    }
}
