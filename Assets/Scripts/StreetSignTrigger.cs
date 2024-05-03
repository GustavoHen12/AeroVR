using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignsTrigger : MonoBehaviour
{
    public float newSignRatio = 0.5f;
    public float meanPosition_x = 7;
    public float meanPosition_y = 25;

    public float standardDeviation = 1;
    public GameObject stopSign;
    public GameObject turnRightSign;

    private float regionWidth = 40, regionHeigh = 40;
    private float regionCenterWidth, regionCenterHeight;
    // Start is called before the first frame update
    void Start()
    {
        Session currentSession = Session.GetInstance();
        LoadProbabilitiArray(currentSession.currentUser.configuration.signDistribution);
        
        regionHeigh = Screen.height / 3;
        regionWidth = Screen.width / 3;
        regionCenterWidth = regionWidth / 2;
        regionCenterHeight = regionHeigh / 2;

        newSignRatio = currentSession.currentUser.configuration.probabilityExibitionSign;
        InvokeRepeating("GenerateSigns", 0f, currentSession.currentUser.configuration.timeExibitionSign);
    }

    void Update()
    {

        
    }

    public void GenerateSigns()
    {
        float randomValue = Random.value;
        if (randomValue < newSignRatio) {
            Vector3 position = GetPostionNewSign();

            GameObject sign = Random.value > 0.5 ? stopSign : turnRightSign;
            GameObject newSign = Instantiate(sign, position, Quaternion.identity);
            // Rotate the sign
            newSign.transform.Rotate(90, 0, 180);
        }
    }

    private Vector3 GetPostionNewSign(){
        int index = SelectPosition();
        int row = index / 3; // Grid 3x3
        int column = index % 3;

        // Get center point of the region
        float x = (column - 1) * regionCenterWidth;
        float y = (row - 1) * regionCenterHeight;

        // Get random point inside the region
        float randomX = Random.Range(x - regionWidth / 2, x + regionWidth / 2);
        float randomY = Random.Range(y - regionHeigh / 2, y + regionHeigh / 2);

        return new Vector3(randomX, randomY, -5);
    }
    int [] probabilities;
    int totalPositions;
    private void LoadProbabilitiArray(int[] positionProbabilities) {
        int total = 0;
        foreach (int prob in positionProbabilities) {
            total += prob;
        }

        probabilities = new int[total];
        int index = 0;
        foreach (int prob in positionProbabilities) {
            for (int i = 0; i < prob; i++) {
                probabilities[index] = i;
                index++;
            }
        }

        totalPositions = total;
    }
    public int SelectPosition() {
        int randomValue = UnityEngine.Random.Range(0, totalPositions);
        return probabilities[randomValue];
    }
}
