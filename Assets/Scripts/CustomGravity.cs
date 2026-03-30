using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    private Rigidbody target;
    private Coroutine delayedStartCoroutine;
    [SerializeField] private float gravity = -170;

    private void OnEnable()
    {
        if (delayedStartCoroutine!=null)
        {
            StopCoroutine(delayedStartCoroutine);
        }
        delayedStartCoroutine = StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        if (target == null)
        {
            target = GetComponent<Rigidbody>();
        }
    }

    //Custom Gravity
    void FixedUpdate()
    {
        if (target!=null)
        {
            target.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        }
    }
}
