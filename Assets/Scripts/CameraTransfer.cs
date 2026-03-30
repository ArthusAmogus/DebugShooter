using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransfer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform[] scenes;
    public Vector3 rotationPivot;
    public int index;
    [SerializeField] private bool doDebugLog = false;

    private void Start()
    {
        cam = Camera.main;
    }

    public void TransferCamera()
    {
        if (index >= 0 && index < scenes.Length)
        {
            cam.transform.position = scenes[index].position;
            Quaternion targetRotation = scenes[index].rotation * Quaternion.Euler(rotationPivot);
            cam.transform.rotation = targetRotation;
            cam.transform.SetParent(scenes[index]);

            // cam.orthographicSize = scenes[index].GetComponent<Camera>().orthographicSize;

            if (doDebugLog) Debug.Log($"Camera transferred to scene {index}: Position {cam.transform.position}, Rotation {cam.transform.rotation}");
        }
        else
        {
            if (doDebugLog) Debug.LogError("Index out of bounds for scenes array.");
        }
    }

    public void SetScene(int num)
    {
        index = num;
        TransferCamera();
    }

    public void NextScene()
    {
        index = (index + 1) % scenes.Length;
        TransferCamera();
    }

    
    public void PreviousScene()
    {
        index = (index - 1 + scenes.Length) % scenes.Length;
        TransferCamera();
    }
}
