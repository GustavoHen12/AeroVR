using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Collections;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class Moviment : MonoBehaviour
{
    [SerializeField] float turnSpeed = 2;
    public float moveSpeed = 1f;
    private float LEFT_LIMIT = -7;
    private float RIGHT_LIMIT = 7;

    public float boxWidthPercentage = 0.5f; // Percentage of screen width for the box width
    public float boxHeightPercentage = 0.5f; // Percentage of screen height for the box height

    // Timer
    private float currentTime = 0f;

    private float currentLane = 0, nextLane = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;

        Session currentSession = Session.GetInstance();

        float gameTimeInSeconds = currentSession.currentUser.configuration.matchDuration * 60f;
        Debug.Log(currentSession.currentUser.configuration.matchDuration);
        StartCoroutine(StartTimer(gameTimeInSeconds));

        StartCoroutine(StartXR());
    }

    private IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed.");
        }
        else
        {
            Debug.Log("XR initialized.");

            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }

    // Update is called once per frame
    void Update() {
        currentLane = transform.position.x;
        float h = Input.GetAxis("Mouse X");

        if(h!=0){
            Debug.Log(h);
            if(h > 0 && currentLane < RIGHT_LIMIT){
                MovePlayer(h > 0 ? (turnSpeed) : (-1 * turnSpeed));
            } else if (h < 0 && currentLane > LEFT_LIMIT){
                MovePlayer(h > 0 ? (turnSpeed) : (-1 * turnSpeed));
            }
        }

    //TODO: TOUCH INPUT
    //     if (Input.touchCount > 0) {
    //         Touch touch = Input.GetTouch(0);
    //         if (touch.phase == TouchPhase.Began) {
    //             if (touch.position.x < Screen.width / 2) {
    //                 nextLane = transform.position.x - 7;
    //             } else {
    //                 nextLane = transform.position.x + 7;
    //             }
    //         }
    //     }
    }

    void MovePlayer(float direction) {
        // Move the player horizontally based on the direction (left or right)
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }

    IEnumerator StartTimer(float gameTimeInSeconds)
    {
        currentTime = gameTimeInSeconds;

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            currentTime -= 1f; // Decrement timer

            UpdateTimerDisplay();

            // Check if time is up
            if (currentTime <= 0)
            {
                EndGame();
            }
        }
    }

    void UpdateTimerDisplay() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame() {
        Debug.Log("Game Over!");
        // Add any game over logic here
    }
}
