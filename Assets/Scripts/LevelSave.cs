using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSave
{
    private int currentLevel;

    public void setLevel(int level) 
    {
        currentLevel = level;
    }

    public int getLevel() 
    {
        return currentLevel;
    }
}
