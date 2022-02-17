using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemyBehavior : MonoBehaviour
{
    private int killTime;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        killTime = 100;
        isDead = false;
    }

    void FixedUpdate()
    {
        if (killTime == 0)
        {
            Destroy(gameObject);
        }
        else if (isDead)
        {
            killTime--;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.SendMessage("ApplyDamage", 1);
        }
    }
}
