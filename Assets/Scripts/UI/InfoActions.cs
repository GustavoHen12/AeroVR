using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoActions : MonoBehaviour
{

    void Start() {

    }


    public void NavigateMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
