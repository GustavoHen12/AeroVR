using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int baseSpeed = 6;
    public float speedFactor = 1f;

    // Start is called before the first frame update
    void Start() {
        Session currentSession = Session.GetInstance();
        speedFactor = currentSession.currentUser.configuration.speed;
        Debug.Log(speedFactor);
    }

    // Update is called once per frame
    void Update()
    {
        float speed = baseSpeed * speedFactor;
        transform.position += new Vector3(0, 0, -1*speed) * Time.deltaTime;   
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("DestroyRoad")){
            Destroy(gameObject);
        }
    }
}
