using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private int[] totalJump;
    [SerializeField] private bool mouseVisibility;
    [SerializeField] private float groundCheckExtention;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int frameRate;
    [SerializeField] private Sprite jumpSprite;
    private SpriteRenderer spriteRenderer;
    private AudioSource walkingAudio;
    private Animator animator;
    private float movHorizontal, movVertical;
    private bool doJump;
    private bool isGrounded;
    private Rigidbody2D playerBody;
    private PolygonCollider2D playerCollider;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        walkingAudio = transform.GetChild(2).GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerCollider = GetComponent<PolygonCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update() //Graphics oriented
    {
        playerBody.gravityScale = gravity;
        Cursor.visible = mouseVisibility;
        Application.targetFrameRate = frameRate;

        
        //Get all input
        movHorizontal = Input.GetAxisRaw("Horizontal");
        movVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            doJump = true;
            if (walkingAudio.isPlaying)
            {
                walkingAudio.Stop();
            }
        }

        //Flip Character
        if (movHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        //While Moving
        if (movHorizontal != 0 && isGrounded)
        {
            animator.SetBool("isMoving", true);
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        else 
        {
            animator.SetBool("isMoving", false);
            if (walkingAudio.isPlaying)
            {
                walkingAudio.Stop();
            }
        } 
    }

    private void FixedUpdate() //Physics Oriented
    {
        checkGround();
        if (doJump && (totalJump[0] > 0))
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            playerBody.AddForce(new Vector2(0, jumpHeight * 100));
            totalJump[0]--;
            doJump = false;
        }
        
        playerBody.velocity = new Vector2(movHorizontal * horizontalSpeed * (Time.fixedDeltaTime * 100), playerBody.velocity.y);
    }

    private void checkGround()
    {
        bool boxCastChecking = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, groundCheckExtention, layerMask);
        //Physics.BoxCast(playerCollider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        if (boxCastChecking)
        {
            animator.SetBool("isJump", false);
            totalJump[0] = totalJump[1];
            isGrounded = true;
        }
        else
        {
            animator.SetBool("isJump", true);
            isGrounded = false;
        }
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        //if (boxCastChecking)
        //{
        //    Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
        //    Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        //}
        //else
        //{
        //    Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
        //    Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        //}
    }

    void ApplyDamage(int damage) 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
