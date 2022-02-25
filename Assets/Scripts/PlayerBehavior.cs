using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject pauseUI;
    private AudioLevel audioLevel;
    private SpriteRenderer spriteRenderer;
    private AudioSource walkingAudio;
    private AudioSource hitAudio;
    private AudioSource bgm;
    private Animator animator;
    private float movHorizontal, movVertical;
    private bool doJump;
    private bool releaseJump;
    private bool isGrounded;
    public static Rigidbody2D playerBody;
    private PolygonCollider2D playerCollider;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bgm = transform.GetChild(1).GetComponent<AudioSource>();
        walkingAudio = transform.GetChild(2).GetComponent<AudioSource>();
        hitAudio = transform.GetChild(3).GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerCollider = GetComponent<PolygonCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        audioLevel = AudioLevel.getInstance();
    }

    private void Start()
    {
        walkingAudio.volume = audioLevel.getSfxLevel();
        hitAudio.volume = audioLevel.getSfxLevel();
        bgm.volume = audioLevel.getBgmLevel();
        saveProgress();
        Cursor.visible = mouseVisibility;
    }

    private void Update() //Graphics oriented
    {
        playerBody.gravityScale = gravity;
        Application.targetFrameRate = frameRate;

        if (!StartCutscene.isCutscene)
        {
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
            if(Input.GetKeyUp(KeyCode.Space))
            {
                releaseJump = true;
                doJump = false;
            }
        }
        if(StartCutscene.isCutscene)
        {
            movHorizontal = 0f;
            movVertical = 0f;
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
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpHeight);
            totalJump[0]--;
            doJump = false;
        }

        if (releaseJump)
        {
            if (playerBody.velocity.y > 0)
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y / 3);
            }
            releaseJump = false;
        }
            
        
        playerBody.velocity = new Vector2(movHorizontal * horizontalSpeed * (Time.fixedDeltaTime * 100), playerBody.velocity.y);
    }

    private void checkGround()
    {
        bool boxCastChecking = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, groundCheckExtention, layerMask);
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
            if (totalJump[0] > 0)
                totalJump[0]--;
        }
        Gizmos.color = Color.red;
    }

    void ApplyDamage(int damage) 
    {
        hitAudio.Play();
        pauseUI.SetActive(false);
        deathUI.SetActive(true);
    }

    void saveProgress() 
    {
        LevelSave save = new LevelSave();
        save.setLevel(SceneManager.GetActiveScene().buildIndex);

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/levelSave.snort";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }
}
