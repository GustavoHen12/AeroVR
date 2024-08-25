using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 10f; // Time after which the object should self-destruct

    void Start() {
        User currentUser = User.LoadCurrentUser();

        destroyTime = currentUser.configuration.timeExibitionSign/1000f;
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject() {
        Destroy(gameObject);
    }
}