using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    private BoxCollider2D box;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerUI.perm.heart > 1)
            {
                player.HeartDecrease(1);
                player.transform.position = player.lastCheckPoint.transform.position;
            }
            else if (PlayerUI.perm.heart == 1)
            {
                player.deathAudio.Play();
                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Decrease", true);
                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Increase", false);

                PlayerUI.perm.heart = 5;
                foreach (GameObject gameObject in PlayerUI.perm.hearts)
                {
                    gameObject.GetComponent<Animator>().SetBool("Increase", true);
                    gameObject.GetComponent<Animator>().SetBool("Decrease", false);
                }
                player.transform.position = player.lastSpawnPoint.transform.position;
            }
        }
    }
}

