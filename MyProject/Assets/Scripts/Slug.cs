using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : Enemy
{
    [SerializeField] GameObject rightCap;
    [SerializeField] GameObject leftCap;
    [SerializeField] private float speed;
    [SerializeField] private Collider2D coll;
    private bool facingLeft = true;

    protected override void Start()
    {
        rightCap.SetActive(false);
        leftCap.SetActive(false);
        base.Start();
        coll = GetComponent<UnityEngine.Collider2D>();
    }

    private void SlugMove()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap.transform.position.x)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 0);
                }
                rb.velocity = new Vector2(-speed, 0);
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
                    transform.localScale = new Vector3(-1, 1, 0);
                }

                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
