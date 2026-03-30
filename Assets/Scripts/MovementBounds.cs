using System.Collections;
using UnityEngine;

public class MovementBounds : MonoBehaviour
{
    [SerializeField] private bool appliedObjectIsCenterPoint;
    [SerializeField] private Vector3 centerPoint;
    [SerializeField] private Vector3 bounds = new Vector3(10, 10, 10);
    [SerializeField] private Vector3 starterPoint;
    [SerializeField] private bool GoToStarterPoint;

    private Rigidbody rb;
    private bool isInitialized = false;
    private bool FirstFrame = true;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        rb = GetComponent<Rigidbody>();
        if (appliedObjectIsCenterPoint)
        {
            centerPoint = transform.position;
        }
        isInitialized = true;
    }

    void FixedUpdate()
    {
        if (GoToStarterPoint)
        {
            transform.position = starterPoint;
            GoToStarterPoint = false;
        }

        if (isInitialized)
        {
            Vector3 minBounds = centerPoint - bounds / 2;
            Vector3 maxBounds = centerPoint + bounds / 2;

            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
                Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z)
            );

            if (transform.position != clampedPosition)
            {
                transform.position = clampedPosition;
                rb.linearVelocity = Vector3.zero;
            }
        }

        if (FirstFrame)
        {
            if (starterPoint!=Vector3.zero) 
            {
                transform.position = starterPoint;
            }
            FirstFrame = false;
        }
    }
}
