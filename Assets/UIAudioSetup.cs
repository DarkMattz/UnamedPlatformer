using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioSetup : MonoBehaviour
{

    AudioLevel audioLevel;

    // Start is called before the first frame update
    public void Start()
    {
        AudioLevel audioLevel = AudioLevel.getInstance();
        transform.GetChild(0).GetComponent<AudioSource>().volume = audioLevel.getSfxLevel();
        transform.GetChild(1).GetComponent<AudioSource>().volume = audioLevel.getSfxLevel();
    }

}
