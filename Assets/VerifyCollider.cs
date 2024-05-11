using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Ground Triggered");
        Debug.Log(other.tag);
        Debug.Log(other.name);
    }

    public void OnCollisionEnter(Collision collision) {
        Debug.Log("Ground Collision");
        Debug.Log(collision.collider.tag);
        Debug.Log(collision.collider.name);
    }
}
