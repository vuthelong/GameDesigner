using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] private string sceneName;
    private AudioSource winSound;
    private void Start()
    {
        winSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            winSound.volume = PlayerUI.perm.sliderAudio.value;
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            winPanel.SetActive(true);
            winSound.Play();
            //SceneManager.LoadScene(sceneName);
            //Auto load Scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
