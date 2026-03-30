using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WebRequest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FetchDate());
        StartCoroutine(FetchUsersInPhp());
    }

    private IEnumerator FetchDate()
    {
        string url = "http://localhost/GDFiveInventory/GetDate.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }

    private IEnumerator FetchUsersInPhp()
    {
        string url = "http://localhost/GDFiveInventory/GetUsers.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }
}
