using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignsTrigger : MonoBehaviour
{
    public float newSignRatio = 0.5f;
    private float signSize = 0;
    public GameObject[] signs;
    
    private GameStatus gameStatus;
    private float regionWidth = 40, regionHeigh = 40;
    private float up = 20, left = -35, down = 0, right = 35;

    private int columns = 5, rows = 3;
    // Start is called before the first frame update
    void Start()
    {
        gameStatus = GameStatus.GetInstance();
        User currentUser = User.LoadCurrentUser();
        Configuration configuration = currentUser.GetConfiguration();

        LoadProbabilitiArray(configuration.signDistribution);
        
        float height = up - down;
        float width = right - left;
        regionHeigh = height / rows;
        regionWidth = width / columns;
        
        newSignRatio = configuration.probabilityExibitionSign;

        string configSize = configuration.signSize;
        if(configSize == "Grande") {
            signSize = 1.5f;
        } else if(configSize == "Muito grande") {
            signSize = 2f;
        } else {
            signSize = 1;
        }

        InvokeRepeating("GenerateSigns", 0f, configuration.timeBetweenSigns);
    }

    void Update() { }

    public void GenerateSigns()
    {
        float randomValue = Random.value;
        if (randomValue < newSignRatio) {
            Vector3 position = GetPostionNewSign();

            // Get last sign
            int lastSignId = -1;
            if(gameStatus.signs.Count > 0){
                SignExibitionData lastSignData = gameStatus.signs[gameStatus.signs.Count - 1];
                lastSignId = lastSignData.sign-1;
            }

            int signId = Random.Range(0, signs.Length);
            while(signId == lastSignId){
                signId = Random.Range(0, signs.Length);
            }

            GameObject sign = signs[signId];
            GameObject newSign = Instantiate(sign, position, Quaternion.identity);
            // Rotate the sign
            newSign.transform.Rotate(90, 0, 180);
            newSign.transform.localScale = new Vector3(signSize, signSize, signSize);

            SignExibitionData signData = new SignExibitionData();
            signData.sign = signId+1;
            signData.timestamp = Time.time - gameStatus.gameStartTime;
            signData.x = position.x;
            signData.y = position.y;
            signData.z = position.z;
            gameStatus.signs.Add(signData);
        }
    }

    private Vector3 GetPostionNewSign(){
        int index = SelectPosition();
        int row = index / rows; // Grid 3x3
        int column = index % columns;

        // // Get center point of the region
        float x = (left + (column * regionWidth)) + regionWidth / 2;
        float y = (up - (row * regionHeigh)) - regionHeigh / 2;


        // // Get random point inside the region
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
        int index = 0, position = 0;
        foreach (int prob in positionProbabilities) {
            for (int i = 0; i < prob; i++) {
                probabilities[index] = position;
                index++;
            }

            position++;
        }

        totalPositions = total;
    }
    public int SelectPosition() {
        int randomValue = UnityEngine.Random.Range(0, totalPositions);
        return probabilities[randomValue];
    }
}
