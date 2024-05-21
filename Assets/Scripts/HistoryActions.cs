using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Collections;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class HistoryActions : MonoBehaviour
{
    private List<string> motivation = new List<string> {
        "Parabéns! Você mostrou habilidades incríveis!",
        "Excelente trabalho! Sua dedicação foi recompensada.",
        "Fantástico! Você continua superando todos os desafios!",
        "Incrível! Seu talento brilha em cada movimento.",
        "Você é um verdadeiro campeão!",
        "Impressionante! Sua perseverança fez a diferença.",
        "Maravilhoso! Cada passo seu foi impecável.",
        "Bravo! Sua inteligência e coragem foram notáveis.",
        "Fantástico! Parabéns pela determinação!",
        "Sensacional! Você dominou o jogo com maestria.",
        "Seu desempenho foi espetacular!",
        "Uau! Sua habilidade e foco são inspiradores.",
        "Inigualável! Você mostrou o que é ser um verdadeiro jogador.",
        "Magnífico! Sua performance foi excepcional.",
        "Bravo! Sua coragem e habilidade fizeram a diferença.",
        "Parabéns! Você é uma lenda no jogo!",
        "Espetacular! Você jogou como um verdadeiro profissional!",
        "Imbatível! Sua performance foi de outro nível!",
        "Parabéns! Seu talento é inegável!",
        "Formidável! Sua habilidade é impressionante!",
        "Excepcional! Você fez história no jogo!",
        "Incrível! Você é um exemplo de superação!"
    };

    private Camera _mainCamera;
    private const float _defaultFieldOfView = 60.0f;

    private Session currentSession;
    private JSONDataManager jsonDataManager;
    
    private User currentUser;    
    // Start is called before the first frame update
    void Start() {
        _mainCamera = Camera.main;

        jsonDataManager = new JSONDataManager();
        jsonDataManager.Awake();

        string previousScene = SceneController.PreviousScene;
        currentSession = Session.GetInstance();
        
        int userId = currentSession.currentUserId;
        currentUser = jsonDataManager.LoadData<User>("user_" + userId);    
        bool lastGameHistoryMode = previousScene == "VRMode";
        if(lastGameHistoryMode && currentUser.configuration.vrMode){
            StopXR();
        }

        if(!lastGameHistoryMode){
            HideScoreInput();
        }

        loadMotivationMessage();
        loadLastMatchData(lastGameHistoryMode);
    }

    private void StopXR () {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        _mainCamera.ResetAspect();
        _mainCamera.fieldOfView = _defaultFieldOfView;
    }

    private void loadMotivationMessage() {
        string motivationMessage = motivation[Random.Range(0, motivation.Count)];
        TMP_Text motivationText = GameObject.Find("MotivationText").GetComponent<TMP_Text>();
        motivationText.text = motivationMessage;
    }

    private void loadLastMatchData (bool justLastGame) {
        string formatedHistory = "";
        if(justLastGame) {
            string fileName = "game_status_" + currentUser.userId + "_" + currentUser.gamesPlayed;
            GameStatus gameStatus = jsonDataManager.LoadData<GameStatus>(fileName);
            formatedHistory += "DADOS DA PARTIDA:\n" + gameStatus.getString();
            formatedHistory += "\nCONFIGURAÇÕES: \n" + gameStatus.game_settings.getString();
        } else {
            for(int i = currentUser.gamesPlayed; i > 0; i--) {
                GameStatus gameStatus = jsonDataManager.LoadData<GameStatus>("game_status_" + currentUser.userId + "_" + i);
                formatedHistory += "DADOS DA PARTIDA " + i + ":\n" + gameStatus.getString();
                formatedHistory += "\nCONFIGURAÇÕES: \n" + gameStatus.game_settings.getString();
                formatedHistory += "\n\n";
            }
        }

        Debug.Log(formatedHistory);
        TMP_Text historyText = GameObject.Find("HistoryText").GetComponent<TMP_Text>();
        historyText.text = formatedHistory;

        GameObject content = GameObject.Find("Content");
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, historyText.preferredHeight);
        historyText.GetComponent<RectTransform>().sizeDelta = new Vector2(historyText.GetComponent<RectTransform>().sizeDelta.x, historyText.preferredHeight);
    }

    public void NavigateMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void NewGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("VRMode");
    }

    public void SaveScore() {
        // Get score from text input
        TMP_InputField scoreInput = GameObject.Find("ScoreInput").GetComponent<TMP_InputField>();
        int score = int.Parse(scoreInput.text);

        GameStatus gameStatus = jsonDataManager.LoadData<GameStatus>("game_status_" + currentUser.userId + "_" + currentUser.gamesPlayed);
        gameStatus.score = score;
        jsonDataManager.SaveData<GameStatus>(gameStatus, "game_status_" + currentUser.userId + "_" + currentUser.gamesPlayed);

        loadLastMatchData(true);
        HideScoreInput();
    }

    private void HideScoreInput() {
        GameObject scoreInput = GameObject.Find("Score");
        scoreInput.SetActive(false);
    }
}
