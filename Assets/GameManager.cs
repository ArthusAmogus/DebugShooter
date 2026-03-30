using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public class MapSave
{
    public Vector3 Player;
    public int Stage;
    public bool[] MapParent   = new bool[25];
    public bool[] MapVariant1 = new bool[25];
    public bool[] MapVariant2 = new bool[25];
    public bool[] MapVariant3 = new bool[25];

    public List<Vector3> EnemyPositions = new();
}

public class GameManager : MonoBehaviour
{
    public MapSave mapSave;
    [SerializeField] Transform Player;
    public List<GameObject> EnemiesAlive = new();
    public List<GameObject> DroppedItems = new();
    public GameObject[] EnemyTypes;
    [SerializeField] TextMeshProUGUI EnemyCounter;
    [SerializeField] TextMeshProUGUI StageCounter;
    [SerializeField] MapGenerator MapGenerator;
    [SerializeField] ScreenMessageEffect ScreenMessage;
    [SerializeField] GameObject StageDisplay;
    public int Stage;
    bool stageRunning;

    [Header("Pause Properties")]
    [SerializeField] GameObject PausePanel;
    [SerializeField] bool isPaused;
    [SerializeField] bool noWinCondition;


    public static GameManager Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Update()
    {
        EnemyCounter.text = "Enemies alive: " + EnemiesAlive.Count;
        StageCounter.text = "Stage: " + Stage;
    

        if (stageRunning && EnemiesAlive.Count==0 && !noWinCondition)
        {
            StartCoroutine(StageComplete());
            stageRunning = false;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        PausePanel.SetActive(isPaused);
        RotationWithMouse.Instance.enabled = !isPaused;
        FPShooterSystem.Instance.enabled = !isPaused;
        InventoryController.Instance.enabled = !isPaused;
        ReloadingSystem.Instance.enabled = !isPaused;
    }

    public void QuitGame(bool save)
    {
        if (save)
        {
            SaveMap();
        }

        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        Time.timeScale = 1;
        CurtainBehavior.Instance.CloseCurtain();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveMap()
    {
        mapSave = new MapSave();

        mapSave.Player = Player.position;
        mapSave.Stage = Stage;

        mapSave.MapParent   = new bool[25];
        mapSave.MapVariant1 = new bool[25];
        mapSave.MapVariant2 = new bool[25];
        mapSave.MapVariant3 = new bool[25];

        for (int i = 0; i < MapGenerator.Instance.mapGrid.Length; i++)
        {
            mapSave.MapParent  [i] = MapGenerator.Instance.mapGrid[i].gameObject.activeSelf;
            mapSave.MapVariant1[i] = MapGenerator.Instance.mapGrid[i].transform.GetChild(0).gameObject.activeSelf;
            mapSave.MapVariant2[i] = MapGenerator.Instance.mapGrid[i].transform.GetChild(1).gameObject.activeSelf;
            mapSave.MapVariant3[i] = MapGenerator.Instance.mapGrid[i].transform.GetChild(2).gameObject.activeSelf;
        }


        foreach (GameObject enemy in EnemiesAlive)
        {
            mapSave.EnemyPositions.Add(enemy.transform.position);
        }

        InventoryManager.Instance.SaveInventory();
        UserDataObject.Instance.userData.mapSave = mapSave;
        UserDataObject.Instance.userData.hasUnfinishedSession = true;
        UserDataObject.Instance.SaveData();

    }

    public void LoadMap()
    {
        mapSave = UserDataObject.Instance.userData.mapSave;
        InventoryManager.Instance.LoadInventory();

        StartCoroutine(ResetTransform(mapSave.Player));
        Stage = mapSave.Stage;
        MapGenerator.Instance.navMeshSurface.RemoveData();
        for (int i = 0; i < MapGenerator.Instance.mapGrid.Length; i++)
        {
            MapGenerator.Instance.mapGrid[i].gameObject.SetActive(mapSave.MapParent[i]);
            MapGenerator.Instance.mapGrid[i].transform.GetChild(0).gameObject.SetActive(mapSave.MapVariant1[i]);
            MapGenerator.Instance.mapGrid[i].transform.GetChild(1).gameObject.SetActive(mapSave.MapVariant2[i]);
            MapGenerator.Instance.mapGrid[i].transform.GetChild(2).gameObject.SetActive(mapSave.MapVariant3[i]);
        }
        MapGenerator.Instance.navMeshSurface.BuildNavMesh();

        foreach (Vector3 pos in mapSave.EnemyPositions)
        {
            int randomIndex = Random.Range(0, EnemyTypes.Length);
            int randomHealth = Random.Range(30, 151 * Stage);
            GameObject enemy = Instantiate(EnemyTypes[randomIndex], pos, Quaternion.identity);
            enemy.GetComponent<StatsSystem>().MaxHP = randomHealth;
            GameManager.Instance.EnemiesAlive.Add(enemy);
        }

    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        CurtainBehavior.Instance.OpenCurtain();
        yield return new WaitForSeconds(0.4f);

        if (UserDataObject.Instance.userData.hasUnfinishedSession)
        {
            Debug.Log("CONTINUE GAME");
            LoadMap();
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Debug.Log("NEW GAME");
            MapGenerator.GenerateMap();
            MapGenerator.SpawnEnemies();
            Stage++;
        }

        stageRunning = true;
        yield return new WaitForSeconds(2f);
        StageDisplay.SetActive(true);
        StageDisplay.GetComponent<StageDisplayEffect>().PlayEffect();
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        PlayerMovement3D.Instance.enabled = false;
        FPShooterSystem.Instance.enabled = false;
        RotationWithMouse.Instance.enabled = false;
        InventoryController.Instance.enabled = false;
        ReloadingSystem.Instance.enabled = false;
        Instance.enabled = false;
        ScreenMessage.message = "GAME OVER";
        ScreenMessage.showTime = 3;
        ScreenMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.5f);

        CurtainBehavior.Instance.CloseCurtain();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void StartCompleteStage()
    {
        StartCoroutine(StageComplete());
    }

    public void StartResetTransform()
    {
        StartCoroutine(ResetTransform(new Vector3(0, 10, 0)));
    }

    IEnumerator ResetTransform(Vector3 pos)
    {
        PlayerMovement3D.Instance.rgbody.isKinematic = true;
        PlayerMovement3D.Instance.rgbody.interpolation = RigidbodyInterpolation.None;
        Player.position = pos;
        yield return new WaitForSeconds(0.2f);
        PlayerMovement3D.Instance.rgbody.isKinematic = false;
        PlayerMovement3D.Instance.rgbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    IEnumerator StageComplete()
    {
        ScreenMessage.message = "STAGE COMPLETE";
        ScreenMessage.showTime = 1f;
        ScreenMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        CurtainBehavior.Instance.CloseCurtain();
        yield return new WaitForSeconds(1f);

        StartCoroutine(ResetTransform(new Vector3(0, 10, 0)));
        yield return new WaitForSeconds(0.5f);

        MapGenerator.ClearMap();
        MapGenerator.ClearMap();
        MapGenerator.GenerateMap();
        MapGenerator.SpawnEnemies();
        Stage++;

        yield return new WaitForSeconds(0.5f);

        CurtainBehavior.Instance.OpenCurtain();
        yield return new WaitForSeconds(0.4f);
        
        yield return new WaitForSeconds(1f);
        StageDisplay.SetActive(true);
        StageDisplay.GetComponent<StageDisplayEffect>().PlayEffect();
        stageRunning = true;
    }

}
