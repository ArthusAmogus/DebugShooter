using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurtainBehavior : MonoBehaviour
{
    public static CurtainBehavior Instance { get; internal set; }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        Instance = this;
    }

    public void OpenCurtain()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.LeanMoveX(Screen.width, 0.3f).setEaseInQuint();
    }

    public void CloseCurtain()
    {
        gameObject.transform.position = new Vector3(-Screen.width, 0, 0);
        gameObject.transform.LeanMoveX(0, 0.3f).setEaseInQuint();
    }

    public void StartFadeToColor(Color targetColor, float duration)
    {
        StartCoroutine(FadeToColor(targetColor, duration));
    }

    private IEnumerator FadeToColor(Color targetColor, float duration)
    {
        Image image = GetComponent<Image>();
        Color startColor = image.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            image.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        image.color = targetColor; // Ensure exact color at end
    }
}
