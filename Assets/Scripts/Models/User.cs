using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string userName;
    public int userId;
    public int gamesPlayed;
    public Configuration configuration;

    public User() {
        this.userName = "Player";
        this.userId = 0;
        this.gamesPlayed = 0;
        this.configuration = new Configuration();
    }

    public void Save() {
        JSONDataManager jsonDataManager = new JSONDataManager();
        jsonDataManager.SaveData<User>(this, "user_" + this.userId);
    }

    public static User LoadUser(int userId) {
        JSONDataManager jsonDataManager = new JSONDataManager();
        return jsonDataManager.LoadData<User>("user_" + userId);
    }

    public static User LoadCurrentUser() {
        Session currentSession = Session.GetInstance();
        return LoadUser(currentSession.currentUserId);
    }

    public Configuration GetConfiguration() {
        return this.configuration;
    }

    public void Clear() {
        JSONDataManager jsonDataManager = new JSONDataManager();
        jsonDataManager.ClearData("user_" + this.userId);
    }
}
