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
    public float probabilityExibitionSign;
    public int [] signDistribution;

    public Configuration()
    {
        this.vrMode = false;
        this.matchDuration = 5;
        this.speed = 1f;
        this.timeExibitionSign = 5;
        this.probabilityExibitionSign = 0.5f;
        this.signDistribution = new int[9];
    }

    public string getString() {
        string configuracao = "Modo VR: " + (vrMode ? "Ativado" : "Desativado") + "\n" +
            "Duração da partida: " + matchDuration + "m\n" +
            "Velocidade: " + (speed == 1 ? "Normal" : speed + "x") + "\n" +
            "Tempo de exibição da placa: " + timeExibitionSign + "s\n" +
            "Frequência: " + (probabilityExibitionSign * 100) + "%\n";
        
        // configuracao += "Distribuição das placas:";
        // for (int i = 0; i < signDistribution.Length; i++) {
        //     if(i%3 == 0) configuracao += "\t\n";
        //     configuracao += signDistribution[i] + " ";
        // }
        return configuracao;
    }
}
