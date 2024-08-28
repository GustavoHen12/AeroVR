using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int baseSpeed = 6;
    public float speedFactor = 1f;

    // Start is called before the first frame update
    void Start() {
        User currentUser = User.LoadCurrentUser();

        speedFactor = currentUser.configuration.speed;
        Debug.Log(speedFactor);
    }

    // Update is called once per frame
    void Update()
    {
        float speedBoost = 0;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            speedBoost = 0.7f;
        }

        float speed = baseSpeed * (speedFactor + speedBoost);
        transform.position += new Vector3(0, 0, -1*speed) * Time.deltaTime;   
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("DestroyRoad")){
            Destroy(gameObject);
        }
    }
}
