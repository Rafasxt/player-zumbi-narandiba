using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public string mainGameSceneName = "MainGameScene";
    public string mainMenuSceneName = "MainMenuScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame chamado");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Retry()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }
}
