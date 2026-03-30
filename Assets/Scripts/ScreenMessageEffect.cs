using TMPro;
using UnityEngine;

public class ScreenMessageEffect : MonoBehaviour
{
    [SerializeField] float transitionTime = 0.2f;
    public float showTime = 0.5f;
    [SerializeField] TextMeshProUGUI text;
    public string message;

    private void OnEnable()
    {
        LeanTween.cancel(gameObject);
        text.text = message;
        gameObject.transform.LeanScaleY(0, 0).setOnComplete(() =>
        {
            gameObject.transform.LeanScaleY(1, transitionTime).setEaseOutQuart().setOnComplete(() =>
            {
                gameObject.transform.LeanScaleY(0, transitionTime).setEaseOutQuart().setDelay(showTime).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
