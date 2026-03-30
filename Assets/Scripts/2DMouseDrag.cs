using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Properties
    float posx; float posy;

    public bool do_x = true;
    public bool do_y = true;

    // UI Components
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Store original position for reference
        originalPosition = rectTransform.anchoredPosition;
    }

    

    private void Update()
    {
        // Get current position from RectTransform
        posx = rectTransform.anchoredPosition.x;
        posy = rectTransform.anchoredPosition.y;

        // Update position
        rectTransform.anchoredPosition = new Vector2(posx, posy);
    }

    // Drag Initialization
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (posx < 0) { pos(posx - 0.01f, posy); }
        if (posx > 0) { pos(posx + 0.01f, posy); }

        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (posx < 0) { pos(posx + 0.01f, posy); }
        if (posx > 0) { pos(posx - 0.01f, posy); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Convert screen position to local position in canvas
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            if (do_x) posx = localPointerPosition.x;
            if (do_y) posy = localPointerPosition.y;


            rectTransform.anchoredPosition = new Vector2(posx, posy);
        }
    }

    private void pos(float posx, float posy)
    {
        rectTransform.anchoredPosition = new Vector2(posx, posy);
    }
}