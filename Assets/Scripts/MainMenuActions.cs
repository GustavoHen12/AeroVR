using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Collections;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject userSelectionMenu;
    [SerializeField] private GameObject newUserInputField;

    private JSONDataManager jsonDataManager;

    private Session currentSession;

    private Camera _mainCamera;
    private const float _defaultFieldOfView = 60.0f;
    // Start is called before the first frame update
    void Start() {
        _mainCamera = Camera.main;

        jsonDataManager = new JSONDataManager();
        jsonDataManager.Awake();

        currentSession = jsonDataManager.LoadData<Session>("session");
        if(currentSession == null) {
            currentSession = new Session();
            currentSession.activeUsersId = new List<int>();
            jsonDataManager.SaveData<Session>(currentSession, "session");
        }

        LoadDropdownOptions();
        LoadMainMenu();
        StopXR();
    }

    private void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");

        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");

        _mainCamera.ResetAspect();
        _mainCamera.fieldOfView = _defaultFieldOfView;
    }

    public void NavigateUserSelectionMenu() {
        mainMenu.SetActive(false);
        userSelectionMenu.SetActive(true);
    }

    public void NavigateMainMenu() {
        mainMenu.SetActive(true);
        userSelectionMenu.SetActive(false);
        LoadMainMenu();
    }

    public void NavigateSettings(){
        // Go to settings scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("UserSettings");
    }

    public void Play(){
        // Go to game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("VRMode");
    }

    public void SaveNewUser() {
        Debug.Log("Save new user");
        string newUserName = newUserInputField.GetComponent<TMP_InputField>().text;
        if(newUserName.Length > 0) {
            User newUser = new User();
            newUser.userName = newUserName;
            newUser.userId = Random.Range(1000, 9999);
            newUser.configuration = new Configuration();
            jsonDataManager.SaveData<User>(newUser, "user_" + newUser.userId);

            currentSession.activeUsersId.Add(newUser.userId);
            jsonDataManager.SaveData<Session>(currentSession, "session");
            Debug.Log(newUserName);

            LoadDropdownOptions();
        }
    }

    public void LoadDropdownOptions() {
        TMP_Dropdown dropdown = userSelectionMenu.GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach(int userId in currentSession.activeUsersId) {
            User user = jsonDataManager.LoadData<User>("user_" + userId);
            options.Add(user.userName);
        }
        dropdown.AddOptions(options);
    }

    public void SelectUser() {
        Debug.Log("Start session");
        TMP_Dropdown dropdown = userSelectionMenu.GetComponentInChildren<TMP_Dropdown>();
        string selectedUserName = dropdown.options[dropdown.value].text;
        Debug.Log(selectedUserName);

        foreach(int userId in currentSession.activeUsersId) {
            User user = jsonDataManager.LoadData<User>("user_" + userId);
            if(user.userName == selectedUserName) {
                currentSession.currentUserId = user.userId;
                currentSession.currentUser = user;
                jsonDataManager.SaveData<Session>(currentSession, "session");

                NavigateMainMenu();
                break;
            }
        }
    }

    public void LoadMainMenu() {
        if(currentSession.currentUserId != 0) {
            Debug.Log("Welcome back " + currentSession.currentUser.userName);

            TMP_Text userNameText = mainMenu.GetComponentInChildren<TMP_Text>();
            userNameText.text = currentSession.currentUser.userName;
        }
    }
}
