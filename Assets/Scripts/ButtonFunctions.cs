using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public class MethodEvent : UnityEvent { }
    public MethodEvent OnClick;





    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

