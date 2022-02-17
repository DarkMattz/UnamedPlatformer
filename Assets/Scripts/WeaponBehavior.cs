using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Notes:
    maximum sword radius: 6 unit
 */


public class WeaponBehavior : MonoBehaviour
{
    [SerializeField] private Vector2 movement;
    [SerializeField] private float stopSpeed;
    [SerializeField] private int movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float[] lockAngles;
    [SerializeField] private float usedAngle;
    [SerializeField] private float maximumSwordRadius;
    [SerializeField] private Boolean unlockSword;
    [SerializeField] private float velocityToHitEnemy;
    private Vector2 spriteRendererBound;
    private Transform playerTransform;
    private Rigidbody2D weaponBody;
    private Rigidbody2D playerBody;
    private Vector2 screenBound;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRendererBound = (transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size);
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        playerTransform = GameObject.Find("Player").transform;
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        weaponBody = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        //Cursor movement
        movement.x = Input.GetAxis("Mouse X");
        movement.y = Input.GetAxis("Mouse Y");
        
        
    }

    private void FixedUpdate()
    {
        /*
         * Sword movement
         */

        if (movement != Vector2.zero)
        {
            rotateSword();

            //Change sword velocity
            weaponBody.velocity = Vector2.Lerp(weaponBody.velocity, (new Vector2(movement.x, movement.y) * movementSpeed) + playerBody.velocity, Time.fixedDeltaTime);
        }
        else //Stop sword if no cursor movement detected
        {
            weaponBody.velocity = Vector2.Lerp(weaponBody.velocity, Vector2.zero + playerBody.velocity, Time.fixedDeltaTime * stopSpeed);
        }

    }

    private void LateUpdate()
    {
        //Sword Clamping
        Vector3 newTransformPos = transform.position;
        if (unlockSword)
        {
            newTransformPos.x = Mathf.Clamp(transform.position.x, ((screenBound.x * -1 - spriteRendererBound.x) + Camera.main.transform.position.x), ((screenBound.x + spriteRendererBound.x) + Camera.main.transform.position.x));
            newTransformPos.y = Mathf.Clamp(transform.position.y, ((screenBound.y * -1 - spriteRendererBound.y) + Camera.main.transform.position.y), ((screenBound.y + spriteRendererBound.y) + Camera.main.transform.position.y));
        } else
        {
            newTransformPos =  Vector2.ClampMagnitude(newTransformPos - playerTransform.position, maximumSwordRadius) + (Vector2)playerTransform.position;
        }
        transform.position = newTransformPos;
    }

    private void rotateSword()
    {
        //Get correct sword rotation
        float smallest = float.MaxValue, currAngle = Mathf.Atan2((transform.position.y - playerTransform.position.y), transform.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        foreach (float lockAngle in lockAngles)
        {
            if (smallest > Mathf.Abs(lockAngle - currAngle))
            {
                smallest = Mathf.Abs(lockAngle - currAngle);
                usedAngle = currAngle;
            }
        }

        //Rotate sword
        Quaternion rotation = Quaternion.AngleAxis(usedAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }


    void OnTriggerEnter2D(Collider2D otherObject) 
    {
        if (otherObject.CompareTag("Enemy") == true && weaponBody.velocity.magnitude > velocityToHitEnemy) 
        {
            otherObject.gameObject.SendMessage("ApplyDamage", 1);
        }
    }
}
