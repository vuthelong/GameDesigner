using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public int heart = 5;
    public int gem, cherry, carrot, goldKey, diamondKey, greenKey = 0;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI carrotText;


    [SerializeField] private GameObject gemObj;
    [SerializeField] private GameObject gemSlot;
    [SerializeField] private GameObject cherryObj;
    [SerializeField] private GameObject cherrySlot;
    [SerializeField] private GameObject carrotObj;
    [SerializeField] private GameObject carrotSlot;
    [SerializeField] private GameObject goldKeySlot;
    [SerializeField] private GameObject diamondKeySlot;
    [SerializeField] private GameObject greenKeySlot;
    [SerializeField] protected AudioSource bgmusic;
    [SerializeField] public Slider sliderAudio;
    [SerializeField] public Slider sliderBGM;

    [SerializeField] private GameObject fisrtSpawnPoint;
    public static PlayerUI perm;
    public GameObject[] hearts = new GameObject[4];

    public void Awake()
    {
        perm = this;
    }

    private void Start()
    {
        if (SaveManager.instance.hasLoad)
        {
            goldKey = SaveManager.instance.activeSave.goldkey;
            diamondKey = SaveManager.instance.activeSave.diamondkey;
            greenKey = SaveManager.instance.activeSave.greenkey;
            carrot = SaveManager.instance.activeSave.carrot;
            perm.gem = SaveManager.instance.activeSave.gem;
            perm.cherry = SaveManager.instance.activeSave.cherry;
            cherryText.text = cherry.ToString();
            carrotText.text = carrot.ToString();
            gemText.text = gem.ToString();
        }
        else
        {
            goldKey = 0;
            diamondKey = 0;
            greenKey = 0;
            carrot = 0;
            perm.gem = 0;
            perm.cherry = 0;
        }

    }

    private void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            bgmusic.volume = sliderBGM.value;
        NumberCheck();
    }

    public void NumberCheck()
    {
        if (gem <= 0)
        {
            gemSlot.SetActive(false);
        }
        else
        {
            gemSlot.SetActive(true);
            gemObj.SetActive(true);
        }

        if (carrot == 0)
        {
            carrotSlot.SetActive(false);
        }
        else
        {
            carrotSlot.SetActive(true);
            carrotObj.SetActive(true);
        }

        if (cherry == 0)
        {
            cherrySlot.SetActive(false);
        }
        else
        {
            cherrySlot.SetActive(true);
            cherryObj.SetActive(true);
        }

        if (goldKey == 0)
        {
            goldKeySlot.SetActive(false);
        }
        else
        {
            goldKeySlot.SetActive(true);
        }

        if (diamondKey == 0)
        {
            diamondKeySlot.SetActive(false);
        }
        else
        {
            diamondKeySlot.SetActive(true);
        }

        if (greenKey == 0)
        {
            greenKeySlot.SetActive(false);
        }
        else
        {
            greenKeySlot.SetActive(true);
        }
    }
}