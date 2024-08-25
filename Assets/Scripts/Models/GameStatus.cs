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
public class ColisionData {
    public float timestamp;
    public float x_player, y_player, z_player;
    public float x_obstacle, y_obstacle, z_obstacle;

    public ColisionData(float timestamp, Vector3 player, Vector3 obstacle) {
        this.timestamp = timestamp;
        this.x_player = player.x;
        this.y_player = player.y;
        this.z_player = player.z;
        this.x_obstacle = obstacle.x;
        this.y_obstacle = obstacle.y;
        this.z_obstacle = obstacle.z;
    }
}

[System.Serializable]
public class Road {
    public List<SceneObject> roads;
    public Road() {
        this.roads = new List<SceneObject>();
    }

    public void addRoad(SceneObject road) {
        this.roads.Add(road);
    }
}

[System.Serializable]
public class MatchData {
    public List<Road> roads;
    public List<SceneHighlight> highlights;
    public MatchData() {
        this.roads = new List<Road>();
        this.highlights = new List<SceneHighlight>();
    }

    public void addRoad(List<SceneObject> road) {
        Debug.Log("Adding road");
        Debug.Log(road.Count);
        Road newRoad = new Road();
        newRoad.roads = road;
        this.roads.Add(newRoad);
    }

    public int getRoadsCount() {
        return this.roads.Count;
    }

    public void addHighlight(SceneHighlight highlight) {
        this.highlights.Add(highlight);
    }
}


[System.Serializable]
public class GameStatus {
    public List<ColisionData> colisions;
    public List<SignExibitionData> signs;
    public float gameStartTime;
    public string dateTimeStart;
    public int score;
    public Configuration game_settings;
    public MatchData matchData;
    public int userId;
    private static GameStatus instance;
    
    public static GameStatus GetInstance () {
        if (instance == null) {
            instance = new GameStatus();
            instance.colisions = new List<ColisionData>();
            instance.signs = new List<SignExibitionData>();
            instance.gameStartTime = Time.time;
            instance.score = -1;
            instance.dateTimeStart = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            instance.matchData = new MatchData();
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
        foreach (ColisionData colision in colisions) {
            int minutes = (int) colision.timestamp / 60;
            int seconds = (int) colision.timestamp % 60;
            resume += "\n\t" + index + "°: " + (colision.timestamp > 60 ? (minutes + "m" + seconds + "s") : seconds + "s");

            // Lateralidade da colisão
            string lateralidade = colision.x_player > colision.x_obstacle ? "esquerda" : "direita";
            resume += " - Lateralidade: " + lateralidade;
            index++;
        }

        return resume;
    }
}