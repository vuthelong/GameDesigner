using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource dieSound;

    protected virtual void Start()
    {
        dieSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
            dieSound.volume = PlayerUI.perm.sliderAudio.value;
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<UnityEngine.Collider2D>().enabled = false;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void DieSound()
    {
        dieSound.Play();
    }

}
