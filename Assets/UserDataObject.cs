
using Firebase.Database;
using Google.MiniJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class UserData
{
    public string username;
    public string password;
    public int id;
    public string dateAccountCreated;
    public List<InventorySlotData> inventorySlotData = new();
    public MapSave mapSave;
    public bool hasUnfinishedSession;
}

public class UserDataObject : MonoBehaviour
{
    public UserData userData;

    public static UserDataObject Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Awake()
    {
        LoadData();
    }

    public async void SaveData()
    {
        PlayerPrefs.SetString("LastUser", userData.username);
        string json = JsonUtility.ToJson(userData, true);

        //Firebase Approach
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            await FirebaseDatabase.DefaultInstance.RootReference.Child("DebugTheGame").Child("Players").Child(userData.username).SetRawJsonValueAsync(json);
        }

        //File Approach
        File.WriteAllText(Application.persistentDataPath + "/userdata.json", json);
    }

    public async void LoadData()
    {
        //Firebase Approach
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.RootReference
                .Child("DebugTheGame").Child("Players")
                .Child(PlayerPrefs.GetString("LastUser", "Player")).GetValueAsync();

            UserData data = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
            bool exists = snapshot.Exists;
            if (exists)
            {
                userData = data;
                SaveData();
            }
        }

        //File Approach
        string path = Application.persistentDataPath + "/userdata.json";
        if (!File.Exists(path))
        {
            Debug.Log("No save found!");
            return;
        }
        string json = File.ReadAllText(path);
        userData = JsonUtility.FromJson<UserData>(json);
        SaveData();
    }

    public void CheckMapData()
    {
        for (int i = 0; i < MapGenerator.Instance.mapGrid.Length; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                //Debug.Log("["+i+","+j+"]: "+ userData.mapSave.Map[i, j]);
            }
        }
    }

    private void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(Application.persistentDataPath + "/userdata.json", json);
        PlayerPrefs.SetString("LastUser", userData.username);
    }
}
