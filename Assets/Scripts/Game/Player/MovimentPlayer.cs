using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

using Google.XR.Cardboard;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class Moviment : MonoBehaviour
{
    [SerializeField] float turnSpeed = 2;
    public GameObject player;
    public GameObject camera;
    public float moveSpeed = 1f;
    private static float turnIntensity = 0.0f;

    void Start() { }

    void Update() {
        float currentLane = transform.position.x;
        float h = Input.GetAxis("Mouse X");

        if(h!=0){
            turnIntensity = h;

            if(h > 0 && currentLane < SceneUtil.RIGHT_LIMIT){
                MovePlayer(turnSpeed, turnIntensity*2);
            } else if (h < 0 && currentLane > SceneUtil.LEFT_LIMIT){
                MovePlayer((-1 * turnSpeed), turnIntensity*2);
            }
        } else {
            // while rotation is not 0, keep rotating
            float z_rotation = player.transform.rotation.eulerAngles.z;
            if((z_rotation > 5  || z_rotation < -5)){
                MovePlayer(0, (z_rotation > 5 && turnIntensity > 0) ? -1 : 1);
            }
        }
    }


    void MovePlayer(float direction, float intensity) {
        // Move the player horizontally based on the direction (left or right)
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(intensity)) * -Mathf.Sign(intensity);
        player.transform.Rotate(Vector3.forward, roll * Time.deltaTime);

        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
        camera.transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }
}
