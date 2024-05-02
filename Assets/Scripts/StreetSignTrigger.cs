using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignsTrigger : MonoBehaviour
{
    public float newSignRatio = 0.5f;
    public float meanPosition_x = 7;
    public float meanPosition_y = 25;

    public float standardDeviation = 1;
    public float regionAvaliableWidth = 40;
    public GameObject stopSign;
    public GameObject turnRightSign;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateSigns", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void GenerateSigns()
    {
        float randomValue = Random.value;
        if (randomValue < newSignRatio)
        {   
            Debug.Log("Generating new sign");
            float x = (float)NormalizedRandom(meanPosition_x);
            float y = (float)NormalizedRandom(meanPosition_y);
            Vector3 position = new Vector3(x, y, -5);

            GameObject sign = Random.value > 0.5 ? stopSign : turnRightSign;
            GameObject newSign = Instantiate(sign, position, Quaternion.identity);
            // Rotate the sign
            newSign.transform.Rotate(90, 0, 180);
        }
    }

    public double NormalizedRandom(float mean)
    {
        Debug.Log("Mean: " + mean);
        double randomValue = NextGaussianDouble(0, (2*mean));
        Debug.Log("Random Value: " + randomValue);
        return randomValue;
    }
    public double NextGaussianDouble(float minValue, float maxValue)
    {
        Debug.Log("Min: " + minValue);
        Debug.Log("Max: " + maxValue);
        float u, v, S;

        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        Debug.Log("Std: " + std);
        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        Debug.Log("Mean: " + mean);
        Debug.Log("Sigma: " + sigma);
        Debug.Log("Before: " + std * sigma + mean);

        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }
}
