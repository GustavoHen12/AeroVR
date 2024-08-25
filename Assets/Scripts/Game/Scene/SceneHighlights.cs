using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SceneHighlight {
    public List<SceneObject> roads;
    public SceneObject player;
    public List<SceneObject> sign;
    public string type;

    public SceneHighlight() {
        this.roads = new List<SceneObject>();
        this.sign = new List<SceneObject>();
    }

    public void setPlayer(SceneObject player) {
        this.player = player;
    }

    public void addRoad(SceneObject road) {
        this.roads.Add(road);
    }

    public void addSign(SceneObject sign) {
        this.sign.Add(sign);
    }
}