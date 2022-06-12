using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    [SerializeField] public GameObject Inventory;
    public AudioSource buttonFX;

    public string menuScene;

    [System.Obsolete]
    void Update()
    {

        if (Time.timeScale == 1f)
        {
            pauseMenuUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.active)
            {
                Inventory.SetActive(false);
            }
            else if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryPannel();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    [System.Obsolete]
    public void InventoryPannel()
    {
        Inventory.SetActive(true);
    }
    public void ButtonCllick()
    {
        buttonFX.Play();
    }
}
