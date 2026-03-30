using UnityEngine;
using Firebase.Database;
using System;

public class DataSaver : MonoBehaviour
{
    public DataToSave dataToSave;
    DatabaseReference reference;
    public int userID;

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataToSave, true);
        reference.Child("users").Child(userID.ToString()).SetRawJsonValueAsync(json);
        Debug.Log(json);
    }
}

[Serializable]
public class DataToSave
{
    public string playerName;
    public int lvl;
    public int money;
}
