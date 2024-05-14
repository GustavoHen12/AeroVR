using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SignExibitionData {
    public int sign;
    public int timestamp;
}

[System.Serializable]
public class GameStatus {
    public List<int> colisions_timestamp;
    public List<SignExibitionData> signs;
    public Configuration game_settings;

    private static GameStatus instance;
    public static GameStatus GetInstance () {
        if (instance == null) {
            instance = new GameStatus();
            instance.colisions_timestamp = new List<int>();
            instance.signs = new List<SignExibitionData>();
        }
        return instance;
    }
}