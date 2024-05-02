using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 10f; // Time after which the object should self-destruct

    void Start()
    {
        // Invoke the DestroyObject method after 'destroyTime' seconds
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}