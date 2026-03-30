using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleText : MonoBehaviour
{
    [Header("Console Settings")]
    [SerializeField] private Vector2 textPosition = new Vector2(0, 0);
    [SerializeField] private float spacing = 30f;
    [SerializeField] private float lifespan = 3f;
    [SerializeField] private bool doAutoDelete = true;
    [SerializeField] private bool testConsole;
    [SerializeField] private bool clearConsole;

    [Header("Text Size & Width")]
    [SerializeField] private float fontSize = 24f;
    [SerializeField] private float textWidth = 500f;

    [Header("References")]
    [SerializeField] private Canvas canvas;

    private float timer;
    private Queue<GameObject> textQueue = new Queue<GameObject>();
    private List<GameObject> allTexts = new List<GameObject>();

    private void OnEnable()
    {
        clearConsole = true;
    }

    private void Update()
    {
        if (testConsole)
        {
            if (timer > 0.5f)
            {
                AddText("Testing the text console: " + Time.time);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if (clearConsole)
        {
            ClearAllText();
            clearConsole = false;
        }
    }

    public void AddText(string text)
    {
        // Always move existing texts up when adding new text
        if (allTexts.Count > 0)
        {
            MoveAllTexts(spacing);
        }

        GameObject textObj = CreateTextObject(text);
        allTexts.Add(textObj);
        textQueue.Enqueue(textObj);

        if (doAutoDelete && lifespan > 0)
        {
            StartCoroutine(RemoveText(textObj));
        }
    }

    private GameObject CreateTextObject(string text)
    {
        GameObject textObj = new GameObject("TextConsoleObject");
        textObj.transform.SetParent(canvas.transform);

        // Setup RectTransform
        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = textPosition;
        rectTransform.sizeDelta = new Vector2(textWidth, fontSize + 10f); // Height based on font size

        // Setup TextMeshPro
        TextMeshProUGUI tmpComponent = textObj.AddComponent<TextMeshProUGUI>();
        tmpComponent.text = text;
        tmpComponent.fontSize = fontSize;
        tmpComponent.color = Color.white;
        tmpComponent.alignment = TextAlignmentOptions.Left;
        tmpComponent.overflowMode = TextOverflowModes.Overflow;
        tmpComponent.enableWordWrapping = true;

        return textObj;
    }

    private void MoveAllTexts(float amount)
    {
        foreach (GameObject textObj in allTexts)
        {
            if (textObj != null)
            {
                RectTransform rect = textObj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchoredPosition += new Vector2(0, amount);
                }
            }
        }
    }

    private IEnumerator RemoveText(GameObject textObj)
    {
        yield return new WaitForSeconds(lifespan);

        if (textObj != null)
        {
            // Remove from collections
            allTexts.Remove(textObj);

            // Rebuild the queue without the destroyed object
            RebuildTextQueue();

            Destroy(textObj);
        }
    }

    private void RebuildTextQueue()
    {
        textQueue.Clear();
        foreach (GameObject obj in allTexts)
        {
            if (obj != null)
            {
                textQueue.Enqueue(obj);
            }
        }
    }

    public void ClearAllText()
    {
        StopAllCoroutines();

        foreach (GameObject textObj in allTexts)
        {
            if (textObj != null)
            {
                Destroy(textObj);
            }
        }

        textQueue.Clear();
        allTexts.Clear();
    }

    // Size and width controls
    public void SetFontSize(float size)
    {
        fontSize = size;

        // Update all existing texts with new size
        foreach (GameObject textObj in allTexts)
        {
            if (textObj != null)
            {
                TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform rect = textObj.GetComponent<RectTransform>();

                if (tmp != null)
                {
                    tmp.fontSize = size;
                }
                if (rect != null)
                {
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, size + 10f);
                }
            }
        }
    }

    public void SetTextWidth(float width)
    {
        textWidth = width;

        // Update all existing texts with new width
        foreach (GameObject textObj in allTexts)
        {
            if (textObj != null)
            {
                RectTransform rect = textObj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
                }
            }
        }
    }

    public void SetTextSizeAndWidth(float size, float width)
    {
        fontSize = size;
        textWidth = width;

        // Update all existing texts
        foreach (GameObject textObj in allTexts)
        {
            if (textObj != null)
            {
                TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform rect = textObj.GetComponent<RectTransform>();

                if (tmp != null)
                {
                    tmp.fontSize = size;
                }
                if (rect != null)
                {
                    rect.sizeDelta = new Vector2(width, size + 10f);
                }
            }
        }
    }

    // Public properties for external control
    public bool ClearConsole
    {
        get { return clearConsole; }
        set { clearConsole = value; }
    }

    public bool TestConsole
    {
        get { return testConsole; }
        set { testConsole = value; }
    }

    public float FontSize
    {
        get { return fontSize; }
        set { SetFontSize(value); }
    }

    public float TextWidth
    {
        get { return textWidth; }
        set { SetTextWidth(value); }
    }
}