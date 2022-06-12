using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool isOn = false;
    [SerializeField] private BoxCollider2D interactRange;
    [SerializeField] private BoxCollider2D gateColl;
    [SerializeField] private GameObject keyPanel;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private AudioSource audioDoor;
    // Start is called before the first frame update
    void Start()
    {
        audioDoor = GetComponent<AudioSource>();
        if (SaveManager.instance.hasLoad)
        {
            isOn = SaveManager.instance.activeSave.doorOpen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
        {
            audioDoor.volume = PlayerUI.perm.sliderAudio.value;
        }

        if(isOn)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            keyPanel.SetActive(true);

            if (PlayerUI.perm.diamondKey == 1 && PlayerUI.perm.greenKey == 1 && PlayerUI.perm.goldKey == 1)
            {
                textPanel.SetActive(true);
                if (Input.GetKeyUp(KeyCode.E))
                {
                    
                    PlayerUI.perm.diamondKey = PlayerUI.perm.diamondKey = PlayerUI.perm.diamondKey = 0;
                    AudioDoorPlay();
                    gameObject.SetActive(false);
                    GameManager.instance.goldkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.activeSave.goldkey = PlayerUI.perm.goldKey;
                    GameManager.instance.diamondkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.activeSave.diamondkey = PlayerUI.perm.goldKey;
                    GameManager.instance.greenkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.activeSave.greenkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.activeSave.doorOpen = true;
                    SaveManager.instance.Save();
                    isOn = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            keyPanel.SetActive(false);
        }
    }

    private void AudioDoorPlay()
    {
        audioDoor.Play();
    }
}
