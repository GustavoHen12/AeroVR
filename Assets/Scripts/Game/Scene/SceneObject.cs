using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SceneObject
{
    public string ObjectType;
    public string ObjectName;
    public int id;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    public SceneObject(GameObject gameObject, string objectType = null, int object_id = 0) {
        ObjectType = objectType;
        ObjectName = gameObject.name;
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.eulerAngles;
        Scale = gameObject.transform.localScale;
        id = object_id;
    }

    public override string ToString()
    {
        return $"{ObjectName} ({ObjectType})";
    }
}