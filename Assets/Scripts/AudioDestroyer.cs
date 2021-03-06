using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
