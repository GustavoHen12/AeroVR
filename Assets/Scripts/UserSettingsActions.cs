using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UserSettingsActions : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject signDistributionMenu;

    private JSONDataManager jsonDataManager;

    private Session currentSession;

    void Start() {
        jsonDataManager = new JSONDataManager();
        jsonDataManager.Awake();

        currentSession = jsonDataManager.LoadData<Session>("session");
        if(currentSession == null) {
            currentSession = new Session();
            currentSession.activeUsersId = new List<int>();
            jsonDataManager.SaveData<Session>(currentSession, "session");
        }

        LoadSettings();
    }

    public void NavigateSettingsMenu() {
        settingMenu.SetActive(true);
        signDistributionMenu.SetActive(false);
    }

    public void NavigateSignDistributionMenu() {
        settingMenu.SetActive(false);
        signDistributionMenu.SetActive(true);
    }

    public void NavigateMainMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update values
    public void ToggleValueChanged(Toggle toogle) {
        // Check if the toggle is on or off
        Debug.Log("Toggle changed to: " + toogle.isOn);
        currentSession.currentUser.configuration.vrMode = toogle.isOn;
    }

    public void MatchDurationValueChanged(Slider slider) {
        // Update the match duration value
        Debug.Log("Match duration changed to: " + slider.value);
        currentSession.currentUser.configuration.matchDuration = ConvertStringToInt(slider.value + "");
    }

    public void SpeedValueChanged(TMP_Dropdown dropdown) {
        // Update the speed value
        float speed = 1f;
        Debug.Log(dropdown.value);
        if(dropdown.value != 0){
            speed =  float.Parse(dropdown.options[dropdown.value].text);
        }

        Debug.Log("Speed changed to: " + speed);
        currentSession.currentUser.configuration.speed = speed;
    }

    public void TimeExibitionSignValueChanged(TMP_Dropdown dropdown) {
        // Update the time exibition sign value
        Debug.Log("Time exibition sign changed to: " + ConvertStringToInt(dropdown.options[dropdown.value].text));
        currentSession.currentUser.configuration.timeExibitionSign = ConvertStringToInt(dropdown.options[dropdown.value].text);
    }

    public void ProbabilityExibitionSignValueChanged(TMP_Dropdown dropdown) {
        // Update the probability exibition sign value
        Debug.Log("Probability exibition sign changed to: " + ConvertStringToInt(dropdown.options[dropdown.value].text)/100f);
        currentSession.currentUser.configuration.probabilityExibitionSign = ConvertStringToInt(dropdown.options[dropdown.value].text)/100f;
    }

    public void SignValueChanged(TMP_Dropdown dropdown) {
        // Update the sign distribution value
        // get component name
        int index = getIndexFromDropdownName(dropdown.name);
        Debug.Log(index + ": " + ConvertStringToInt(dropdown.options[dropdown.value].text));
        currentSession.currentUser.configuration.signDistribution[index] = ConvertStringToInt(dropdown.options[dropdown.value].text);
    }

    public void SaveSettings() {
        // Save the current user settings
        jsonDataManager.SaveData<Session>(currentSession, "session");
        User user = currentSession.currentUser;
        jsonDataManager.SaveData<User>(user, "user_" + user.userId);
        Debug.Log("Settings saved");

        NavigateMainMenu();
    }

    public void LoadSettings() {
        Toggle vrModeToggle = settingMenu.GetComponentInChildren<Toggle>();
        vrModeToggle.isOn = currentSession.currentUser.configuration.vrMode;

        Slider matchDurationSlider = settingMenu.GetComponentInChildren<Slider>();
        matchDurationSlider.value = currentSession.currentUser.configuration.matchDuration;

        TMP_Dropdown speedDropdown = settingMenu.GetComponentsInChildren<TMP_Dropdown>()[0];
        int speedDropdownValue = speedDropdown.options.FindIndex(option => (option.text != "Normal") && float.Parse(option.text) == currentSession.currentUser.configuration.speed);
        speedDropdown.value = speedDropdownValue > 0 ? speedDropdownValue : 0;

        TMP_Dropdown timeExibitionSignDropdown = settingMenu.GetComponentsInChildren<TMP_Dropdown>()[1];
        timeExibitionSignDropdown.value = timeExibitionSignDropdown.options.FindIndex(option => ConvertStringToInt(option.text) == currentSession.currentUser.configuration.timeExibitionSign);

        TMP_Dropdown probabilityExibitionSignDropdown = settingMenu.GetComponentsInChildren<TMP_Dropdown>()[2];
        probabilityExibitionSignDropdown.value = probabilityExibitionSignDropdown.options.FindIndex(option => ConvertStringToInt(option.text) == (currentSession.currentUser.configuration.probabilityExibitionSign*100));

        LoadSignDistribution();
    }

    public void LoadSignDistribution() {
        TMP_Dropdown[] dropdowns = signDistributionMenu.GetComponentsInChildren<TMP_Dropdown>();
        for(int i = 0; i < dropdowns.Length; i++) {
            TMP_Dropdown dropdown = dropdowns[i];
            int index = getIndexFromDropdownName(dropdown.name);

            dropdown.value = dropdown.options.FindIndex(option => ConvertStringToInt(option.text) == currentSession.currentUser.configuration.signDistribution[index]);
        }
    }

    private int getIndexFromDropdownName(string dropdown_name) {
        string[] name = dropdown_name.Split('_');
        int row = (int.Parse(name[0]) / 10) - 1;
        int column = (int.Parse(name[0]) % 10) - 1;
        return row * 3 + column;
    }

    private static int ConvertStringToInt(string input) {
        int result = 0;
        string numericPart = Regex.Replace(input, "[^0-9]", "");
        int.TryParse(numericPart, out result);
        return result;
    }
}
