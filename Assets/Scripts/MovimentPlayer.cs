using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Moviment : MonoBehaviour
{
    [SerializeField] float turnSpeed = 2;
    public float moveSpeed = 1f;
    private float LEFT_LIMIT = -7;
    private float RIGHT_LIMIT = 7;

    public float boxWidthPercentage = 0.5f; // Percentage of screen width for the box width
    public float boxHeightPercentage = 0.5f; // Percentage of screen height for the box height

    private float currentLane = 0, nextLane = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.Locked;
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

        // if(currentLane > LEFT_LIMIT && currentLane < RIGHT_LIMIT && Math.Abs(currentLane - nextLane) > 0.05) {
        
        // }

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

    //     if(currentLane > LEFT_LIMIT && currentLane < RIGHT_LIMIT && Math.Abs(currentLane - nextLane) > 0.05) {
    //         if (currentLane < nextLane) {
    //             MovePlayer(turnSpeed);
    //         } else {
    //             MovePlayer(-1 * turnSpeed);
    //         }
    //     }
    }

    void MovePlayer(float direction) {
        // Move the player horizontally based on the direction (left or right)
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }
}
