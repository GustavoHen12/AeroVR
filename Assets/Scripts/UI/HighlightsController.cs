using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighlightsController : MonoBehaviour
{
    private MatchData matchData;
    private SceneUtil sceneUtil;
    public GameObject[] signs;
    public GameObject timerText;
    public GameObject infoText;
    public GameObject camera;
    
    private int currentHighlightIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        JSONDataManager jsonDataManager = new JSONDataManager();

        // Get last game data
        User currentUser = User.LoadCurrentUser();
        string fileName = "game_status_" + currentUser.userId + "_" + currentUser.gamesPlayed;
        Debug.Log("Loading game status from file: " + fileName);
        GameStatus gameStatus = jsonDataManager.LoadData<GameStatus>(fileName);

        matchData = gameStatus.matchData; 

        // Get last game highlights
        sceneUtil = GameObject.Find("GameManager").GetComponent<SceneUtil>();
        LoadHighlight();
    }

    // Update is called once per frame
    void Update(){}

    public void nextHighlight() {
        if(currentHighlightIndex >= matchData.highlights.Count - 1) {
            return;
        }
        currentHighlightIndex++;
        ClearObjects();
        LoadHighlight();
    }

    public void previousHighlight() {
        if(currentHighlightIndex <= 0) {
            return;
        }
        currentHighlightIndex--;
        ClearObjects();
        LoadHighlight();
    }

    public void goToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHistory");
    }

    public void ClearObjects () {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            // Road position
            if (obj.tag == "Road") {
                obj.SetActive(false);
            }
            // Signs position
            if (obj.tag == "Sign") {
                obj.SetActive(false);
            }
        }

        camera.transform.position = new Vector3(0, 4, -30);
    }
    public void LoadHighlight() {
        SceneHighlight sceneHighlight = matchData.highlights[currentHighlightIndex];
        Debug.Log("Loading highlight: " + currentHighlightIndex);

        // Set player position
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(sceneHighlight.player.Position.x, sceneHighlight.player.Position.y, sceneHighlight.player.Position.z);

        // Set camera position 
        camera.transform.position = new Vector3(sceneHighlight.player.Position.x, 4, -30);

        // Set sign position
        List<SceneObject> highlight_signs = sceneHighlight.sign;
        int signIndex = 0;
        foreach(SceneObject sign in highlight_signs) {
            int id = sign.id;
            if(id > 0) {
                signIndex = id;
                GameObject newSign = Instantiate(signs[id], sign.Position, Quaternion.identity);
                newSign.transform.position = sign.Position;
                newSign.transform.Rotate(90, 0, 180);
                newSign.transform.localScale = sign.Scale;

                Destroy(newSign.GetComponent<SelfDestroy>());
            }
        }

        // Set road
        List<SceneObject> roads = sceneHighlight.roads;
        foreach(SceneObject road in roads) {
            int id = road.id;
            List<SceneObject> roadObjects = matchData.roads[id].roads;
            GameObject roadInstance = sceneUtil.InstantiateRoad(roadObjects, id == 0 ? 50 : 193f);

            roadInstance.transform.position = road.Position;
        }

        // Add info text
        if(sceneHighlight.type == "new_sign") {
            infoText.GetComponent<TMP_Text>().text = "Placa exibida: " + (signIndex + 1);
        } else {
            infoText.GetComponent<TMP_Text>().text = "Colisão!";
        }        
    }
}
