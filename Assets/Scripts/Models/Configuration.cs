using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Configuration
{
    public bool vrMode;
    public int matchDuration;
    public float speed;
    public int timeExibitionSign;
    public string signSize;
    public float probabilityExibitionSign;
    public int timeBetweenSigns;
    public int forestDensity;
    public int [] signDistribution;

    public Configuration()
    {
        this.vrMode = false;
        this.matchDuration = 5;
        this.speed = 1f;
        this.timeExibitionSign = 500;
        this.probabilityExibitionSign = 0.5f;
        this.signDistribution = new int[15];
        this.signSize = "Normal";
        this.forestDensity = 5;
        this.timeBetweenSigns = 5;
    }

    public string getString() {
        string configuracao = "Modo VR: " + (vrMode ? "Ativado" : "Desativado") + "\n" +
            "Duração da partida: " + matchDuration + "m\n" +
            "Velocidade: " + (speed == 1 ? "Normal" : speed + "x") + "\n" +
            "Tempo de exibição da placa: " + timeExibitionSign + "ms\n" +
            "Frequência: " + (probabilityExibitionSign * 100) + "%\n";
        configuracao += "Tamanho da placa: " + signSize + "\n";
        configuracao += "Densidade da floresta: " + forestDensity + "\n";
        configuracao += "Tempo entre placas: " + timeBetweenSigns + "s\n";
        
        return configuracao;
    }
}
