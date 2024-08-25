using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Google.XR.Cardboard;
using UnityEngine.XR;
using UnityEngine.XR.Management;


public class GetReadyVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        User currentUser = User.LoadCurrentUser();

        if(currentUser.configuration.vrMode){
            Cursor.visible = false;
            StartCoroutine(StartXR());            
        } else {
            UnityEngine.SceneManagement.SceneManager.LoadScene("VRMode");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If any button is pressed, go to VRMode scene
        if (Input.anyKey) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("VRMode");
        }
    }

    private IEnumerator StartXR() {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null) {
            Debug.LogError("Initializing XR Failed.");
        } else {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }
}
