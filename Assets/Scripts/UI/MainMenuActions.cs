using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using Google.XR.Cardboard;
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

        currentSession = jsonDataManager.LoadData<Session>("session");
        if(currentSession == null) {
            currentSession = new Session();
            currentSession.activeUsersId = new List<int>();
            currentSession.usersCounter = 0;
            currentSession.Save();
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

    public void NavigateHistory(){
        // Go to history scene
        SceneController.PreviousScene = SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHistory");
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
            newUser.gamesPlayed = 0;
            newUser.userId = currentSession.usersCounter + 1;
            newUser.configuration = new Configuration();
            newUser.Save();

            currentSession.activeUsersId.Add(newUser.userId);
            currentSession.usersCounter++;
            currentSession.Save();
            Debug.Log(newUserName);

            LoadDropdownOptions();
        }
    }

    public void LoadDropdownOptions() {
        TMP_Dropdown dropdown = userSelectionMenu.GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach(int userId in currentSession.activeUsersId) {
            User user = User.LoadUser(userId);
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
            User user = User.LoadUser(userId);
            if(user.userName == selectedUserName) {
                currentSession.currentUserId = user.userId;
                currentSession.Save();
                NavigateMainMenu();
                break;
            }
        }
    }

    public void LoadMainMenu() {
        if(currentSession.currentUserId != 0) {
            User currentUser = User.LoadUser(currentSession.currentUserId);

            TMP_Text userNameText = mainMenu.GetComponentInChildren<TMP_Text>();
            userNameText.text = currentUser.userName;
        }
    }
}
