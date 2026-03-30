using UnityEngine;

public class RotationWithMouse : MonoBehaviour
{
    [SerializeField] private Transform AppliedObject;
    [SerializeField] [Range(1,20)] private float RotationSpeed = 5f;
    [SerializeField] private bool doRotate = true;
    [SerializeField] private bool lockCursor = true;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public static RotationWithMouse Instance { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (AppliedObject == null)
        {
            AppliedObject = transform;
        }
    }

    private void OnEnable()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (!doRotate) return;

        float mouseX = Input.GetAxis("Mouse X") * RotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * RotationSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        AppliedObject.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
