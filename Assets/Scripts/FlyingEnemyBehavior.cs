using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class FlyingEnemyBehavior : MonoBehaviour
{
    private Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
