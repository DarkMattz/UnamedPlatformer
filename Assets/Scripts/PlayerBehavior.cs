using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private float gravity = 15;
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private int[] totalJump = {2,2};
    [SerializeField] private bool mouseVisibility = false;
    [SerializeField] private float groundCheckExtention;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int frameRate = 60;
    private float movHorizontal, movVertical;
    private bool isJump;
    private bool isGrounded;
    private Rigidbody2D playerBody;
    private Collider2D playerCollider;
    private Transform groundCheck;

    private void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Update() //perframe
    {
        playerBody.gravityScale = gravity;
        Cursor.visible = mouseVisibility;
        Application.targetFrameRate = frameRate;

        //Get all input
        movHorizontal = Input.GetAxisRaw("Horizontal");
        movVertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
        checkGround();
    }

    private void FixedUpdate() //50 kali perdetik
    {
            if (isJump && (totalJump[0] > 0))
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
                playerBody.AddForce(new Vector2(0, jumpHeight * 100));
                totalJump[0]--;
                isJump = false;
            }

            playerBody.velocity = new Vector2(movHorizontal * horizontalSpeed * (Time.fixedDeltaTime * 100), playerBody.velocity.y);
    }

    private void checkGround()
    {
            RaycastHit2D raycast = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + groundCheckExtention, layerMask);
            Color rayColor;
            if (raycast.collider != null)
            {
                totalJump[0] = totalJump[1];
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + groundCheckExtention), rayColor);
    }
}
