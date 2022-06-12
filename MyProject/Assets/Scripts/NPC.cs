using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    [SerializeField] private GameObject panel;
    private AudioSource itemUnlock;

    public enum ItemType { GoldKey, DiamondKey, GreenKey }
    [SerializeField] ItemType Keytype = ItemType.GoldKey;
    // Start is called before the first frame update
    void Start()
    {
        itemUnlock = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            itemUnlock.volume = PlayerUI.perm.sliderAudio.value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (Keytype)
            {
                case ItemType.GoldKey:
                    if (PlayerUI.perm.goldKey == 0)
                    {
                        panel.SetActive(true);
                    }
                    break;
                case ItemType.DiamondKey:
                    if (PlayerUI.perm.diamondKey == 0)
                    {
                        panel.SetActive(true);
                    }
                    break;
                case ItemType.GreenKey:
                    if (PlayerUI.perm.greenKey == 0)
                    {
                        panel.SetActive(true);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }

    public void ButtonClick()
    {
        switch (Keytype)
        {
            case ItemType.GoldKey:
                if (PlayerUI.perm.carrot > 10)
                {
                    PlayerUI.perm.carrot -= 10;
                    PlayerUI.perm.goldKey = 1;
                    SoundPlay();
                    GameManager.instance.carrot = PlayerUI.perm.carrot;
                    GameManager.instance.goldkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.activeSave.carrot = PlayerUI.perm.carrot;
                    SaveManager.instance.activeSave.goldkey = PlayerUI.perm.goldKey;
                    SaveManager.instance.Save();
                }
                break;
            case ItemType.DiamondKey:
                if (PlayerUI.perm.gem > 5)
                {
                    PlayerUI.perm.gem -= 10;
                    PlayerUI.perm.diamondKey = 1;
                    SoundPlay();
                    GameManager.instance.gem = PlayerUI.perm.gem;
                    GameManager.instance.diamondkey = PlayerUI.perm.diamondKey;
                    SaveManager.instance.activeSave.gem = PlayerUI.perm.gem;
                    SaveManager.instance.activeSave.diamondkey = PlayerUI.perm.diamondKey;
                    SaveManager.instance.Save();
                }
                break;
            case ItemType.GreenKey:
                if (PlayerUI.perm.cherry > 5)
                {
                    PlayerUI.perm.cherry -= 5;
                    PlayerUI.perm.greenKey = 1;
                    SoundPlay();
                    GameManager.instance.cherry = PlayerUI.perm.cherry;
                    GameManager.instance.greenkey = PlayerUI.perm.greenKey;
                    SaveManager.instance.activeSave.cherry = PlayerUI.perm.cherry;
                    SaveManager.instance.activeSave.greenkey = PlayerUI.perm.greenKey;
                    SaveManager.instance.Save();
                }
                break;
        }
    }
    private void SoundPlay()
    {
        itemUnlock.Play();
    }
}
