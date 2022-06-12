using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [HideInInspector] private int startingPoint;
    [SerializeField] private Transform[] points;
    [HideInInspector] private int i;
    [SerializeField] private GameObject pos1;
    [SerializeField] private GameObject pos2;
    [SerializeField] private Collider2D coll;
    [SerializeField] private LayerMask player;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        pos1.SetActive(false);
        pos2.SetActive(false);
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        *//*if (Input.GetButton("Jump") || Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
        {
            collision.transform.SetParent(null);
            //return;
        }
        else*//* if (coll.IsTouchingLayers(player))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        *//*if (Input.GetButton("Jump") || Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
        {
            collision.transform.SetParent(null);
            //return;
        }
        else*//* if (coll.IsTouchingLayers(player))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (coll.IsTouchingLayers(player))
        {
            collision.transform.SetParent(null);
        }
    }*/
}
