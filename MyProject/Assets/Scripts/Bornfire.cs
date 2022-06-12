using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bornfire : MonoBehaviour
{
    private BoxCollider2D coll;
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject BornfireText;
    [SerializeField] private Animator SetFire;
    [SerializeField] private AudioSource SetFireSound;
    [SerializeField] private Animator HeartIncrease;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            SetFireSound.volume = PlayerUI.perm.sliderAudio.value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BornfireText.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                SetFireSound.Play();
                SetFire.SetTrigger("SetFire");
                PlayerUI.perm.heart = 5;
                PlayerController player = collision.GetComponent<PlayerController>();
                player.lastCheckPoint = obj;
                player.lastSpawnPoint = obj;
                foreach (GameObject HeartObj in PlayerUI.perm.hearts)
                {
                    HeartObj.gameObject.GetComponent<Animator>().SetBool("Increase", true);
                    HeartObj.gameObject.GetComponent<Animator>().SetBool("Decrease", false);
                    GameManager.instance.respawnPoint = transform.position;
                    SaveManager.instance.activeSave.respawnPosition = transform.position;
                    SaveManager.instance.Save();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BornfireText.gameObject.SetActive(false);
    }
}
