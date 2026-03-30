using TMPro;
using UnityEngine;

public class StageDisplayEffect : MonoBehaviour
{
    [SerializeField] GameObject Halo1;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float dur1 = 0.2f;
    [SerializeField] float dur2 = 5;


    

    public void PlayEffect()
    {
        text.text = "Floor: " + GameManager.Instance.Stage;
        transform.position = new(-Screen.width/2, transform.position.y, 0);
        transform.gameObject.LeanMove(new Vector3(Screen.width/2, transform.position.y, 0), dur1).setEaseInCubic().setOnComplete(() =>
        {
            Halo1.SetActive(true);
            transform.gameObject.LeanMove(new Vector3(Screen.width-Screen.width/3, transform.position.y, 0), dur2).setOnComplete(() =>
            {
                transform.gameObject.LeanMove(new Vector3(Screen.width+ Screen.width/2, transform.position.y, 0), dur1).setEaseInCubic().setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
}
