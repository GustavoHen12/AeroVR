using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SignExibitionData {
    public int sign;
    public float timestamp;
    public float x, y, z;
}

[System.Serializable]
public class GameStatus {
    public List<float> colisions_timestamp;
    public List<SignExibitionData> signs;
    public float gameStartTime;
    public string dateTimeStart;
    public int score;
    public Configuration game_settings;
    private static GameStatus instance;
    public static GameStatus GetInstance () {
        if (instance == null) {
            instance = new GameStatus();
            instance.colisions_timestamp = new List<float>();
            instance.signs = new List<SignExibitionData>();
            instance.gameStartTime = Time.time;
            instance.score = -1;
            instance.dateTimeStart = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        return instance;
    }

    public string getString () {
        string resume = "Início do jogo: " + dateTimeStart + "\n";
        resume += "Placas exibidas (em ordem): ";
        foreach (SignExibitionData sign in signs) {
            resume += sign.sign + " ";
        }
        resume += score >= 0 ? "\nPontuação: " + score : "\nSem pontuação";
        resume += "\nColisões:";
        int index = 1;
        foreach (float colision in colisions_timestamp) {
            int minutes = (int) colision / 60;
            int seconds = (int) colision % 60;
            resume += "\n\t" + index + "°: " + (colision > 60 ? (minutes + "m" + seconds + "s") : seconds + "s");
            index++;
        }

        return resume;
    }
}