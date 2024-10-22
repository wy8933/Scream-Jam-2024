using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsPandel;
    public GameObject CreditsPandel;

    public void OnStartPressed() {
        SceneManager.LoadScene("Kitchen");
    }

    public void OnSettingPressed() {
        SettingsPandel.SetActive(true);
    }

    public void OnCreditsPressed() {
        CreditsPandel.SetActive(true);
    }

    public void OnQuitPressed() { 
        Application.Quit();
    }

    public void OnSettingExitPressed() {
        SettingsPandel.SetActive(false);
    }

    public void OnCreditsExitPressed()
    {
        CreditsPandel.SetActive(false);
    }
}
