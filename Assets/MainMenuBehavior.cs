using Firebase.Database;
using Google.MiniJSON;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField] GameObject haloContinue;
    [SerializeField] GameObject haloNewGame;
    [SerializeField] GameObject haloAccount;
    [SerializeField] GameObject haloQuit;

    [SerializeField] Transform Menu;
    [SerializeField] Transform Account;

    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject AccountPanel;
    [SerializeField] GameObject MessagePanel;
    [SerializeField] GameObject CurrentPanel;

    [SerializeField] Button loginBack;
    [SerializeField] Button loginConfirm;
    [SerializeField] Button messageConfirm;
    [SerializeField] Button accountDelete;
    [SerializeField] Button accountLogOut;
    [SerializeField] Button continueGame;

    [SerializeField] DatabaseReference reference;
    [SerializeField] bool bypassLoginConstraint;
    [SerializeField] bool toAccountDelete;

    [SerializeField] TextMeshProUGUI MessagePanelText;
    [SerializeField] TextMeshProUGUI accountNameMenuText;
    [SerializeField] TextMeshProUGUI accountNameText;
    [SerializeField] TextMeshProUGUI dateAccountCreatedText;
    [SerializeField] TMP_InputField NameInput;
    [SerializeField] TMP_InputField PassInput;

    private void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OpenAccountPanel(bool show)
    {
        UserDataObject.Instance.SaveData();
        CurrentPanel = AccountPanel;
        haloAccount.SetActive(show);
        accountNameText.text = UserDataObject.Instance.userData.username;
        dateAccountCreatedText.text = UserDataObject.Instance.userData.dateAccountCreated;
        OpenPanel(AccountPanel, show, true);
    }

    public void AccountLogOut()
    {
        toAccountDelete = false;
        MessagePanelText.text = "Are you sure to log out?";
        messageConfirm.gameObject.SetActive(true);
        OpenMessagePanel(true);
    }

    public void AccountDelete()
    {
        toAccountDelete = true;
        MessagePanelText.text = "Are you sure to delete your current account?";
        messageConfirm.gameObject.SetActive(true);
        OpenMessagePanel(true);
    }

    public void OpenLoginPanel(bool show)
    {
        CurrentPanel = LoginPanel;
        OpenPanel(LoginPanel, show, true);
    }

    public void OpenMessagePanel(bool show)
    {
        if (show)
        {
            OpenPanel(CurrentPanel, false, false);
            OpenPanel(MessagePanel, true, false);
        }
        else
        {
            if (CurrentPanel!=null) OpenPanel(CurrentPanel, true, false);
            OpenPanel(MessagePanel, false, false);
        }
    }

    public async void CheckDatabase()
    {
        if (string.IsNullOrWhiteSpace(NameInput.text) || string.IsNullOrWhiteSpace(PassInput.text))
        {
            MessagePanelText.text = "Both input fields can't be empty.";
            messageConfirm.gameObject.SetActive(false);
            OpenMessagePanel(true);
            return;
        }

        if (NameInput.text.Length<8 || PassInput.text.Length<8)
        {
            MessagePanelText.text = "Both input fields should be 8 characters long.";
            messageConfirm.gameObject.SetActive(false);
            OpenMessagePanel(true);
            return;
        }

        DataSnapshot snapshot = await reference.Child("DebugTheGame").Child("Players").Child(NameInput.text).GetValueAsync();
        UserData data = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());

        // Check if exists
        bool exists = snapshot.Exists;

        if (exists)
        {
            if (PassInput.text == data.password)
            {
                UserDataObject.Instance.userData = data;
                OpenPanel(LoginPanel, false, true);
            }
            else
            {
                MessagePanelText.text = "The password is incorrect.";
                messageConfirm.gameObject.SetActive(false);
                OpenMessagePanel(true);
            }
        }
        else
        {
            CurrentPanel = LoginPanel;
            OpenPanel(LoginPanel, false, false);
            MessagePanelText.text = "It seems that the account details are not found in the database. Register account?";
            messageConfirm.gameObject.SetActive(true);
            OpenMessagePanel(true);
        }
    }

    public void MessageConfirm()
    {
        if (CurrentPanel==LoginPanel)
        {
            UserData data = new()
            {
                username = NameInput.text,
                password = PassInput.text
            };
            int[] id = new int[4];
            for (int i = 0; i < id.Length; i++)
                id[i] = Random.Range(1, 10);
            string userID =
                id[0].ToString() + "" +
                id[1].ToString() + "" +
                id[2].ToString() + "" +
                id[3].ToString();
            data.id = int.Parse(userID);
            data.dateAccountCreated = DateTime.Now.ToString("MMMM/dd/yyyy, hh:mmtt");

            UserDataObject.Instance.userData = data;

            string json = JsonUtility.ToJson(data, true);
            Debug.Log(json);
            File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
            reference.Child("DebugTheGame").Child("Players").Child(data.username).SetRawJsonValueAsync(json);

            CurrentPanel = null;
            OpenMessagePanel(false);
            OpenLoginPanel(false);
        }

        if (CurrentPanel==AccountPanel)
        {
            if (toAccountDelete)
            {
                reference.Child("DebugTheGame").Child("Players").Child(UserDataObject.Instance.userData.username).RemoveValueAsync();
                UserDataObject.Instance.userData = new UserData();
            }
            else
            {
                UserDataObject.Instance.userData = new UserData();
            }

            CurrentPanel = LoginPanel;
            OpenMessagePanel(false);
        }
    }

    public void OpenPanel(GameObject panel, bool show, bool showMenu)
    {
        if (show)
        {
            Menu.LeanScaleX(100, 0.2f).setEaseInExpo().setOnComplete(() =>
            {
                Menu.gameObject.SetActive(false);
                Account.gameObject.SetActive(false);
                panel.SetActive(true);
                panel.LeanScaleY(1, 0.1f).setEaseOutExpo().setOnComplete(() =>
                {
                    if (UserDataObject.Instance.userData.id==0&&!bypassLoginConstraint)
                    {
                        loginBack.interactable = false;
                    }
                    else
                    {
                        loginBack.interactable = true;
                    }
                });
            });
            Account.LeanScaleX(100, 0.2f).setEaseInExpo();
        }
        else
        {
            panel.LeanScaleY(0, 0.1f).setEaseInExpo().setOnComplete(() =>
            {
                panel.SetActive(false);

                if (showMenu)
                {
                    Menu.gameObject.SetActive(true);
                    Account.gameObject.SetActive(true);
                    Menu.LeanScaleX(1, 0.3f).setEaseOutExpo();
                    Account.LeanScaleX(1, 0.3f).setEaseOutExpo().setOnComplete(() =>
                    {
                        accountNameMenuText.text = UserDataObject.Instance.userData.username;
                    });
                }
            });
        }
    }

    private void Start()
    {
        Menu.LeanScaleX(100, 0).setEaseInExpo().setOnComplete(() =>
        {
            Menu.gameObject.SetActive(false);
            Account.gameObject.SetActive(false);
        });
        Account.LeanScaleX(100, 0).setEaseInExpo();
        StartCoroutine(MainMenuStart());
    }

    IEnumerator MainMenuStart()
    {
        accountNameText.text = UserDataObject.Instance.userData.username;
        CurtainBehavior.Instance.StartFadeToColor(Color.white, 0.5f);
        yield return new WaitForSeconds(0.6f);
        CurtainBehavior.Instance.OpenCurtain();
        yield return new WaitForSeconds(0.5f);
        if (UserDataObject.Instance.userData.id == 0)
        {
            OpenLoginPanel(true);
        }
        else
        {
            ShowMenuText(true);
        }
    }

    public void ShowMenuText(bool show)
    {
        if (show)
        {
            Menu.gameObject.SetActive(true);
            Account.gameObject.SetActive(true);
            Menu.LeanScaleX(1, 0.3f).setEaseOutExpo();
            Account.LeanScaleX(1, 0.3f).setEaseOutExpo().setOnComplete(() =>
            {
                accountNameMenuText.text = UserDataObject.Instance.userData.username;
                continueGame.interactable = UserDataObject.Instance.userData.hasUnfinishedSession;
            });
        }
        else
        {
            Menu.LeanScaleX(100, 0.2f).setEaseInExpo().setOnComplete(() =>
            {
                Menu.gameObject.SetActive(false);
                Account.gameObject.SetActive(false);
            });
            Account.LeanScaleX(100, 0.2f).setEaseInExpo();
        }
    }

    public void StartContinueGame()
    {

        haloContinue.SetActive(true);
        PlayerPrefs.SetInt("Continue?", 1);
        StartCoroutine(StartGame());
    }

    public void StartNewGame()
    {
        UserDataObject.Instance.userData.hasUnfinishedSession = false;
        UserDataObject.Instance.userData.mapSave = null;
        UserDataObject.Instance.userData.inventorySlotData.Clear();
        haloNewGame.SetActive(true);
        PlayerPrefs.SetInt("Continue?", 0);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        UserDataObject.Instance.SaveData();
        ShowMenuText(false);
        yield return new WaitForSeconds(0.3f);
        CurtainBehavior.Instance.CloseCurtain();
        yield return new WaitForSeconds(0.5f);
        CurtainBehavior.Instance.StartFadeToColor(Color.black, 0.5f);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("MainGame");
    }

    public void StartExitGame()
    {
        haloQuit.SetActive(true);
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        UserDataObject.Instance.SaveData();
        ShowMenuText(false);
        yield return new WaitForSeconds(0.3f);
        CurtainBehavior.Instance.CloseCurtain();
        yield return new WaitForSeconds(0.5f);
        CurtainBehavior.Instance.StartFadeToColor(Color.black, 0.5f);
        yield return new WaitForSeconds(0.7f);
        UserDataObject.Instance.SaveData();

        Application.Quit();
        Debug.Log("App terminated");
    }

}
