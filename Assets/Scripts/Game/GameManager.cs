using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Google.XR.Cardboard;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class GameController : MonoBehaviour {
    // Timer
    private float currentTime = 0f;
    public GameObject timerText;


    void Start() {
        User currentUser = User.LoadCurrentUser();

        float gameTimeInSeconds = ((float)currentUser.configuration.matchDuration) * 60f;
        StartCoroutine(StartTimer(gameTimeInSeconds));

        if(currentUser.configuration.vrMode){
            Cursor.visible = false;
            StartCoroutine(StartXR());            
        }        
    }

    void Update() { }

    private IEnumerator StartXR() {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null) {
            Debug.LogError("Initializing XR Failed.");
        } else {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }

    IEnumerator StartTimer(float gameTimeInSeconds) {
        currentTime = gameTimeInSeconds;
        UpdateTimerDisplay();

        while (currentTime > 0) {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            currentTime -= 1f; // Decrement timer

            UpdateTimerDisplay();

            // Check if time is up
            if (currentTime <= 0) {
                EndGame();
            }
        }
    }

    void UpdateTimerDisplay() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame() {
        Debug.Log("Game Over!");
        JSONDataManager jsonDataManager = new JSONDataManager();

        Session currentSession = Session.GetInstance();
        User currentUser = User.LoadCurrentUser();

        // Save the game status
        GameStatus gameStatus = GameStatus.GetInstance();
        gameStatus.game_settings = currentUser.configuration;

        currentUser.gamesPlayed = currentUser.gamesPlayed + 1;
        Debug.Log("Saving game status to file: " + currentUser.userId + "_" + currentUser.gamesPlayed);
        
        // Save the game status
        string fileName = "game_status_" + currentUser.userId + "_" + currentUser.gamesPlayed;
        Debug.Log("Saving game status to file: " + fileName);
        Debug.Log(gameStatus.getString());
        jsonDataManager.SaveData<GameStatus>(gameStatus, fileName);

        // Save the user
        currentUser.Save();

        // Navigate to the main menu
        SceneController.PreviousScene = SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHistory");
    }
}