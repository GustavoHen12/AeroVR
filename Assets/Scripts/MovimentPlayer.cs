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
    public GameObject player;
    public GameObject camera;
    public float moveSpeed = 1f;
    private float LEFT_LIMIT = -7;
    private float RIGHT_LIMIT = 7;

    // Timer
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Session currentSession = Session.GetInstance();

        float gameTimeInSeconds = currentSession.currentUser.configuration.matchDuration * 60f;
        Debug.Log(currentSession.currentUser.configuration.matchDuration);
        StartCoroutine(StartTimer(gameTimeInSeconds));

        if(currentSession.currentUser.configuration.vrMode){
            Cursor.visible = false;
            StartCoroutine(StartXR());            
        }
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

    private static float lastMoviment = 0;
    private static float turnIntensity = 0.0f;
    // Update is called once per frame
    void Update() {
        float currentLane = transform.position.x;
        float h = Input.GetAxis("Mouse X");

        if(h!=0){
            turnIntensity = h;

            Debug.Log(h);
            if(h > 0 && currentLane < RIGHT_LIMIT){
                MovePlayer(turnSpeed, turnIntensity*2);
            } else if (h < 0 && currentLane > LEFT_LIMIT){
                MovePlayer((-1 * turnSpeed), turnIntensity*2);
            }
        } else {
            // while rotation is not 0, keep rotating
            float z_rotation = player.transform.rotation.eulerAngles.z;
            Debug.Log(z_rotation + " " + turnIntensity);
            if((z_rotation > 2  || z_rotation < -2)){
                MovePlayer(0, (z_rotation > 2 && turnIntensity > 0) ? -1 : 1);
            }
        }
    }


    void MovePlayer(float direction, float intensity) {

        // Move the player horizontally based on the direction (left or right)
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(intensity)) * -Mathf.Sign(intensity);
        player.transform.Rotate(Vector3.forward, roll * Time.deltaTime);

        // transform.localRotation = Quaternion.Euler(Vector3.forward * roll);

        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
        camera.transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
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
