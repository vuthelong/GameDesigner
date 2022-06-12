using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 respawnPoint;
    public bool doorOpen;
    public int goldkey;
    public int diamondkey;
    public int greenkey;
    public int cherry;
    public int carrot;
    public int gem;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        respawnPoint = PlayerController.instance.lastSpawnPoint.transform.position;

        if (SaveManager.instance.hasLoad)
        {
            respawnPoint = SaveManager.instance.activeSave.respawnPosition;
            PlayerController.instance.lastSpawnPoint.transform.position = respawnPoint;
            goldkey = SaveManager.instance.activeSave.goldkey;
            diamondkey = SaveManager.instance.activeSave.diamondkey;
            greenkey = SaveManager.instance.activeSave.greenkey;
            cherry = SaveManager.instance.activeSave.cherry;
            carrot = SaveManager.instance.activeSave.carrot;
            gem = SaveManager.instance.activeSave.gem;
            

        }
        else
        {
            SaveManager.instance.activeSave.goldkey = goldkey;
            SaveManager.instance.activeSave.diamondkey = diamondkey;
            SaveManager.instance.activeSave.greenkey = greenkey;
            SaveManager.instance.activeSave.cherry = cherry;
            SaveManager.instance.activeSave.carrot = carrot;
            SaveManager.instance.activeSave.gem = gem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
