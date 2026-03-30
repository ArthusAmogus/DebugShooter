using UnityEditor;
using UnityEngine;

public class AccountSystemBehavior : MonoBehaviour
{
    public static AccountSystemBehavior Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    

}
