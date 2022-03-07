using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageController : MonoBehaviour
{
    public void GoToTest()
    {
        Debug.Log("Redirect to: Go To Test Scene");
        SceneManager.LoadScene("TestScene");
    }

    public void Leaderboard()
    {
        Debug.Log("Redirect to: Leaderboard Scene");
        SceneManager.LoadScene("LeaderboardScene");
    }
    
    public void Settings()
    {
        Debug.Log("Redirect to: Settings Scene");
        SceneManager.LoadScene("SettingsScene");
    }

    public void BackToMainMenu()
    {
        Debug.Log("Redirect to: Main Menu Scene");
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GoToGamePlay()
    {
        Debug.Log("Redirect to: Go To Game Play");
        SceneManager.LoadScene("GamePlay");
    }

    public void Exit()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
    
}
