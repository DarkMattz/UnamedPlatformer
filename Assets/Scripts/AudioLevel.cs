using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioLevel
{
    private static AudioLevel instance = null;
    private float sfxLevel;
    private float bgmLevel;

    public static AudioLevel getInstance()
    {
        if (instance == null)
        {
            instance = new AudioLevel();
        }
        return instance;
    }

    public float getBgmLevel() 
    {
        return bgmLevel;
    }

    public float getSfxLevel()
    {
        return sfxLevel;
    }

    public void setBgmLevel(float level)
    {
        bgmLevel = level;
    }

    public void setSfxLevel(float level)
    {
        sfxLevel = level;
    }
}
