using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "Level Container")]
public class LevelContainer : ScriptableObject
{
    public List<Level> levels;

    public Level this[int index]
    {
        get
        {
            return levels[index];
        }
    }
}
