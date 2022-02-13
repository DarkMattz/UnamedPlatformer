using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyBehavior : MonoBehaviour
{
    [SerializeField] private float enemyVelocity;
    [SerializeField] private float enemyRange;
    [SerializeField] private int health;
    [SerializeField] Transform enemyParticle;
    private Animator animator;
    private int killTime;
    private Transform spriteTransform;
    private Animator spriteAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D enemyBody;
    private Transform playerTransform;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        killTime = 100;
        isDead = false;
        enemyParticle = transform.GetChild(1);
        playerTransform = GameObject.Find("Player").transform;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteTransform = transform.GetChild(0);
        spriteAnimator = spriteTransform.GetComponent<Animator>();
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (health < 0) 
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            isDead = true;
        }
    }

    void FixedUpdate()
    {
        //Move Character
        if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= enemyRange && !isDead)
        {
            enemyBody.velocity = new Vector2(enemyVelocity * (Time.fixedDeltaTime * 100) * (playerTransform.position.x - transform.position.x), enemyBody.velocity.y);
            spriteAnimator.SetBool("isMoving", true);
            if (transform.position.x - playerTransform.position.x < 0)
            {
                spriteRenderer.flipX = true;
            } 
            else 
            {
                spriteRenderer.flipX = false;
            }
        }
        else 
        {
            enemyBody.velocity = new Vector2(0,0);
            spriteAnimator.SetBool("isMoving", false);
        }
        if (killTime == 0) 
        {
            Destroy(gameObject);
        } else if(isDead)
        {
            killTime--;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Player") && !isDead) 
        {
            collision.transform.SendMessage("ApplyDamage", 1);
        }
    }

    void ApplyDamage(int damage) 
    {
        health--;
    }
}
