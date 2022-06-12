using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //varialbles
    [Header("varialbles")]
    private Rigidbody2D rigid;
    private Animator playerAnim;
    private Collider2D coll;
    private CircleCollider2D circleColl;
    [SerializeField] private TilemapCollider2D tileColl2D;
    [SerializeField] private TilemapRenderer groundFrontTilemapRenderer;
    [SerializeField] private List<CapsuleCollider2D> murshroomList = new List<CapsuleCollider2D>();
    [SerializeField] private BoxCollider2D movingPlatformCollider;
    [HideInInspector] private bool canJump = true;
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool lowerLimitLadder = false;
    [HideInInspector] public bool upperLimitLadder = false;
    public Ladder ladderScript;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;
    public BoxCollider2D ladderSurface;
    public GameObject lastSpawnPoint;
    public GameObject lastCheckPoint;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject gameover;

    //Crouch system
    [SerializeField] private Collider2D crouchDisableCollider;
    [SerializeField] private Transform overheadCheckCollider;
    const float overheadCheckRadius = 0.2f;

    //FSM
    [HideInInspector] public enum State { idle, running, jumping, falling, hurt, crouch, climb, death };
    [HideInInspector] public State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask TreeLog;
    [SerializeField] private LayerMask Ladder;
    [SerializeField] private LayerMask MovingPlatform;
    [SerializeField] private LayerMask GroundFront;

    //
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float hurtForce = 7f;

    [SerializeField] private ParticleSystem dust;

    //Audio
    [Header("Audio")]
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] public AudioSource deathAudio;

    //Load menu after die
    [Header("Menu")]
    public string menuScene;

    public static PlayerController instance;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        circleColl = GetComponent<CircleCollider2D>();
        footstep = GetComponent<AudioSource>();
        naturalGravity = rigid.gravityScale;
        tileColl2D.isTrigger = false;
        ladderSurface.isTrigger = false;
    }
    private void Update()
    {
        if (PlayerUI.perm.sliderAudio != null)
        {
            AudioChange();
        }
        MoveCheck();
        DisableMushroomColl();
        playerAnim.SetInteger("state", (int)state);
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void AudioChange()
    {
        deathAudio.volume = PlayerUI.perm.sliderAudio.value;
        hitAudio.volume = PlayerUI.perm.sliderAudio.value;
        jumpAudio.volume = PlayerUI.perm.sliderAudio.value;
        footstep.volume = PlayerUI.perm.sliderAudio.value;
    }

    private void MoveCheck()
    {
        if (state != State.death)
        {
            if (state == State.climb)
            {
                Climb();
            }
            else if (state != State.hurt)
            {
                Movement();
            }

            //Cross though Tree Log & Disable tree log collider
            if (state == State.jumping)
            {
                tileColl2D.isTrigger = true;
            }
            else if ((state == State.falling || state == State.running || state == State.idle) && circleColl.IsTouchingLayers(TreeLog))
            {
                tileColl2D.isTrigger = false;
            }
            //Cross though & Disable Moving platform
            if (state == State.jumping)
            {
                movingPlatformCollider.isTrigger = true;
            }
            else if ((state == State.falling || state == State.running || state == State.idle) && circleColl.IsTouchingLayers(MovingPlatform))
            {
                movingPlatformCollider.isTrigger = false;
            }

            //Moving system
            if (coll.IsTouchingLayers(Ladder))
            {
                ladderSurface.isTrigger = true;
            }
            else if (Input.GetButtonDown("Crouch") && circleColl.IsTouchingLayers(Ladder))
            {
                ladderSurface.isTrigger = true;
                canClimb = true;
            }
            else if (Input.GetButtonDown("Crouch") && circleColl.IsTouchingLayers(Ground))
            {
                state = State.crouch;
                canJump = false;
                speed = 4f;
                crouchDisableCollider.enabled = false;
            }
            else if ((Input.GetButtonUp("Crouch") || Input.GetButtonDown("Jump")) && circleColl.IsTouchingLayers(Ground))
            {
                if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, Ground))
                {
                    state = State.crouch;
                    canJump = false;
                    speed = 4f;
                    crouchDisableCollider.enabled = false;
                }
                else
                {
                    canJump = true;
                    speed = 7f;
                    crouchDisableCollider.enabled = true;
                    VelocitySate();
                }
            }
            else if (Input.GetButton("Crouch") == false && !Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, Ground))
            {
                canJump = true;
                speed = 7f;
                crouchDisableCollider.enabled = true;
                VelocitySate();
            }
            else if (Input.GetButton("Crouch") && Input.GetKey(KeyCode.Space) && circleColl.IsTouchingLayers(TreeLog))
            {
                tileColl2D.isTrigger = true;
                state = State.falling;
                VelocitySate();
            }
            else if (state != State.crouch)
            {
                VelocitySate();
            }
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            if (PlayerUI.perm.heart > 1)
            {
                hitAudio.Play();
                HeartDecrease(1);
                transform.position = lastCheckPoint.transform.position;
            }
            else if (PlayerUI.perm.heart == 1)
            {
                deathAudio.Play();
                rigid.velocity = Vector2.zero;
                state = State.death;

                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Decrease", true);
                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Increase", false);

                PlayerUI.perm.heart = 5;
                foreach (GameObject gameObject in PlayerUI.perm.hearts)
                {
                    gameObject.GetComponent<Animator>().SetBool("Increase", true);
                    gameObject.GetComponent<Animator>().SetBool("Decrease", false);
                }
                transform.position = lastSpawnPoint.transform.position;
                state = State.idle;
            }

        }
        if (collision.CompareTag("GroundFront"))
        {
            groundFrontTilemapRenderer.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("TreeLog"))
        {
            tileColl2D.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundFront"))
        {
            //groundFrontTilemapRenderer.gameObject.GetComponent<TilemapRenderer>().enabled = state != State.crouch;
            if (state == State.crouch)
            {
                groundFrontTilemapRenderer.gameObject.GetComponent<TilemapRenderer>().enabled = false;
            }

            if (!circleColl.IsTouchingLayers(GroundFront))
            {
                groundFrontTilemapRenderer.gameObject.GetComponent<TilemapRenderer>().enabled = true;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (PlayerUI.perm.heart > 1)
            {
                if (state == State.falling)
                {
                    enemy.JumpedOn();
                    Jump();
                }
                else
                {
                    state = State.hurt;
                    hitAudio.Play();
                    //HeartDecrease(1);
                    rigid.velocity = collision.gameObject.transform.position.x > transform.position.x ?
                        new Vector2(-hurtForce, rigid.velocity.y) : new Vector2(hurtForce, rigid.velocity.y);
                }
            }
            else if (PlayerUI.perm.heart == 1)
            {

                deathAudio.Play();
                rigid.velocity = Vector2.zero;
                state = State.death;

                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Decrease", true);
                PlayerUI.perm.hearts[0].GetComponent<Animator>().SetBool("Increase", false);

                PlayerUI.perm.heart = 5;
                foreach (GameObject gameObject in PlayerUI.perm.hearts)
                {
                    gameObject.GetComponent<Animator>().SetBool("Increase", true);
                    gameObject.GetComponent<Animator>().SetBool("Decrease", false);
                }
                transform.position = lastSpawnPoint.transform.position;
                state = State.idle;
            }
        }
    }


    public void HeartDecrease(int d)
    {
        PlayerUI.perm.heart -= d;
        Animator HeartObj = PlayerUI.perm.hearts[PlayerUI.perm.heart].GetComponent<Animator>();
        HeartObj.SetBool("Decrease", true);
        HeartObj.SetBool("Increase", false);
    }

    private void Movement()
    {
        float hDrection = Input.GetAxis("Horizontal");

        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladderScript.transform.position.x, rigid.position.y);
            rigid.gravityScale = 0f;
        }

        //Moving left
        if (hDrection < 0)
        {
            rigid.velocity = new Vector2((float)(speed * Math.Pow(hDrection, 1)), rigid.velocity.y);
            CreateDust();
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving right
        else if (hDrection > 0)
        {
            rigid.velocity = new Vector2((float)(speed * Math.Pow(hDrection, 1)), rigid.velocity.y);
            CreateDust();
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }


        //Jumping
        if (canJump)
        {
            if (Input.GetButtonDown("Jump") && (circleColl.IsTouchingLayers(Ground) || circleColl.IsTouchingLayers(TreeLog) || circleColl.IsTouchingLayers(MovingPlatform)))
            {
                CreateDust();
                Jump();
            }
        }

        //Coruching
        if (Input.GetButtonDown("Crouch") && (circleColl.IsTouchingLayers(Ground) || circleColl.IsTouchingLayers(TreeLog) || circleColl.IsTouchingLayers(MovingPlatform)))
        {
            state = State.crouch;
            canJump = false;
            speed = 4f;
            crouchDisableCollider.enabled = false;
        }
        else if (Input.GetButtonDown("Jump") && (circleColl.IsTouchingLayers(Ground) || circleColl.IsTouchingLayers(TreeLog) || circleColl.IsTouchingLayers(MovingPlatform)))
        {
            canJump = true;
            state = State.jumping;
            speed = 7f;
            crouchDisableCollider.enabled = true;
            VelocitySate();
        }
    }

    public void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
        jumpAudio.Play();
        state = State.jumping;
    }

    public void Jump2(float jumpheight2)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpheight2);
        jumpAudio.Play();
        state = State.jumping;
    }

    private void VelocitySate()
    {
        if (state == State.climb)
        {

        }
        else if (state == State.jumping)
        {
            if (rigid.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (circleColl.IsTouchingLayers(Ground) || circleColl.IsTouchingLayers(TreeLog) || circleColl.IsTouchingLayers(MovingPlatform))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rigid.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rigid.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    public void DisableMushroomColl()
    {
        if (state != State.falling)
        {
            murshroomList.ForEach(delegate (CapsuleCollider2D capsule)
            {
                capsule.isTrigger = true;
            });
        }
        else
        {
            murshroomList.ForEach(delegate (CapsuleCollider2D capsule)
            {
                capsule.isTrigger = false;
            });
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }
    private void Climb()
    {
        if (Input.GetButtonDown("Jump"))
        {
            state = State.climb;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rigid.gravityScale = naturalGravity;
            playerAnim.speed = 1f;
            Jump();
            return;
        }

        float vDirection = Input.GetAxis("Vertical");

        if (vDirection > .1f && !upperLimitLadder)
        {
            rigid.velocity = new Vector2(0f, vDirection * climbSpeed);
            playerAnim.speed = 1f;
        }
        else if (vDirection < -.1 && !lowerLimitLadder)
        {
            rigid.velocity = new Vector2(0f, vDirection * climbSpeed);
        }
        else
        {
            playerAnim.speed = 0f;
            rigid.velocity = Vector2.zero;
        }

        if (upperLimitLadder == true)
        {
            state = State.idle;

            canClimb = false;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.gravityScale = naturalGravity;
            ladderSurface.isTrigger = false;
            playerAnim.speed = 1f;
            //VelocitySate();
            Movement();
            VelocitySate();
            return;
        }
        else if (lowerLimitLadder == true && (Input.GetButton("Crouch") || Input.GetButtonDown("Crouch")))
        {
            state = State.idle;
            canClimb = false;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.gravityScale = naturalGravity;
            playerAnim.speed = 1f;
            //VelocitySate();
            Movement();
            VelocitySate();
            return;
        }
    }
}
