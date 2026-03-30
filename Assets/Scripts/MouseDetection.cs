using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Mouse Logging")]
    public bool LogMouseEnter = false;
    public bool LogMouseExit = false;
    public bool LogMouseDown = false;
    public bool LogMouseUp = false;

    [Header("Custom Mouse Events")]
    public UnityEvent MouseEnterEvent;
    public UnityEvent MouseExitEvent;
    public UnityEvent MouseDownEvent;
    public UnityEvent MouseUpEvent;

    private bool isUIElement = false;

    void Start()
    {
        isUIElement = GetComponent<RectTransform>() != null;
        if (isUIElement && GetComponent<UnityEngine.UI.Graphic>() != null)
        {
            var graphic = GetComponent<UnityEngine.UI.Graphic>();
            graphic.raycastTarget = true;
        }
    }

    // Traditional 3D/2D object events
    private void OnMouseEnter()
    {
        if (!isUIElement)
        {
            if (LogMouseEnter)
            {
                Debug.Log("Entered " + gameObject.name);
            }
            MouseEnterEvent?.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (!isUIElement)
        {
            if (LogMouseExit)
            {
                Debug.Log("Exited " + gameObject.name);
            }
            MouseExitEvent?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        if (!isUIElement)
        {
            if (LogMouseDown)
            {
                Debug.Log("Pressed Down " + gameObject.name);
            }
            MouseDownEvent?.Invoke();
        }
    }

    private void OnMouseUp()
    {
        if (!isUIElement)
        {
            if (LogMouseUp)
            {
                Debug.Log("Pressed Up " + gameObject.name);
            }
            MouseUpEvent?.Invoke();
        }
    }

    // UI Events (for UI Image GameObjects)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUIElement)
        {
            if (LogMouseEnter)
            {
                Debug.Log("UI Entered " + gameObject.name);
            }
            MouseEnterEvent?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isUIElement)
        {
            if (LogMouseExit)
            {
                Debug.Log("UI Exited " + gameObject.name);
            }
            MouseExitEvent?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isUIElement)
        {
            if (LogMouseDown)
            {
                Debug.Log("UI Pressed Down " + gameObject.name);
            }
            MouseDownEvent?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isUIElement)
        {
            if (LogMouseUp)
            {
                Debug.Log("UI Pressed Up " + gameObject.name);
            }
            MouseUpEvent?.Invoke();
        }
    }
}