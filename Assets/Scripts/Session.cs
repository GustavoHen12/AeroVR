using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session {
    private static Session instance;
    public static Session GetInstance () {
        if (instance == null) {
            JSONDataManager jsonDataManager = new JSONDataManager();
            jsonDataManager.Awake();
            Debug.Log("Session instance is null");
            instance = jsonDataManager.LoadData<Session>("session");;
        }
        return instance;
    }

    public int currentUserId;
    public User currentUser;
    public List<int> activeUsersId;
    public int usersCounter;
}
