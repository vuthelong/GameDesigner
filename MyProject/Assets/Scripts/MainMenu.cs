using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private string sceneName;
    private void Start()
    {
        Time.timeScale = 1;
        string dataPath = Application.persistentDataPath;

        if (SaveManager.instance.DetectSaveFile())
        {
            resumeButton.SetActive(true);
        }
        else
        {
            resumeButton.SetActive(false);
        }

    }


    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
