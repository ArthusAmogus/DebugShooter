using System.Collections;
using TMPro;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    [SerializeField] private float Size = 1.0f;
    [SerializeField] private float moveAmount=1;
    [SerializeField] private float lifespan=0.1f;
    [SerializeField] private float speed=0.5f;
    [SerializeField] private bool overlapText;
    private GameObject prevObject;

    public void FlashText(string text)
    {
        if (!overlapText)
        {
            if (prevObject!=null) LeanTween.cancel(prevObject);
            if (prevObject != null) Destroy(prevObject);
        }

        GameObject TextObject = new GameObject(text);
        prevObject = TextObject;
        TextObject.transform.position = transform.position;
        TextObject.AddComponent<TextMeshPro>();
        TextObject.GetComponent<TextMeshPro>().text = text;
        TextObject.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
        TextObject.GetComponent<TextMeshPro>().fontSize = Size;
        TextObject.AddComponent<Billboard>();
        TextObject.transform.LeanMoveY(moveAmount, speed).setEaseOutQuint().setOnComplete(() =>
        {
            LeanTween.delayedCall(lifespan, () => {
                Destroy(TextObject);
            });
        });
    }
}
