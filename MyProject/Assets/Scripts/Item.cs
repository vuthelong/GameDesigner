using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private enum ItemType { cherry, gem, carrot, heart };
    [SerializeField] ItemType type = ItemType.cherry;
    [SerializeField] private AudioSource collectAudio;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        collectAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            collectAudio.volume = PlayerUI.perm.sliderAudio.value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Collected");
        }
    }

    private void StartAnimation()
    {
        switch (type)
        {
            case ItemType.cherry:
                PlayerUI.perm.cherry += 1;
                PlayerUI.perm.cherryText.text = PlayerUI.perm.cherry.ToString();
                collectAudio.Play();
                GameManager.instance.cherry = PlayerUI.perm.cherry;
                SaveManager.instance.activeSave.cherry = PlayerUI.perm.cherry;
                SaveManager.instance.Save();
                break;
            case ItemType.gem:
                PlayerUI.perm.gem += 1;
                PlayerUI.perm.gemText.text = PlayerUI.perm.gem.ToString();
                collectAudio.Play();
                GameManager.instance.gem = PlayerUI.perm.gem;
                SaveManager.instance.activeSave.gem = PlayerUI.perm.gem;
                SaveManager.instance.Save();
                break;
            case ItemType.carrot:
                PlayerUI.perm.carrot += 1;
                PlayerUI.perm.carrotText.text = PlayerUI.perm.carrot.ToString();
                collectAudio.Play();
                GameManager.instance.carrot = PlayerUI.perm.carrot;
                SaveManager.instance.activeSave.carrot = PlayerUI.perm.carrot;
                SaveManager.instance.Save();
                break;
            case ItemType.heart:
                if (PlayerUI.perm.heart == 5)
                {
                    break;
                }
                PlayerUI.perm.heart++;
                Animator gameobject = PlayerUI.perm.hearts[PlayerUI.perm.heart - 1].GetComponent<Animator>();
                gameobject.SetBool("Increase", true);
                gameobject.SetBool("Decrease", false);
                collectAudio.Play();
                break;
        }
    }

    private void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
