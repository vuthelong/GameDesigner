using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private bool isJumpedOn = false;
    [SerializeField] private CapsuleCollider2D mushroomcoll;
    [SerializeField] private float jumpHeight;
    private Animator anim;
    private AudioSource JumpedOnAudio;

    // Start is called before the first frame update
    void Start()
    {
        JumpedOnAudio = GetComponent<AudioSource>();
        mushroomcoll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {

        if (PlayerUI.perm.sliderAudio != null)
        {
            JumpedOnAudio.volume = PlayerUI.perm.sliderAudio.value;
        }
        if (isJumpedOn)
            anim.SetBool("isJumpedOn", true);
        else
            anim.SetBool("isJumpedOn", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.state == PlayerController.State.falling)
            {
                player.Jump2(jumpHeight);
                isJumpedOn = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isJumpedOn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isJumpedOn = false;
        }
    }

    private void JumpAudio()
    {
        JumpedOnAudio.Play();
    }
}
