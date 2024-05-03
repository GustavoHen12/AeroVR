using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 10f; // Time after which the object should self-destruct

    void Start() {
        Session currentSession = Session.GetInstance();
        destroyTime = currentSession.currentUser.configuration.timeExibitionSign;
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject() {
        Destroy(gameObject);
    }
}