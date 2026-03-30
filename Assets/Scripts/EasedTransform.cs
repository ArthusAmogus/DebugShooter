using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EasedTransform : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    public Vector3 Location;


    void OnEnable()
    {
        transform.LeanMove(transform.position + Location, duration).setEaseOutQuint();
    }
}
