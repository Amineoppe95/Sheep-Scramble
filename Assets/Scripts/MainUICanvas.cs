using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainUICanvas : MonoBehaviour
{
    public static MainUICanvas intance;


    public GameObject InGamePanel;
    public GameObject WinPanel;
    public GameObject LosePanel;


    private void Awake()
    {
        intance = this;
    }

    public void LoseLose()
    {
        Time.timeScale = 0;
        InGamePanel.SetActive(false);
        LosePanel.SetActive(true);
    }
    public void WinWin()
    {
        Time.timeScale = 0;
        InGamePanel.SetActive(false);
        WinPanel.SetActive(true);
    }

    public void LoadNextScene()
    {

        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Next scene index is out of range!");
        }
    }

    // Method to reload the current scene
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
