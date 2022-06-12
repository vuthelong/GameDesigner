using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private GameObject leftCap;
    [SerializeField] private GameObject rightCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private CircleCollider2D circleCollider2D;

    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        leftCap.SetActive(false);
        rightCap.SetActive(false);
        base.Start();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if (circleCollider2D.IsTouchingLayers(Ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }
    }


    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap.transform.position.x)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (circleCollider2D.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap.transform.position.x)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (circleCollider2D.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

}
