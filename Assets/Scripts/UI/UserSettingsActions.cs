using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UserSettingsActions : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject signMenu;
    [SerializeField] private GameObject signDistributionMenu;

    private JSONDataManager jsonDataManager;

    private Session currentSession;
    private User currentUser;

    void Start() {
        jsonDataManager = new JSONDataManager();

        currentSession = Session.GetInstance();
        currentUser = User.LoadCurrentUser();
        LoadGeneralConfigurations();
        LoadSignConfigurations();
        LoadSignDistribution();
    }

    public void NavigateSettingsMenu() {
        settingMenu.SetActive(true);
        signMenu.SetActive(false);
        signDistributionMenu.SetActive(false);
    }

    public void NavigateSignMenu() {
        settingMenu.SetActive(false);
        signMenu.SetActive(true);
        signDistributionMenu.SetActive(false);
    }

    public void NavigateSignDistributionMenu() {
        settingMenu.SetActive(false);
        signMenu.SetActive(false);
        signDistributionMenu.SetActive(true);
    }

    public void NavigateMainMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update values
    public void ToggleValueChanged(Toggle toogle) {
        if (toogle == null) {
            return;
        }

        Debug.Log("Toggle changed to: " + toogle.isOn);
        currentUser.configuration.vrMode = !!toogle.isOn;
    }

    public void MatchDurationValueChanged(Slider slider) {
        if (slider == null) {
            return;
        }

        // Update the match duration value
        Debug.Log("Match duration changed to: " + slider.value);
        currentUser.configuration.matchDuration = ConvertStringToInt(slider.value + "");

        TMP_Text matchDurationText = settingMenu.GetComponentsInChildren<TMP_Text>()[1];
        matchDurationText.text = "Duração da partida: " + currentUser.configuration.matchDuration + " min";
    }

    public void TimeExibitionSignValueChanged(TMP_InputField input) {
        if (input == null) {
            return;
        }

        Debug.Log("Time exibition sign changed to: " + input.text);
        currentUser.configuration.timeExibitionSign = ConvertStringToInt(input.text + "");
    }

    public void SpeedValueChanged(TMP_Dropdown dropdown) {
        // Update the speed value
        float speed = 1f;
        Debug.Log(dropdown.value);
        if(dropdown.value != 0){
            speed =  float.Parse(dropdown.options[dropdown.value].text);
            while(speed > 10){
                speed /= 10f;
            }
        }

        Debug.Log("Speed changed to: " + speed);
        currentUser.configuration.speed = speed;
    }

    public void ForestDensityValueChanged(TMP_Dropdown dropdown) {
        currentUser.configuration.forestDensity = ConvertStringToInt(dropdown.options[dropdown.value].text);
    }

    public void TimeBetweenSignsValueChanged(TMP_Dropdown dropdown) {
        // Update the time exibition sign value
        Debug.Log("Time between sign changed to: " + ConvertStringToInt(dropdown.options[dropdown.value].text));
        currentUser.configuration.timeBetweenSigns = ConvertStringToInt(dropdown.options[dropdown.value].text);
    }

    public void ProbabilityExibitionSignValueChanged(TMP_Dropdown dropdown) {
        // Update the probability exibition sign value
        Debug.Log("Probability exibition sign changed to: " + ConvertStringToInt(dropdown.options[dropdown.value].text)/100f);
        currentUser.configuration.probabilityExibitionSign = ConvertStringToInt(dropdown.options[dropdown.value].text)/100f;
    }

    public void SignSizeValueChanged(TMP_Dropdown dropdown) {
        currentUser.configuration.signSize = dropdown.options[dropdown.value].text;
    }

    public void SignValueChanged(TMP_Dropdown dropdown) {
        // Update the sign distribution value
        // get component name
        int index = getIndexFromDropdownName(dropdown.name);
        Debug.Log(index + ": " + ConvertStringToInt(dropdown.options[dropdown.value].text));
        currentUser.configuration.signDistribution[index] = ConvertStringToInt(dropdown.options[dropdown.value].text);
    }

    public void SaveSettings() {
        // Save the current user settings
        currentUser.Save();
        NavigateMainMenu();
    }

    public void LoadGeneralConfigurations() {
        Toggle vrModeToggle = settingMenu.GetComponentInChildren<Toggle>();
        vrModeToggle.isOn = currentUser.configuration.vrMode;

        Slider matchDurationSlider = settingMenu.GetComponentInChildren<Slider>();
        matchDurationSlider.value = currentUser.configuration.matchDuration;

        TMP_Dropdown speedDropdown = settingMenu.GetComponentsInChildren<TMP_Dropdown>()[0];
        int speedDropdownValue = speedDropdown.options.FindIndex(option => (option.text != "Normal") && float.Parse(option.text) == currentUser.configuration.speed);
        speedDropdown.value = speedDropdownValue > 0 ? speedDropdownValue : 0;

        TMP_Dropdown forestDropdown = settingMenu.GetComponentsInChildren<TMP_Dropdown>()[1];
        forestDropdown.value = forestDropdown.options.FindIndex(option => ConvertStringToInt(option.text) == currentUser.configuration.forestDensity);
    }

    public void LoadSignConfigurations() {
        TMP_InputField timeExibitionSignInput = signMenu.GetComponentInChildren<TMP_InputField>();
        timeExibitionSignInput.text = (currentUser.configuration.timeExibitionSign) + "";

        TMP_Dropdown timeBetweenSignDropdown = signMenu.GetComponentsInChildren<TMP_Dropdown>()[0];
        timeBetweenSignDropdown.value = timeBetweenSignDropdown.options.FindIndex(option => ConvertStringToInt(option.text) == (currentUser.configuration.timeBetweenSigns));

        TMP_Dropdown probabilityExibitionSignDropdown = signMenu.GetComponentsInChildren<TMP_Dropdown>()[1];
        probabilityExibitionSignDropdown.value = probabilityExibitionSignDropdown.options.FindIndex(option => ConvertStringToInt(option.text) == (currentUser.configuration.probabilityExibitionSign*100));

        TMP_Dropdown signSizeDropdown = signMenu.GetComponentsInChildren<TMP_Dropdown>()[2];
        signSizeDropdown.value = signSizeDropdown.options.FindIndex(option => option.text == currentUser.configuration.signSize);
    }

    public void LoadSignDistribution() {
        TMP_Dropdown[] dropdowns = signDistributionMenu.GetComponentsInChildren<TMP_Dropdown>();
        for(int i = 0; i < dropdowns.Length; i++) {
            TMP_Dropdown dropdown = dropdowns[i];
            int index = getIndexFromDropdownName(dropdown.name);

            dropdown.value = dropdown.options.FindIndex(option => ConvertStringToInt(option.text) == currentUser.configuration.signDistribution[index]);
        }
    }

    private int getIndexFromDropdownName(string dropdown_name) {
        string[] name = dropdown_name.Split('_');
        int row = (int.Parse(name[0]) / 10) - 1;
        int column = (int.Parse(name[0]) % 10) - 1;
        return row * 5 + column;
    }

    private static int ConvertStringToInt(string input) {
        int result = 0;
        string numericPart = Regex.Replace(input, "[^0-9]", "");
        int.TryParse(numericPart, out result);
        return result;
    }
}
