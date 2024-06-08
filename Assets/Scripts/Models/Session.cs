using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session {
    private static Session instance;
    public static Session GetInstance () {
        if (instance == null) {
            JSONDataManager jsonDataManager = new JSONDataManager();
            instance = jsonDataManager.LoadData<Session>("session");
        }
        return instance;
    }

    public int currentUserId;
    public List<int> activeUsersId;
    public int usersCounter;

    public void Save() {
        JSONDataManager jsonDataManager = new JSONDataManager();
        jsonDataManager.SaveData<Session>(this, "session");
    }
}
