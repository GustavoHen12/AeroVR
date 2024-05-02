using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int baseSpeed = 6;
    public float speedIncreaseRate = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = baseSpeed + (speedIncreaseRate * Time.time);

        transform.position += new Vector3(0, 0, -1*speed) * Time.deltaTime;   
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("DestroyRoad")){
            Destroy(gameObject);
        }
    }
}
