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
}
